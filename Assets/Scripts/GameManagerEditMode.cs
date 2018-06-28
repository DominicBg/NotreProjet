using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;

[ExecuteInEditMode]
public class GameManagerEditMode : MonoBehaviour{

	private GameManager manager; 

	//Supprimer

	/*
	//Script appele au lancement meme si inactif
	void Awake (){


		this.transform.position = new Vector3(0f,0f,0f);

		Scene scene = SceneManager.GetActiveScene ();


		SetMinimumPlayers ();

		if (Application.isPlaying) {
			Debug.Log ("IN PLAY AWAKE");
		}

		if (Application.isEditor && !Application.isPlaying) {

			if (!enabled) {
				enabled = true;
			}
			Debug.Log ("IN Editor AWAKE");
		}

	}

	void Start(){


		if (Application.isPlaying) {
			Debug.Log ("IN PLAY");
		}

		if (Application.isEditor && !Application.isPlaying) {
			Debug.Log ("IN Editor");
		}

		manager = GameManager.gameManager;


		//Debug.Log (manager);

		CheckSceneType ();
	}

	void CheckSceneType(){
		//Check ou se trouve la scene dans les files
		Scene scene = SceneManager.GetActiveScene ();
		string levelsPath = UsefulPath.levelScenes + scene.name + ".unity";
		string menusPath = UsefulPath.menusScenes + scene.name + ".unity";

		//Verifier le type de scene depuis lendroit ou elle est rangee
		if (File.Exists (levelsPath)) {
			string levelDataPath = UsefulPath.levelData + scene.name + ".asset";
			//manager.levelData = (LevelData)AssetDatabase.LoadAssetAtPath (levelDataPath, typeof(LevelData));
		} else {
			if (File.Exists(menusPath)){
				Debug.Log (scene + " Is a Menu");
			} else {
				Debug.Log (scene + " Is Unknown");
			}		
		}
	}

	void SetMinimumPlayers (){
		
	}



	*/

}
