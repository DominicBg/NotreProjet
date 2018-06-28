using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayersInitialisation {

	private readonly PlayersManager playersManager;
	private SetupScene setupScene;
	public PlayersInitialisation (SetupScene setupSceneFrom){
		playersManager = PlayersManager.playersManager;
		setupScene = setupSceneFrom;
	}

	public void Initialization(){	
		//Modele du Character
		Character playerBase;
		playerBase = (Character)Resources.Load ("Prefabs/IsoCharacter", typeof(Character));

		SpawnPoint[] spawnList = new SpawnPoint[0];
		spawnList = Object.FindObjectsOfType (typeof(SpawnPoint)) as SpawnPoint[];	



		//Creer les 4 personnages d'origine
		Debug.Log(playersManager.playersNumber);
		Character[] characterToCreate = new Character[playersManager.playersNumber];

		for (int i = 0; i < characterToCreate.Length; i++) {

			//Instantiation
			characterToCreate[i] = GameObject.Instantiate(playerBase, Vector3.zero, Quaternion.Euler(Vector3.zero));

			//Mettre le controleur
			IsoCharacterController zzz = characterToCreate [i].gameObject.AddComponent(typeof(IsoCharacterController)) as IsoCharacterController;
			zzz.device = playersManager.playersConfig [i].controller;

			//Placer les personnages sur le SpawnPoint
			characterToCreate[i].transform.position = spawnList [i].gameObject.transform.position;

			SetInformationFromPlayerCard (characterToCreate [i], i);

		}

		/*
		playersManager.players = new Character[playersManager.playersNumber];
		for (int i = 0; i < playersManager.playersNumber; i++) {			
			playersManager.players[i] = GameObject.Instantiate(playerBase, Vector3.zero, Quaternion.Euler(Vector3.zero));
			CharController zzz = playersManager.players [i].GetComponent<CharController> ();
			zzz.device = playersManager.playersConfig [i].controller;
		}	
		*/

		//Desactiver les personnages pour pas qu'ils bougent
		//A ENLEVER
		//playersManager.DisableAllCharacters ();

		//Fin de l'initialisation
		setupScene.PlayersInitializationFinished ();
	}

	void SetInformationFromPlayerCard(Character character, int i){
		
		string playerCardPath = UsefulPath.playerCardData + "PlayerCard_" + (i+1) + ".asset";
		PlayerCard card = (PlayerCard)AssetDatabase.LoadAssetAtPath (playerCardPath, typeof(PlayerCard));

		SpriteRenderer renderer = character.GetComponentInChildren<SpriteRenderer> ();
		card.playerColor.a = 1f;
		renderer.color = card.playerColor;

		PlayableCharacter playablePart = character.GetComponent<PlayableCharacter> ();
		playablePart.playerNumber = i;
		playablePart.playerColor = card.playerColor;


	}


}
