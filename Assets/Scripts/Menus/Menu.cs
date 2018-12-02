using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//We want multiple menus to inherit from our Menu class
public abstract class Menu<T> : Menu where T : Menu<T> {

	//We need to use genereict for the singleton Instance reference
	public static T Instance { get; private set; }
	
	protected virtual void Awake(){
		Instance = (T)this;		
	}

	//Awake and OnDestroy to register and unregiser our instance
	protected virtual void OnDestroy(){
		Instance = null;
	}

	protected static void Open(){
		if (Instance == null)
			MenuManager.Instance.CreateInstance<T>();
		else
			Instance.gameObject.SetActive(true);
		
		MenuManager.Instance.OpenMenu(Instance);
	}

	protected static void Close(){
		if(Instance == null){
			Debug.LogErrorFormat("Trying to close menu {0} but Instance is null", typeof(T));
			return;
		}
		MenuManager.Instance.CloseMenu(Instance);
	}

	//Default implementation for OnBackPressed
	//In most cases, clsing the menu is what we want to do
	public override void OnBackPressed(){
		Close();
	}

	public override void OnAnyButtonPressed(){

	}

}

//We need a non-generic base class to be able to reference all menus
public abstract class Menu : MonoBehaviour {

	public GameObject firstButtonSelected;

	[Tooltip("Destroy the Game Object when menu is closed (reduces memory usage)")]
	public bool DestroyWhenClosed = true;

	[Tooltip("Disable menus that are under this one in the stack")]
	public bool DisableMenusUnderneath = true;

	//We can move the code there that does not depend on the Instance

	public abstract void OnBackPressed();

	public abstract void OnAnyButtonPressed();


	
	
}
