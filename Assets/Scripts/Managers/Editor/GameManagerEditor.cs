using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using ManagerEnums;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor 
{	 

	private GameManager manager; 
	private Scene scene;
	private bool levelDataExists;
	public int nbrJoueurs; 

	public int zzz;

	//S'active chaque fois quon clique sur lobjet - voir Awake
	void OnEnable(){		
		manager = (GameManager)target;
		CheckIfLevelDataExists ();
	}

	void CheckIfLevelDataExists(){

		levelDataExists = false;

		Scene scene = SceneManager.GetActiveScene ();
		string levelsPath = UsefulPath.levelScenes + scene.name + ".unity";
        Debug.Log(levelsPath);
        

		if (File.Exists (levelsPath)) {
			levelDataExists = true;
            Debug.Log("Level Data exists");
            if (manager.levelData == null)
            {
                //Load level data into Manager
                LoadLevelDataOnManager(levelsPath);

            } else if (manager.levelData != null){
                Debug.Log(this.name + " Leveldata Loade");                
            }
		}
        else if (File.Exists(levelsPath))
        {
            Debug.Log("Level Path not found. Check Build Settings - Scenes in build");
        }

	}

    //If LevelData is not Loaded into Game Manager
    void LoadLevelDataOnManager(string levelPath)
    {
        Debug.Log("Ouverture du LoadLevelDataManager");

        Scene scene = SceneManager.GetActiveScene();
        string levelDataPath = UsefulPath.levelData + scene.name + ".asset";
        Debug.Log(levelDataPath + " Path");

        LevelData zzz = (LevelData)AssetDatabase.LoadAssetAtPath(levelDataPath, typeof(LevelData));

        if (zzz != null)
        {
            Debug.Log(zzz);
        } else
        {
            Debug.Log("NON TORUV");
        }

        Debug.Log("LevelData : " + zzz.name);
        manager.levelData = zzz;
    }

    public override void OnInspectorGUI(){

		if (!Application.isPlaying) {
			manager.launchingFrom = (LaunchingFrom)EditorGUILayout.EnumPopup ("Play Mode : ", manager.launchingFrom);

			//Changer le manager en fonction des selections
			switch (manager.launchingFrom) {
			case LaunchingFrom.ThisLevel:
				if (levelDataExists) {				
					OnLevelScene ();
				} else {
					LevelDataNotFound ();
				}
				break;
			case LaunchingFrom.StartMenu:
				OnMenuScene ();
				break;
			}
		}




		if (EditorApplication.isPlaying) {
			EditorGUILayout.LabelField ("Actual Mode", manager.actualMode.ToString());
		}

		//IMPORTANT
		//CONSERVER LES MODIFICATIONS
		EditorUtility.SetDirty (manager);

	}

	void OnLevelScene(){
		
		EditorGUILayout.LabelField ("--==Play From This Level ==--");
		manager.nbrJoueursTemp = EditorGUILayout.IntField ("Testing Players", manager.nbrJoueursTemp);

		if (manager.levelData != null) {
			EditorGUILayout.LabelField ("Map", manager.levelData.mapName);
			EditorGUILayout.LabelField ("Game Mode", manager.levelData.gameModeName);
			if (manager.levelData.gameMode.gameMode == null) {
				EditorGUILayout.LabelField ("Game Mode Script", "Empty");
			} else {
				EditorGUILayout.LabelField ("Game Mode Script", "Ok");
			}
		}

		if (manager.levelData == null) {
			EditorGUILayout.LabelField ("LEVELDATA", "No Level Data");
		}

	}

	void OnMenuScene(){
		EditorGUILayout.LabelField ("zZz Launching From Start Menu zZz");
	}

	void LevelDataNotFound(){
		EditorGUILayout.LabelField ("XXX Level Data not found XXX");
		EditorGUILayout.LabelField ("XXX Please Create One XXX");
	}

	void DisplayLevelAndModeName(){
		EditorGUILayout.LabelField ("Infos dans le manager");
	}


}




