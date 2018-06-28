﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SetupScene : MonoBehaviour{
	
	public SetupScene(){
		gameManager = GameManager.gameManager;
	}

	public GameManager gameManager;
	private bool sceneOpened;

	private bool sceneIsLoaded = false;
	private bool charactersIsLoaded = false;
	private bool cameraIsLoaded = false;
	private bool gamemodeIsLoaded = false;

	public void StartLevelLoading(){
		StartCoroutine (SceneLoadingCoroutine ());	
	}
	public IEnumerator SceneLoadingCoroutine(){
		
		//Attendre que la scene soit chargee
		sceneIsLoaded = false;
		LoadingSceneFrom ();
		yield return new WaitUntil (() => sceneIsLoaded);

		//Attendre que les personnages soient charges
		charactersIsLoaded = false;
		InitializePlayers ();
		yield return new WaitUntil (() => charactersIsLoaded);
		//Debug.Log(this + " : Players initialized");

		cameraIsLoaded = false;
		InitializeCamera ();
		yield return new WaitUntil (() => cameraIsLoaded);
		//Debug.Log(this + " : Camera Initialized");

		gamemodeIsLoaded = false;
		InitializeGameMode ();
		yield return new WaitUntil (() => gamemodeIsLoaded);
		//Debug.Log(this + " : GameMode initialized");

		InitializeTriggerZones ();
		//Debug.Log(this + " : Triggers Initialized");

		SetupSceneFinished ();
		//Debug.Log(this + " : Scene Loading Finished");

		yield return new WaitForSeconds (1);

		//Debug.Log ("Coroutine Scene Loading Ended");

	}

	void LoadingSceneFrom(){
		//Initialisation du niveau depuis Main Menu ou depuis l'editeur
		if (gameManager.launchingFrom == ManagerEnums.LaunchingFrom.ThisLevel)
		{
			//Si la map est deja ouverte dans l'editeur
			sceneIsLoaded = true;
			gameManager.launchingFrom = ManagerEnums.LaunchingFrom.StartMenu;
		} else {
			SceneManager.LoadScene (gameManager.levelData.mapName, LoadSceneMode.Single);
			SceneManager.sceneLoaded += OnSceneLoaded;
		}
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode){	
		//Delegate Disabled
		SceneManager.sceneLoaded -= OnSceneLoaded;
		sceneIsLoaded = true;
	}





	public void InitializePlayers(){
		PlayersInitialisation playersInit = new PlayersInitialisation (this);
		playersInit.Initialization ();
	}

	//Appelé depuis PlayersManager
	public void PlayersInitializationFinished(){
		charactersIsLoaded = true;
	}


	public void InitializeCamera(){

		/*
		CameraFollowing cam = (CameraFollowing)GameObject.FindObjectOfType (typeof(CameraFollowing));
		cam.m_Targets = new Transform[gameManager.playersManager.players.Length];

		for (int i = 0; i < gameManager.playersManager.players.Length; i++) {
			cam.m_Targets [i] = gameManager.playersManager.players [i].transform;
		}
		cam.enabled = true;
		*/

		cameraIsLoaded = true;
	}


	void InitializeGameMode(){

		string gameModeName = gameManager.levelData.gameMode.name;
		string gameModeDataPath = UsefulPath.gameModeData + gameModeName + ".asset";

		//Debug.Log (gameModeDataPath);

		GameModeData tempData = (GameModeData)AssetDatabase.LoadAssetAtPath (gameModeDataPath, typeof(GameModeData));

		gameManager.currentGameMode = tempData.gameMode;

		gameManager.currentGameMode.Initialize ();

		gamemodeIsLoaded = true;
	}

	void InitializeTriggerZones(){
		TriggerZone[] triggers = Object.FindObjectsOfType<TriggerZone>();
		foreach(TriggerZone trig in triggers){
			trig.Initialize();
		}
	}

	void SetupSceneFinished(){
		gameManager.PlayLevel ();
	}





}
