using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIState : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Methodes pareilles pour tout les etats du UI
	public void SetCanvasActive(){
		gameObject.SetActive (true);
	}

	public void SetCanvasInactive(){
		gameObject.SetActive (false);
	}
}
