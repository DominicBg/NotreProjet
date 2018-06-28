using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class UIPlayerDetectionCanvas : UIState, IUIManager {

	public Canvas playerSelectionCanvas;
	public Canvas[] playSelCanvas;


	public InputDevice[] activeDevices;
	PlayersManager playersManager;

	void Awake(){
		
	}

	// Use this for initialization
	void Start () {

		//Liste des devices en cours d'utilisation dans le manager
		activeDevices = PlayersManager.playersManager.activeDevices;
		playersManager = PlayersManager.playersManager;

		InstantiateCanvas ();

		ResetAllCanvas ();

		//SetCanvas ();
	}

	void FixedUpdate(){
		playersManager.CheckForNewPlayer ();
	}

	void InstantiateCanvas(){
		playerSelectionCanvas = (Canvas)Resources.Load ("Canvas/PlayerSelectionCanvas", typeof (Canvas));

		playSelCanvas = new Canvas[4];
		for (int i = 0; i < playSelCanvas.Length; i++) {

			//Creer les Canvas
			playSelCanvas[i] = Instantiate(playerSelectionCanvas, transform);

			//Dimensionner les Canvas
			RectTransform rect = playSelCanvas [i].GetComponent<RectTransform> ();
			switch (i) {
			case 0:
				// = Screen.width / 2;
				rect.position = new Vector2 (0 + Screen.width / 4, Screen.height / 4 + Screen.height / 2);
				break;
			case 1:
				rect.position = new Vector2(Screen.width / 4 + Screen.width / 2, Screen.height / 4 + Screen.height / 2);
				break;
			case 2: 
				rect.position = new Vector2(Screen.width / 4, Screen.height / 4);
				break;
			case 3:
				rect.position = new Vector2(Screen.width / 4 + Screen.width / 2, Screen.height / 4);
				break;
			}

			//Numeroter les Canvas
			PlayerSelectionCanvas tempPSC = playSelCanvas [i].GetComponent<PlayerSelectionCanvas> ();
			tempPSC.canvasNumber = i;
			tempPSC.parentCanvas = this;
		}
	}

	void ResetAllCanvas(){
		for (int i = 0; i < playSelCanvas.Length; i++){
			PlayerSelectionCanvas tempPSC = playSelCanvas [i].GetComponent<PlayerSelectionCanvas> ();
			tempPSC.SetPressAnyState ();
		}
	}

	public void AllPlayersReady(){
		UIManager.uiManager.ChangeMode (UIManager.uiManager.newGameCanvas);
	}



	public void Enable(){

	}

	public void Disable(){

	}
}
