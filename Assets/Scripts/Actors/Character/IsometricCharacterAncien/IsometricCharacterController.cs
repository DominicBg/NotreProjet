using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SUPPRIMER CE CODE 
[RequireComponent(typeof(IsometricCharacter))]
public class IsometricCharacterController : CharController {

	//Character Variables
	private IsometricCharacter character;
	//private Vector3 move;
	//private float leftStickDistance;

	//private bool jump;

	//Weapon Variables
	private BombLauncher bombLauncher;
	private Vector3 rightStickDirection;
	private float rightStickDistance;
	private bool shoot;
	private bool place;


	//Triggers Variables



	//Camera Agnles;
	public Vector3 cameraForward = new Vector3(-0.5f, 0f, 0.5f);
	public Vector3 cameraRight;

	// Use this for initialization
	void Start () {
        Debug.Log("CODE A SUPPRIMER");

        //Character
        character = GetComponent<IsometricCharacter> ();

		//Weapon
		bombLauncher = GetComponentInChildren<BombLauncher> ();

		cameraRight = Quaternion.Euler (new Vector3 (0, 90, 0)) * cameraForward;
	}

	// Update is called once per frame
	void Update () {
        //Get buttons saut
        //		jump = Input.GetButton (boutonA);

        //Input for Weapon
        //shoot = Input.GetButton ("ButB0");
        //place = Input.GetButton ("ButX0");

        Debug.Log("CODE A SUPPRIMER");

	}

	private void FixedUpdate (){

		/*
		//Left Stick
		float h = Input.GetAxis (hautBas);
		float v = Input.GetAxis (gaucheDroite);
		//Right Stick
		float hdroite = Input.GetAxis ("RSHor0");
		float vdroite = Input.GetAxis ("RSVer0");
		//Triggers
		float leftTrigger = Input.GetAxis(triggerLT);
		float rightTrigger = Input.GetAxis (triggerRT);
		bool ltPressed = false;
		bool rtPressed = false;

		//Left Stick. VERTICAL set negative 
		move = -v * cameraForward + h * cameraRight; 
		Vector2 tempLeftStick = new Vector2 (-v, h);
		leftStickDistance = tempLeftStick.magnitude;
		if (leftStickDistance >= 1)
			leftStickDistance = 1;

		//OLD WAY without Camera Angle
		//move = v * Vector3.forward + h * Vector3.right; 

		//Right Stick Direction
		rightStickDirection = -vdroite * cameraForward + hdroite * cameraRight;

		//Right Stick MAGNETUDE
		Vector2 tempRightStick = new Vector2 (-vdroite, hdroite);
		rightStickDistance = tempRightStick.magnitude;
		if (rightStickDistance >= 1) 
			rightStickDistance = 1;

		//Convert LT and RT float into BOOL 
		if (leftTrigger > 0)
			ltPressed = true;
		if (rightTrigger > 0)
			rtPressed = true;


		character.Move (move, leftStickDistance, jump);
		bombLauncher.AimAndUse (move, leftStickDistance, rightStickDirection, rightStickDistance, ltPressed, rtPressed);
		*/
		//character.Move (move, leftStickDistance, jump);

	}



}


