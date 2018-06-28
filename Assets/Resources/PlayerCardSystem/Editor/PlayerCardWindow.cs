using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerCardWindow : EditorWindow {

	static PlayerCard playerCard; 

	[MenuItem("Window/Max - PlayerCards")]
	static void OpenWindow(){
		PlayerCardWindow window = (PlayerCardWindow)GetWindow (typeof(PlayerCardWindow));
		window.minSize = new Vector2 (400, 400);
		window.Show ();
	}

	void OnEnable(){
		InitData ();
	}

	void InitData(){
		playerCard = (PlayerCard)ScriptableObject.CreateInstance (typeof(PlayerCard));
	}

	void OnGUI(){
		if (GUILayout.Button ("Create",GUILayout.Height(30)))
		{
			CreatePlayerData ();		
		}
	}


	void CreatePlayerData(){

		for (int i = 0; i < 4; i ++)
		{
			string dataPath = UsefulPath.playerCardData + "PlayerCard_" + (i+1) + ".asset";
			playerCard.playerNumber = i + 1;
			AssetDatabase.CreateAsset (PlayerCardWindow.playerCard, dataPath);
			InitData ();
		}
	}
}
