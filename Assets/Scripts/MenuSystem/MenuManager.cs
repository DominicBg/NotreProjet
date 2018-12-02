﻿using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using InControl;

public class MenuManager : MonoBehaviour
{
	public SplashScreenMenu SplashScreenMenuPrefab;
	public StartMenu StartMenuPrefab;
	public OptionsMenu OptionsMenuPrefab;
	public LobbyMenu LobbyMenuPrefab;
	public NewGameMenu NewGameMenuPrefab;
	public MainMenu MainMenuPrefab;
	public EventSystem eventSystem;
    private Stack<Menu> menuStack = new Stack<Menu>();

    public static MenuManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;

		eventSystem = GetComponent<EventSystem>();

		SplashScreenMenu.Show();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

	public void CreateInstance<T>() where T : Menu
	{
		var prefab = GetPrefab<T>();

		Instantiate(prefab, transform);
	}

	public void OpenMenu(Menu instance)
    {
        // De-activate top menu
        if (menuStack.Count > 0)
        {
			if (instance.DisableMenusUnderneath)
			{
				foreach (var menu in menuStack)
				{
					menu.gameObject.SetActive(false);

					if (menu.DisableMenusUnderneath)
						break;
				}
			}

			
            var topCanvas = instance.GetComponent<Canvas>();
            var previousCanvas = menuStack.Peek().GetComponent<Canvas>();

			topCanvas.sortingOrder = previousCanvas.sortingOrder + 1;
        }
		
		//Set a button of the menu as First Selected
		if(instance.firstButtonSelected != null){
			eventSystem.SetSelectedGameObject(instance.firstButtonSelected); 
		}
        menuStack.Push(instance);
    }

    private T GetPrefab<T>() where T : Menu
    {
        // Get prefab dynamically, based on public fields set from Unity
		// You can use private fields with SerializeField attribute too
        var fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach (var field in fields)
        {
            var prefab = field.GetValue(this) as T;
            if (prefab != null)
            {
                return prefab;
            }
        }

        throw new MissingReferenceException("Prefab not found for type " + typeof(T));
    }
	
	public void CloseMenu(Menu menu)
	{
		if (menuStack.Count == 0)
		{
			Debug.LogErrorFormat(menu, "{0} cannot be closed because menu stack is empty", menu.GetType());
			return;
		}

		if (menuStack.Peek() != menu)
		{
			Debug.LogErrorFormat(menu, "{0} cannot be closed because it is not on top of stack", menu.GetType());
			return;
		}

		CloseTopMenu();		
	}

	public void CloseTopMenu()
    {
        var instance = menuStack.Pop();

		if (instance.DestroyWhenClosed)
        	Destroy(instance.gameObject);
		else
			instance.gameObject.SetActive(false);

        // Re-activate top menu
		// If a re-activated menu is an overlay we need to activate the menu under it
		foreach (var menu in menuStack)
		{
            menu.gameObject.SetActive(true);

			//Set a button of the menu as First Selected
			if(menu.firstButtonSelected != null){
				eventSystem.SetSelectedGameObject(menu.firstButtonSelected); 
			}

			if (menu.DisableMenusUnderneath)
				break;
		}
    }

    private void Update()
    {
        // On Android the back button is sent as Esc
        if (Input.GetKeyDown(KeyCode.Escape) && menuStack.Count > 0)
        {
            menuStack.Peek().OnBackPressed();
        }

		//METTRE CONTROL INCONTROL ICI
		if(InputManager.ActiveDevice.Action2.WasPressed && menuStack.Count > 0){
			menuStack.Peek().OnBackPressed();
		}
    }
}
