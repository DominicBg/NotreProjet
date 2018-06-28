using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader {

	private string sceneName;
	public SceneLoader(string nameScene){
		sceneName = nameScene;
	}

	public void LoadScene(){
		SceneManager.LoadScene (sceneName, LoadSceneMode.Single);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode){
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}
