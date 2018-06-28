using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class GameSystemWindow : EditorWindow {

	//PRESENTATION DE LA FENETRE
	Texture2D headerSectionTexture;
	Texture2D levelDataSectionTexture;
	Texture2D gameModeDataSectionTexture;
	Texture2D levelListDataSectionTexture;
	Texture2D gameModeListDataSectionTexture;

	Color headerSectionColor = new Color (50f / 255f, 50f / 255f, 50f / 255f, 1f);
	Color ldSectionColor = new Color (150f / 255f, 150f / 255f, 150f / 255f, 1f);
	Color gmdSectionColor = new Color (100f / 255f, 100f / 255f, 100f / 255f, 1f);
	Color llistSectionColor = new Color (100f / 255f, 100f / 255f, 100f / 255f, 1f);
	Color gmlistSectionColor = new Color (150f / 255f, 150f / 255f, 150f / 255f, 1f);

	Rect headerSection;
	Rect levelDataSection;
	Rect gameModeDataSection;
	Rect levelListDataSection;
	Rect gameModeListDataSection;

	//LEVEL DATA SYSTEM 
	static LevelData levelData;
	public static LevelData levelInfo { get { return levelData;} }
	public static GameModeListData gameModeList;
	public string[] gameModeStringList;
	public int index = 0;
	public int levelMinPlayer;
	public int levelMaxPlayer;

	//LEVEL LIST DATA SYSTEM
	static LevelListData levelListData;
	public static LevelListData levelListInfo { get { return levelListData;} }
	public bool listAlreadyExists;

	//GAME MODE DATA SYSTEM
	static GameModeData gameModeData;
	public GameMode gameMode;
	public static GameModeData gameModeInfo { get { return gameModeData; } }
	public int gameModeMinPlayer = 1;
	public int gameModeMaxPlayer = 4;

	//GAME MODE LIST SYSTEM
	static GameModeListData gameModeListData;
	public static GameModeListData gameModeListInfo { get { return gameModeListData; } }
	public string[] gameModeStrings;
	public bool listGameModeAlreadyExists;

	[MenuItem("Window/Max - Game System")]
	static void OpendWindow(){
		GameSystemWindow window = (GameSystemWindow)GetWindow (typeof(GameSystemWindow));
		window.minSize = new Vector2 (800, 400);
		window.Show ();
	}

	void OnEnable(){
		InitTextures ();
		InitData ();
	}

	void InitTextures(){
		headerSectionTexture = new Texture2D (1, 1);
		headerSectionTexture.SetPixel (0, 0, headerSectionColor);
		headerSectionTexture.Apply ();

		levelDataSectionTexture = new Texture2D (1, 1);
		levelDataSectionTexture.SetPixel (0, 0, ldSectionColor);
		levelDataSectionTexture.Apply ();

		gameModeDataSectionTexture = new Texture2D (1, 1);
		gameModeDataSectionTexture.SetPixel (0, 0, gmdSectionColor);
		gameModeDataSectionTexture.Apply ();

		levelListDataSectionTexture = new Texture2D (1, 1);
		levelListDataSectionTexture.SetPixel (0, 0, llistSectionColor);
		levelListDataSectionTexture.Apply ();

		gameModeListDataSectionTexture = new Texture2D (1, 1);
		gameModeListDataSectionTexture.SetPixel (0, 0, gmlistSectionColor);
		gameModeListDataSectionTexture.Apply ();
	}

	//Initialiser les Datas
	void InitData(){
		InitLevelData();
		InitGameModeData ();
		InitLevelListData ();
		InitGameModeListData ();
	}

	void OnGUI(){
		DrawLayouts ();
		DrawHeader ();
		DrawLevelDataSettings ();
		DrawGameDataSettings ();
		DrawLevelListSettings ();
		DrawGameModeListSettings ();
	}

	void DrawLayouts(){
		headerSection.x = 0;
		headerSection.y = 0;
		headerSection.width = Screen.width;
		headerSection.height = 20;
		GUI.DrawTexture (headerSection, headerSectionTexture);

		levelDataSection.x = 0;
		levelDataSection.y = headerSection.height;
		levelDataSection.width = Screen.width/2f;
		levelDataSection.height = (Screen.height - headerSection.height) / 2;
		GUI.DrawTexture (levelDataSection, levelDataSectionTexture);

		gameModeDataSection.x = Screen.width/2f;
		gameModeDataSection.y = headerSection.height;
		gameModeDataSection.width = Screen.width/2f;
		gameModeDataSection.height = Screen.height - headerSection.height;
		GUI.DrawTexture (gameModeDataSection, gameModeDataSectionTexture);

		levelListDataSection.x = 0;
		levelListDataSection.y = (Screen.height - headerSection.height) / 2;
		levelListDataSection.width = Screen.width/2f;
		levelListDataSection.height = (Screen.height - headerSection.height) / 2;
		GUI.DrawTexture (levelListDataSection, levelListDataSectionTexture);

		gameModeListDataSection.x = Screen.width/2f;
		gameModeListDataSection.y = (Screen.height - headerSection.height) / 2;
		gameModeListDataSection.width = Screen.width/2f;
		gameModeListDataSection.height = (Screen.height - headerSection.height) / 2;
		GUI.DrawTexture (gameModeListDataSection, gameModeListDataSectionTexture);
	}

	void DrawHeader(){
		GUILayout.BeginArea (headerSection);
		GUILayout.Label ("Game System");
		GUILayout.EndArea ();
	}




	void InitLevelData(){		
		levelData = (LevelData)ScriptableObject.CreateInstance (typeof(LevelData));	
		gameModeList = (GameModeListData)AssetDatabase.LoadAssetAtPath (UsefulPath.listGameModeData, typeof(GameModeListData));
	}

	void DrawLevelDataSettings(){
		GUILayout.BeginArea (levelDataSection);
		GUILayout.Label ("Create New Level Data");

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("Map Name");
		levelData.mapName = EditorGUILayout.TextField (levelData.mapName);
		EditorGUILayout.EndHorizontal ();

		GUILayout.Label ("Game Mode");
		EditorGUILayout.BeginHorizontal ();
		index = EditorGUILayout.Popup(index, gameModeList.gameModeStringList);
		levelData.gameModeName = gameModeList.gameModeStringList [index];
		levelMinPlayer = gameModeList.gameModeDatas [index].gameModeMinPlayer;
		levelMaxPlayer = gameModeList.gameModeDatas [index].gameModeMaxPlayer;
		levelData.gameMode = gameModeList.gameModeDatas [index];
		EditorGUILayout.EndHorizontal ();

		GUILayout.Label ("Number of Player");
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("Min Players");
		levelData.levelMinPlayers = EditorGUILayout.IntSlider (levelData.levelMinPlayers, levelMinPlayer, levelMaxPlayer);
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("Max Players");
		levelData.levelMaxPlayers = EditorGUILayout.IntSlider (levelData.levelMaxPlayers, levelMinPlayer, levelMaxPlayer);
		EditorGUILayout.EndHorizontal ();

		if (GUILayout.Button ("Create",GUILayout.Height(30)))
		{
			SaveNewLevelData ();		
		}
		GUILayout.EndArea ();
	}

	void SaveNewLevelData(){
		//Creer l'asset
		string dataPath = UsefulPath.levelData + levelData.mapName + ".asset";
		AssetDatabase.CreateAsset (GameSystemWindow.levelData, dataPath);
		//Creer la scene
		levelData.scene = EditorSceneManager.NewScene (NewSceneSetup.EmptyScene, NewSceneMode.Additive);
		//Sauvegarder la scene
		EditorSceneManager.SaveScene (levelData.scene, UsefulPath.levelScenes + levelData.mapName + ".unity");
		Debug.Log ("LEVEL CREATED");
		//Reinitialiser
		InitLevelData ();
	}



	void InitGameModeData (){
		gameModeData = (GameModeData)ScriptableObject.CreateInstance (typeof(GameModeData));
	}

	void DrawGameDataSettings(){
		GUILayout.BeginArea (gameModeDataSection);
		GUILayout.Label ("Create New Game Mode Data");

		//Place pour entrer le nom du Game Mode
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("GameMode Name");
		gameModeData.gameModeName = EditorGUILayout.TextField (gameModeData.gameModeName);
		EditorGUILayout.EndHorizontal ();

		//Place pour entrer le script du GameMode
		EditorGUILayout.BeginHorizontal ();
		gameModeData.gameMode = (GameMode)EditorGUILayout.ObjectField ("Game Mode Script", gameModeData.gameMode, typeof (GameMode), false);
		//Le false oblige a inserer des assets uniquement. 
		//True permetterait de mettre des objets de scene
		EditorGUILayout.EndHorizontal ();

		GUILayout.Label ("Number of Player");
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("Min Players");
		gameModeData.gameModeMinPlayer = EditorGUILayout.IntSlider (gameModeData.gameModeMinPlayer, gameModeMinPlayer, gameModeMaxPlayer);
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("Max Players");
		gameModeData.gameModeMaxPlayer = EditorGUILayout.IntSlider (gameModeData.gameModeMaxPlayer, gameModeMinPlayer, gameModeMaxPlayer);
		EditorGUILayout.EndHorizontal ();

		if (GUILayout.Button ("Create",GUILayout.Height(30)))
		{
			SaveNewGameModeData ();		
		}

		GUILayout.EndArea ();
		
	}

	void SaveNewGameModeData(){
		string dataPath = UsefulPath.gameModeData + gameModeData.gameModeName + ".asset";
		AssetDatabase.CreateAsset (GameSystemWindow.gameModeData, dataPath);
		//Reinitialiser
		InitGameModeData();
	}




	//LEVEL LIST DATA
	void InitLevelListData(){
		LevelListData tempLevelList = (LevelListData)AssetDatabase.LoadAssetAtPath (UsefulPath.listLevelData, typeof(LevelListData));
		if (tempLevelList == null) {
			listAlreadyExists = false;
			levelListData = (LevelListData)ScriptableObject.CreateInstance (typeof(LevelListData));	
		} else {
			listAlreadyExists = true;
			levelListData = tempLevelList;
		}
	}

	void DrawLevelListSettings(){
		GUILayout.BeginArea (levelListDataSection);
		GUILayout.Label ("Create or Edit Level List");

		if (listAlreadyExists) {
			if (GUILayout.Button ("Refresh List",GUILayout.Height(70)))
				RefreshLevelList ();				
		} else {
			if (GUILayout.Button ("Create Level List",GUILayout.Height(70)))
				CreateLevelList ();					
		}

		GUILayout.EndArea ();
	}

	void CreateLevelList(){

		//Creer si la liste N'existe pas deja
		AssetDatabase.CreateAsset (GameSystemWindow.levelListData, UsefulPath.listLevelData);

		Debug.Log ("Asset Created");
		RefreshLevelList ();
		InitLevelListData ();
	}

	void RefreshLevelList(){
		//Rafraichir la list avec les nouveau Levels
		string[] foundGUIDAssets;

		foundGUIDAssets = AssetDatabase.FindAssets ("t:LevelData");

		levelListData.leveldatas = new LevelData[foundGUIDAssets.Length];

		foreach (string zzz in foundGUIDAssets) {
			Debug.Log("LevelData" + AssetDatabase.GUIDToAssetPath(zzz));
		}

		for (int i = 0; i < foundGUIDAssets.Length; i++) {
			LevelData zzz = (LevelData)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(foundGUIDAssets[i]), typeof(LevelData));
			levelListData.leveldatas[i] = zzz;
			Debug.Log("Saved" + levelListData.leveldatas[i]);	
		}

		LevelListData dataLevelList = (LevelListData)AssetDatabase.LoadAssetAtPath (UsefulPath.listLevelData, typeof(LevelListData));

		dataLevelList.leveldatas = levelListData.leveldatas;
		EditorUtility.SetDirty (dataLevelList);
	}




	void InitGameModeListData(){
		GameModeListData tempGameModeList = (GameModeListData)AssetDatabase.LoadAssetAtPath (UsefulPath.listGameModeData, typeof(GameModeListData));
		if (tempGameModeList == null) {
			listGameModeAlreadyExists = false;
			gameModeListData = (GameModeListData)ScriptableObject.CreateInstance (typeof(GameModeListData));			
		} else {
			listGameModeAlreadyExists = true;
			gameModeListData = tempGameModeList;
		}
	}


	void DrawGameModeListSettings(){
		GUILayout.BeginArea (gameModeListDataSection);
		GUILayout.Label ("Create Or Edit Game Mode List");
		if (listGameModeAlreadyExists) {		
			if (GUILayout.Button ("Refresh Game Mode List",GUILayout.Height(70)))
				RefreshGameModeList ();					
		} else {
			if (GUILayout.Button ("Create Game Mode List",GUILayout.Height(70)))
				CreateGameModeList ();					
		}
		GUILayout.EndArea ();
	}

	void CreateGameModeList(){
		AssetDatabase.CreateAsset (GameSystemWindow.gameModeListData, UsefulPath.listGameModeData);
		Debug.Log ("Asset Created");
	}

	void RefreshGameModeList(){
		string[] foundGUIDAssets;

		foundGUIDAssets = AssetDatabase.FindAssets ("t:GameModeData");

		gameModeListData.gameModeDatas = new GameModeData[foundGUIDAssets.Length];
		gameModeListData.gameModeStringList = new string[foundGUIDAssets.Length];

		foreach (string zzz in foundGUIDAssets) {
			Debug.Log("GameModeData" + AssetDatabase.GUIDToAssetPath(zzz));
		}

		for (int i = 0; i < foundGUIDAssets.Length; i++) {
			GameModeData zzz = (GameModeData)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(foundGUIDAssets[i]), typeof(GameModeData));
			gameModeListData.gameModeDatas[i] = zzz;
			Debug.Log("Saved" + gameModeListData.gameModeDatas[i]);
			gameModeListData.gameModeStringList [i] = zzz.ToString ();
		}

		//string dataPath = "Assets/Resources/GameModeListData/Data/";
		//dataPath += "GameModeList" + ".asset";

		GameModeListData dataGameModeList = (GameModeListData)AssetDatabase.LoadAssetAtPath (UsefulPath.listGameModeData, typeof(GameModeListData));

		dataGameModeList.gameModeDatas = gameModeListData.gameModeDatas;
		EditorUtility.SetDirty (dataGameModeList);
	}








}
