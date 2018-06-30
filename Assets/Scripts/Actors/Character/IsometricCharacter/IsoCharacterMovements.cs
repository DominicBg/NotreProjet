using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
//[RequireComponent(typeof(Animator))]
public class IsoCharacterMovements : Character {

	//Variables de deplacement ect
	//[SerializeField] float walkingSpeed = 0.5f;
	[SerializeField] float runningSpeedAct;
    public float runningSpeedMax;
	[SerializeField] float runningSpeedAcceleration = 1f;
	[SerializeField] float velocityMagnetudeMax = 7f;
	[SerializeField] float runningBrake = 1f;


	public float jumpPower = 360;
	[Range(1f, 100f)][SerializeField] float gravityMultiplier = 2f;
	[SerializeField] float groundCheckDistance = 100f;

	//Animator animator;
	public CapsuleCollider capsule;

	float origGroundCheckDistance;
	public bool isGrounded;
	//Vector3 groundNormal;

	public float wallCheckDistance;
	public bool isOnWall;
	Vector3 wallNormal;
	Vector3 wallDecalage;

	//public float directionAngle;
	//public float characterDirection;

	Animator animator;

	Vector3 lastMoveDirection;
	//A METTRE DANS UNE AUTRE CLASSE PLUS TARD
	public int playerNumber;

	//Action en cours du perso
	public enum CharacterState
	{
		Idle,
		Running,
		Jumping,
		Falling,
	}
	public CharacterState charState;


	public int directionAnim;

	// Use this for initialization
	public override void Start () {

        base.Start();

		//Prendre lanimator dans le sprite enfant GameObject
		//animator = GetComponentInChildren<Animator>();
		rb = this.GetComponent<Rigidbody>();
		capsule = this.GetComponent<CapsuleCollider>();

		rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

		origGroundCheckDistance = groundCheckDistance;

		Vector3 lastMoveDirection = Vector3.zero;

		animator = GetComponentInChildren<Animator> ();
		charState = CharacterState.Idle;

	}

	public void FixedUpdate(){
		SetCharacterDirectionForAnimation ();
		UpdateAnimator ();
	}

	//Methode appelee au Fixed Update par IsometricCharacterController
	public void Move(Vector2 move, bool jump){

		if (isGrounded) {	
			HandleGroundedMovement (jump);
			if (move.magnitude > 0f) {			
				CharacterRun ();
			} else {
				CharacterBrakeRunning ();
			}
			/*
			//Change state for Animator
			if (move.magnitude <= 0.1f) {
				charState = CharacterState.Idle;
			} else {
				charState = CharacterState.Running;
			}
			*/
		} else {
			HandleAirborneMovement ();
		}

		ActualizeDirection (move);
		CheckForWalls();
		CheckGroundStatus ();
		LimitPhysicsSpeed ();

	}

	void ActualizeDirection (Vector2 move){
		if (move.magnitude > 0f){
			//move.Normalize ();
			//lastMoveDirection = move;
			lastMoveDirection.x = move.x;
			lastMoveDirection.z = move.y;
		}
	}

	void CharacterRun (){
		if (runningSpeedAct < runningSpeedMax)				//Augmenter la vitesse de deplacement					
			runningSpeedAct += runningSpeedAcceleration;
		if (rb.velocity.magnitude < velocityMagnetudeMax) 	//Si on est pas a la vitesse maximum					
			rb.AddForce ((lastMoveDirection + wallDecalage) * runningSpeedAct * Time.deltaTime, ForceMode.Force);	
		charState = CharacterState.Running;
	}

	void CharacterBrakeRunning(){
		if (rb.velocity.magnitude > 0.1f) {	//Ralentir le depacement jusqu'au freinage					
			rb.AddForce (- rb.velocity * runningBrake * Time.deltaTime, ForceMode.VelocityChange);
		} else { //Bloquer le personnage					
			runningSpeedAct = 0f;
			charState = CharacterState.Idle;
		}
	}

	void HandleGroundedMovement(bool jump){
		if (jump) {
			rb.velocity = new Vector3 (rb.velocity.x, jumpPower, rb.velocity.z);
			isGrounded = false;
			groundCheckDistance = 0.1f;
			//Debug.Log ("JUMP!");
			charState = CharacterState.Jumping;
		}
	}

	void HandleAirborneMovement(){
		Vector3 extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
		rb.AddForce (extraGravityForce);

		groundCheckDistance = rb.velocity.y < 0 ? origGroundCheckDistance : 0.01f;

		//Changer letat quand le perso redescend
		if (rb.velocity.y < 0) {
			charState = CharacterState.Falling;
		}
	}

	void LimitPhysicsSpeed(){

	}

	/*
	void UpdateAnimator(){	

		//Envoyer la direction du perso
		animator.SetInteger("Direction", characterDirection);

		//Declancher l'anim en fonction de l'action
		switch (charState){
		case CharacterState.Idle:
			animator.SetTrigger ("Idle");
			break;
		case CharacterState.Running:
			animator.SetTrigger ("Run");	
			break;
		case CharacterState.Jumping:
			animator.SetTrigger ("Jump");
			break;
		case CharacterState.Falling:
			animator.SetTrigger ("Fall");
			break;
		}
	}
	*/

	void CheckForWalls(){

		RaycastHit hitInfo;

		#if UNITY_EDITOR
		Debug.DrawRay(transform.position + (Vector3.up * 0.25f), (lastMoveDirection * wallCheckDistance) + (Vector3.up * 0.25f), Color.black);
		#endif	

		if (Physics.Raycast (transform.position, lastMoveDirection, out hitInfo, wallCheckDistance)) {
			wallNormal = hitInfo.normal;
			#if UNITY_EDITOR
			Debug.DrawRay (hitInfo.transform.position, wallNormal * 10f, Color.red);
			#endif	
			isOnWall = true;
			wallDecalage = lastMoveDirection + wallNormal;

		} else {
			isOnWall = false;
			wallDecalage = Vector3.zero;
		}

		#if UNITY_EDITOR
		Debug.DrawRay (transform.position, wallDecalage * 10f, Color.red);
		#endif	
	}

	void CheckGroundStatus(){

		RaycastHit hitInfo;
		if (Physics.Raycast (transform.position, Vector3.down, out hitInfo, groundCheckDistance)) {
			//groundNormal = hitInfo.normal;
			isGrounded = true;
			//animator.applyRootMotion = true;
		} else {
			isGrounded = false;
			//groundNormal = Vector3.up;
			//animator.applyRootMotion = false;
		}

		#if UNITY_EDITOR
		Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.down * groundCheckDistance));
		#endif	
	}

	void SetAnimationDirection(){
		
	}

	void UpdateAnimator (){

		//A SUPPRIMER
		float z = Mathf.RoundToInt(Mathf.Atan2(lastMoveDirection.x, lastMoveDirection.y) * Mathf.Rad2Deg);

		animator.SetInteger ("Direction", directionAnim);

		switch (charState) {
		case CharacterState.Idle:
			animator.SetTrigger ("Idle");
			break;
		case CharacterState.Running:
			animator.SetTrigger ("Run");
			break;
		case CharacterState.Jumping:
			animator.SetTrigger ("Jump");
			break;
		case CharacterState.Falling:
			//animator.SetTrigger ("Falling");
			break;
		
		}
	}

	void SetCharacterDirectionForAnimation(){
		
		int angle = Mathf.RoundToInt(Vector3.SignedAngle(lastMoveDirection, Vector3.forward, Vector3.up));

		if (angle > -45 && angle < 45) {
			directionAnim = 0;
		} else if (angle < -45 && angle > -135) {
			directionAnim = 1;
		} else if (angle < -135 || angle > 135) {
			directionAnim = 2;
		}else if (angle > 45 && angle < 135) {
			directionAnim = 3;
		}
	}

}
