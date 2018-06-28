using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacter : MonoBehaviour {

	public Character character;
	public int playerNumber;
	public Color playerColor;
	// Use this for initialization
	void Start () {
		character = GetComponent<Character> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
