using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class EndMenuCanvas : MonoBehaviour {

	public Button restartLevel;
	public Button nextLevel;
	public Button returnToMainMenu;

	public GMEndedGame endMenuManager;

	void Start () {
		Debug.Log ("endMenu");
		returnToMainMenu.onClick.AddListener (ReturnToMainMenu);
	}

	// Update is called once per frame
	void Update () {

	}

	void ReturnToMainMenu(){
		Debug.Log ("RETURN FONCTION");
		endMenuManager.ReturnToMainMenu ();
	}

}
