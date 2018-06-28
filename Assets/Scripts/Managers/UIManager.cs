using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modes;

public class UIManager : MonoBehaviour {

	public static UIManager uiManager;

	public GameManager gameManager;

	public UIManagerMode uiManagerMode;
	public IUIManager actualUIMode;

	public UIState actualUIState;

	public UIStartCanvas startCanvas;
	public UIPlayerDetectionCanvas playerDetectionCanvas;
	public UINewGameCanvas newGameCanvas;
	public UIInGameCanvas inGameCanvas;
	public UIEndGameCanvas endGameCanvas;
	public UIDisabled disabledCanvas;

	public IUIManager[] availableUIs;
	public GameObject[] availableUIsGO;
	public UIState[] availableUIStates;

	void Awake ()
	{
		DontDestroyOnLoad (gameObject);
		if (uiManager == null) {
			uiManager = this;		
		} else {
			Destroy (gameObject);
		}

		actualUIMode = startCanvas;



	}

	// Use this for initialization
	void Start () {

		GetAllUICanvas ();

		actualUIState = disabledCanvas;

		SetAllInactive ();
		//actualUIMode.Enable ();

		ChangeMode(startCanvas);

	}



	// Update is called once per frame
	void Update () {
		
	}

	void GetAllUICanvas(){
		UIState[] tempUIStates = (UIState[])gameObject.GetComponentsInChildren<UIState> ();
		availableUIStates = tempUIStates;
	}

	void SetAllInactive(){
		foreach (UIState tempCanvas in availableUIStates) {
			tempCanvas.SetCanvasInactive ();
		}
	}

	/*
	//Requete pour changer de mode
	public void ChangeMode(IUIManager newUIMode){

		//Desactiver l'ancien mode
		actualUIMode.Disable ();

		//Activer le nouveau mode
		actualUIMode = newUIMode;

		actualUIMode.Enable ();

	}
	*/ 


	public void ChangeMode(UIState newUIState){

		//Desactiver l'ancien
		actualUIState.SetCanvasInactive();

		actualUIState = newUIState;
		//Activer le nouveau
		actualUIState.SetCanvasActive();
	}





}
