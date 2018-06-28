using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterColorsWindow : EditorWindow {

	static CharacterColors colorList;

	[MenuItem("Window/Max - CharacterColorsList")]
	static void OpenWindow(){
		CharacterColorsWindow window = (CharacterColorsWindow)GetWindow (typeof(CharacterColorsWindow));
		window.minSize = new Vector2 (400, 400);
		window.Show ();
	}

	void OnEnable(){
		InitData ();
	}

	void InitData(){
		colorList = (CharacterColors)ScriptableObject.CreateInstance (typeof(CharacterColors));
	}

	void OnGUI(){
		if (GUILayout.Button ("Create",GUILayout.Height(30)))
		{
			CreateColorList ();		
		}
	}

	void CreateColorList(){
		string dataPath = UsefulPath.characterColorData + "CharacterColorList" + ".asset";
		AssetDatabase.CreateAsset (CharacterColorsWindow.colorList, dataPath);
		InitData ();
	}
}
