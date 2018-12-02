using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using InControl;

[CustomEditor(typeof(PlayersManager))]
public class PlayersManagerEditor : Editor {

	private PlayersManager pManager;
	private GameManager manager;
	private GameManagerEditor managerEditor;
	private enum CustomEditorState{PlayersSettings,Other};
	private CustomEditorState state;
	private int playersNumber;

	public InputDevice[] devices;
	public string[] devicesList;

	void OnEnable(){
		pManager = (PlayersManager)target;

		manager = (GameManager)FindObjectOfType (typeof(GameManager)) as GameManager;

		CheckManagerState ();

		SelectController ();
	}

	void CheckManagerState(){
		//GameManagerEditor[] editors = (GameManagerEditor[])Resources.FindObjectsOfTypeAll (typeof(GameManagerEditor));
	//	Debug.Log (editors[0]);

		//zzz = FindObjectOfType<GameManagerEditor>();

		state = CustomEditorState.PlayersSettings;
	}

	public override void OnInspectorGUI(){
		switch (state) {
		case CustomEditorState.PlayersSettings:
			if (manager.nbrJoueursTemp > 0) {
			
			}

			ShowPlayersSettings ();
			break;
		}
	}

	void ShowPlayersSettings(){

		for (int i = 1; i <= manager.nbrJoueursTemp; i ++){

			string playerCardPath = UsefulPath.playerCardData + "PlayerCard_" + i + ".asset";
			PlayerCard card = (PlayerCard)AssetDatabase.LoadAssetAtPath (playerCardPath, typeof(PlayerCard));

			EditorGUILayout.LabelField ("Player Number : " + i);
			card.playerColor = EditorGUILayout.ColorField ("Player Color", card.playerColor);
			card.playerColor.a = 1f;
			EditorGUILayout.Space ();
		}

		EditorGUILayout.LabelField ("NICE :", "NOPE");
		//EditorGUILayout.LabelField (zzz.nbrJoueurs);
		//EditorGUILayout.IntField ("INT", blabla);

	}

	void SelectController(){
		Debug.Log ("Empty methods Select Controller from PlayersManagerEditor");

		/*
		devicesList = new string[InputManager.Devices.Count];
		for (int i = 0; i < devicesList.Length; i++) {
			Debug.Log (InputManager.Devices[i].Name);
		}
		*/

	}


}
