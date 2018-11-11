using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
//[RequireComponent(typeof(Animator))]
public class IsometricCharacter : Character {

	//Variables de deplacement ect
	//[SerializeField] float walkingSpeed = 0.5f;
	[SerializeField] float runningSpeedAct;
	[SerializeField] float runningSpeedMax = 10f;
	[SerializeField] float runningSpeedAcceleration = 1f;
	[SerializeField] float velocityMagnetudeMax = 7f;
	[SerializeField] float runningBrake = 1f;


	[SerializeField] float jumpPower = 360;
	[Range(1f, 100f)][SerializeField] float gravityMultiplier = 2f;
	[SerializeField] float groundCheckDistance = 100f;

	//Animator animator;
	public CapsuleCollider capsule;

	float origGroundCheckDistance;
	public bool isGrounded;
	Vector3 groundNormal;

	public float wallCheckDistance;
	public bool isOnWall;
	Vector3 wallNormal;
	Vector3 wallDecalage;

	//Direction du perso: 0 = Up 90 = Right
	public int directionAngle;
	public int characterDirection;
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


	// Use this for initialization
	public override void Start () {

        base.Start();

		//Prendre lanimator dans le sprite enfant GameObject
		//animator = GetComponentInChildren<Animator>();

		capsule = this.GetComponent<CapsuleCollider>();

		rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

		origGroundCheckDistance = groundCheckDistance;

		charState = CharacterState.Idle;

		Vector3 lastMoveDirection = Vector3.zero;



	}

	// Update is called once per frame
	void Update () {

	}

	//Methode appelee au Fixed Update par IsometricCharacterController
	public void Move(Vector3 move, float moveDistance, bool jump){

		//Checker direction du vecteur Move
		//Transformer la direction du V3 move en un angle  
		directionAngle = Mathf.RoundToInt(Mathf.Atan2 (move.x, move.z) * Mathf.Rad2Deg);




		//INUTILE ? 
		//Magnetude de 1
		if (move.magnitude > 0f){
			move.Normalize ();
			//Sauvegarder la derniere valeur de Move
			lastMoveDirection = move;
		}
		//INUTILE ?
		//Mettre le vecteur a plat;
		move = Vector3.ProjectOnPlane(move,groundNormal);









		// ESSAIS DEPLACEMENT + vieux trucs
		//rb.AddForce (lastMoveDirection * runningSpeedAct * Time.deltaTime * 5000, ForceMode.Acceleration);
		//transform.Translate (lastMoveDirection * runningSpeedAct * Time.deltaTime);
		//rb.MovePosition (transform.position + move * 0.1f);
		/*
		if (move.magnitude > 0.2f) {
			rb.AddForce (move * runningSpeed * Time.deltaTime, ForceMode.Force);
		} else {
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}
		*/
		//rb.velocity.Set (move.x * runningSpeed * Time.deltaTime, move.y * runningSpeed * Time.deltaTime, 0f);
		//rb.velocity.Set (move * runningSpeed * Time.deltaTime);


		if (isGrounded) {

			//Changer la direction du perso seulement si il bouge 
			//Le perso ne doit pas revenir en direction 0 quand il arrete de marcher
			if (moveDistance > 0f) {
				//Si le stick est appuye
				//Changer la direction du perso 
				characterDirection = directionAngle;

				//Augmenter la vitesse de deplacement
				if (runningSpeedAct < runningSpeedMax)
					runningSpeedAct += runningSpeedAcceleration;

				//Si on est pas a la vitesse maximum
				if (rb.velocity.magnitude < velocityMagnetudeMax) 
					rb.AddForce ((lastMoveDirection + wallDecalage) * runningSpeedAct * Time.deltaTime, ForceMode.Force);

			} else {
				//Si le stick n'est plus appuye


				if (rb.velocity.magnitude > 0.1f) {
					//Ralentir le depacement jusqu'au freinage
					//runningSpeedAct = Mathf.Lerp (runningSpeedAct, 0f, 5f * Time.deltaTime);

					rb.AddForce (- rb.velocity * runningBrake * Time.deltaTime, ForceMode.VelocityChange);

				} else {
					//Bloquer le personnage
					runningSpeedAct = 0f;
					//rb.velocity = Vector3.left * 0f;
				}
			}

			//Change state for Animator
			if (move.magnitude <= 0.1f) {
				charState = CharacterState.Idle;
			} else {
				charState = CharacterState.Running;
			}

			//Permettre le saut
			HandleGroundedMovement (jump);

		} else {
			HandleAirborneMovement ();
		}

		//UpdateAnimator ();
		CheckForWalls();
		CheckGroundStatus ();

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
		#if UNITY_EDITOR
		Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.down * groundCheckDistance));
		#endif	
		if (Physics.Raycast (transform.position, Vector3.down, out hitInfo, groundCheckDistance)) {
			groundNormal = hitInfo.normal;
			isGrounded = true;
			//animator.applyRootMotion = true;
		} else {
			isGrounded = false;
			groundNormal = Vector3.up;
			//animator.applyRootMotion = false;
		}
	}

}
