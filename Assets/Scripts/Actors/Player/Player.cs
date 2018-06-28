using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public PlayerMovement movement;
	public int playerNumber;
	public Color playerColor;

	public bool isAlive;

	void Awake(){
		movement = GetComponent<PlayerMovement> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
