using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{

	//Pour les controles
	[HideInInspector]public string hautBas; //Deplacements
	[HideInInspector]public string gaucheDroite;
	[HideInInspector]public string droitHautBas; //Deplacements
	[HideInInspector]public string droitGaucheDroite;
	[HideInInspector]public string boutonA; //Saut

	// Stick Gauche pour deplacement
	[HideInInspector]public float stickGX;
	[HideInInspector]public float stickGY; 

	private float stickDX;
	private float stickDY;

	private Vector3 dirStickG;
	private Vector3 dirStickD;

	//private Vector3 movement;
	public float speed;
	public float airSpeed;

	private float graviteAct;
	public float gravite;

	//Pour le deplacement
	private Rigidbody rb;
	private Quaternion rotation;
	private Vector3 direction;

	private Vector3 dirMoving;
//	private Vector3 dirAiming;

	private Quaternion rotAiming;

	//Pour le saut
	private bool onGround;

	private bool onMovingGround;
	public bool canJump;
	private bool isJumping;
	private float horJumpForce;
	private float verJumpForce;
	private float rofSaut;

	public float jumpSpeed;
	private float jumpSpeedAct;
	public float jumpSpeedAtt;
	public float maxJumpSpeed;

	//Pour le dash
	private bool canDash;
	public float dashForce;
	private bool isDashing;
	public float dashTime;

	//Propulsion dexplosion
	public bool isExploding;

	//Pour les platesFormes Mouvante
	public GameObject plateFormeAct;



	void Awake (){
		
		//Prendre reference du RigidBody
		rb = GetComponent<Rigidbody>();

	}

	// Use this for initialization
	void Start () {
		canJump = true;
	}


	// Update is called once per frame
	void Update () {

		//Enregistrer les valeurs des inputs
		//Stick Gauche
		stickGX = Input.GetAxis (gaucheDroite);
		stickGY = Input.GetAxis (hautBas);

		dirStickG.Set (stickGX, 0f, stickGY);

		//Stick Droit
		stickDX = Input.GetAxis (droitGaucheDroite);
		stickDY = Input.GetAxis (droitHautBas);

		dirStickD.Set (stickDX, 0f, stickDY);

		Jump (); 
		Dashing ();

	} //FIN UPDATE

	void FixedUpdate(){

		// Adjust the rigidbodies position and orientation in FixedUpdate.
		Move ();
		Turn ();
		// Orientation du perso avec StickGauche
		Orientation ();

		GroundCheck ();
		if (onMovingGround)
			GroundMouvement ();
	}

	private void Move(){

		dirMoving = Quaternion.AngleAxis (45f, Vector3.up) * dirStickG;



		if (onGround) {
			if (dirMoving != Vector3.zero) 
				transform.Translate (dirMoving * speed * Time.deltaTime, Space.World);
			rb.velocity.Set (0f, 0f, 0f);
		}

		//Appliquer la gravité
		if (!onGround){
			if (dirMoving != Vector3.zero) 
				transform.Translate (dirMoving * airSpeed * Time.deltaTime);	
			if (!isJumping && !isDashing && !isExploding) {
				transform.Translate (Vector3.down * graviteAct * Time.deltaTime);
				graviteAct += gravite;
			}
		}
	}

	private void Turn(){


	}

	private void Orientation(){



		/*
		if (dirStickD == Vector3.zero) {
			rotAiming = Quaternion.LookRotation (dirMoving, Vector3.up);
		}

		if (dirStickD != Vector3.zero) {
			rotAiming = Quaternion.LookRotation (dirAiming, Vector3.up);
		}
		*/

	}

	private void Jump(){

		if (Input.GetButtonDown (boutonA)) {
			if (onGround) {
				jumpSpeedAct = jumpSpeed;
				transform.Translate (Vector3.up * jumpSpeedAct * Time.deltaTime);
				isJumping = true;
			}

			if (!onGround && canDash) {
				Dash ();
			}
		}

		if (Input.GetButton (boutonA) && isJumping) {		
			if ( jumpSpeedAct > 0 ){
				transform.Translate (Vector3.up * jumpSpeedAct * Time.deltaTime);
				jumpSpeedAct -= jumpSpeedAtt;
			}
			if (jumpSpeedAct <= 0) {
				isJumping = false;
				jumpSpeedAct = 0f;			
			}
		}

		if (!Input.GetButton (boutonA)) {
			isJumping = false;		
			jumpSpeedAct = 0f;
		}
	}

	void Sauter(){

		if ( jumpSpeedAct < maxJumpSpeed ){
			transform.Translate (Vector3.up * jumpSpeedAct * Time.deltaTime);
			jumpSpeedAct += jumpSpeed;
		}

		if (jumpSpeedAct >= maxJumpSpeed) {


		}


		canJump = false;
		canDash = true;
		onGround = false;
		//Invoke ("ResetJump",rofSaut);
	}

	void ResetJump(){
		canJump = true;
	}

	void Dash(){
		canDash = false;
		isDashing = true;
		Invoke ("StopDash", dashTime);

	}

	void Dashing(){
		if (isDashing) transform.Translate (dirMoving * dashForce * Time.deltaTime);
	}

	void StopDash(){
		isDashing = false;
	}

	void GroundCheck(){
		
		RaycastHit hitGround;

		bool ray = Physics.Raycast (this.transform.position, Vector2.down, out hitGround, 1.2f);

		//Changement sur sol sans saut


		if (ray) {

			//Changement sur sol sans saut

			if (plateFormeAct != hitGround.collider.gameObject) {
				plateFormeAct = hitGround.collider.gameObject;
				if (hitGround.collider.tag == "Elevator") {
					if (!onMovingGround) {
						onMovingGround = true;	
						transform.parent = hitGround.collider.transform;
					}
				} else {
					onMovingGround = false;	
				}		
			}



			//En arrivant au sol 

			if (!onGround) {
				//Changement des qu'on passe onGround
				onGround = true;
				graviteAct = 0f;
				onGround = true;
				canDash = true;
				canJump = true;
				plateFormeAct = hitGround.collider.gameObject;

				if (hitGround.collider.tag == "Elevator") {
					if (!onMovingGround) {
						onMovingGround = true;	
						transform.parent = hitGround.collider.transform;
					}
				} else {
					onMovingGround = false;	

				}
			}	 

		} else {
			transform.parent = null;

			onGround = false;
			onMovingGround = false;
			canJump = false;
			transform.parent = null;
			plateFormeAct = null;
		}
	}

	public void GroundMouvement(){

		if (plateFormeAct != null) {

		}
	}





}
