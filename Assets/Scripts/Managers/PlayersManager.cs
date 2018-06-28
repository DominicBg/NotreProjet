using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayersManager : MonoBehaviour {

	private readonly GameManager gameManager;
	public PlayersManager (GameManager gamemanager){
		gameManager = gamemanager;
	}

	public static PlayersManager playersManager;

	//Initializer
	PlayersInitialisation playersInit;

	public int playersNumber;
	public PlayableCharacter playerBase;

	//Pour la visibilite des controlleurs, a supprimer
	public string[] devicesList;

	//Tableau des devices, a supprimer
	public InputDevice[] activeDevices;

	public PlayerConfig[] playersConfig;

	void Awake (){
		//Ne pas detruire le chargement en changeant de scene
		DontDestroyOnLoad (gameObject);

		//Empecher que le Manager se duplique
		if (playersManager == null) {
			playersManager = this;
		} else {
			Destroy (gameObject);
		}

		activeDevices = new InputDevice[4];

	}
	void Start (){
		CheckForPresentsController ();	
	}

	void Update(){
		//CheckForNewPlayer ();
	}

	void CheckForPresentsController(){

		//Lister les devices
		devicesList = new string[InputManager.Devices.Count];
		for (int i = 0; i < devicesList.Length; i++) {
			devicesList [i] = InputManager.Devices [i].Name;
		}
	}

	//Called from UIPlayerDetectionCanvas
	public void CheckForNewPlayer(){
		//Quand une touche est pressee
		if (InputManager.ActiveDevice.AnyButtonIsPressed) {
			bool alreadyExists = false;

			//Verifier si le device est deja assigne a un joueur
			foreach (PlayerConfig pconfig in playersConfig) {
				if (pconfig.controller == InputManager.ActiveDevice)
					alreadyExists = true;				
			}

			if (!alreadyExists) {				
				AddPlayer (InputManager.ActiveDevice);
			}
		}
	}

	void AddPlayer(InputDevice device){

		int positionInArray = 0;

		for (int i = 0; i < playersConfig.Length; i++) {
			if (playersConfig [i].activated == false) {
				positionInArray = i;
				break;
			}
		}

		playersConfig [positionInArray].activated = true;
		playersConfig [positionInArray].playerNumber = positionInArray;
		playersConfig [positionInArray].controller = device;

		//Initialiser ca dans le PLAYERCONFIG
		playersConfig [positionInArray].uiController.controller = device;
		playersConfig [positionInArray].uiController.playerNumber = positionInArray;
		playersConfig [positionInArray].uiController.GetCanvas ();

		playersNumber = positionInArray + 1;			
	}

	public void ConnectCanvasToControllers(UIControllableCanvas controllableCanvas){
		foreach (PlayerConfig conf in playersConfig) {
			conf.uiController.ConnectToCanvas (controllableCanvas);
		}
	}





	/*
	public void InitializePlayersCharacters(){

		Debug.Log ("HIIOHPUIO");

		players = new PlayableCharacter[playersNumber];

		//Lancer L'initialisation
		playersInit = new PlayersInitialisation (this);
		playersInit.Initialization ();
	}
*/

	


	//ALLER CHERCHER LES CHARCONTROLLER AILLEURS
	public void EnableAllCharacters(){

		/*
		for (int i = 0; i < players.Length; i++) {
			CharController tempController = players [i].GetComponent<CharController> ();
			tempController.enabled = true;
		}
		*/
	}

	public void DisableAllCharacters(){

		/*
		for (int i = 0; i < players.Length; i++) {
			CharController tempController = players [i].GetComponent<CharController> ();
			tempController.enabled = false;
		}
		*/ 
	}
}