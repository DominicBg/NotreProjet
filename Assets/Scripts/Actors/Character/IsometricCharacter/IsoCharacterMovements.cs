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
    public float airMoveVelocityMagnetudeMax = 7f;

	public float jumpPower = 360;
	[Range(1f, 100f)][SerializeField] float gravityMultiplier = 2f;
	[SerializeField] float groundCheckDistance = 100f;

	//Animator animator;
	public CapsuleCollider capsule;

	float origGroundCheckDistance;
    Vector3 eulerAngleVelocity; // Pour la rotation
	public bool isGrounded;
    public bool isLifted;
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
        Braking,
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

        rb.maxAngularVelocity = 0f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        eulerAngleVelocity = new Vector3(0, 100, 0);
	}

    public void Update()
    {
    }

    public void FixedUpdate(){
        
		SetCharacterDirectionForAnimation ();
		UpdateAnimator ();

        CheckGroundStatus();
        CheckWallStatus();
        ApplyGravity();
    }



    //Methode appelee au Fixed Update par IsometricCharacterController
    public void Move(Vector2 move, bool jump){

        ActualizeLastMoveDirection(move);

        if (isGrounded) {	
			HandleGroundedMovement (move, jump);
		} else {
			HandleAirborneMovement (move);
		}
    }

    void ActualizeLastMoveDirection (Vector2 move){
		if (move.magnitude > 0f){
			lastMoveDirection.x = move.x;
			lastMoveDirection.z = move.y;
		}
	}

	void CharacterRun (){
        //Augmenter la vitesse d'acceleration du deplacement
        if (runningSpeedAct <= runningSpeedMax - runningSpeedAcceleration)
        {
            runningSpeedAct += runningSpeedAcceleration;
        }


        //Si on est pas a la vitesse maximum				
        Vector2 horizontalMovement = new Vector2(rb.velocity.x, rb.velocity.z);

        //Acceleration dans la bonne direction
        rb.AddForce(lastMoveDirection * runningSpeedAct * Time.deltaTime, ForceMode.Acceleration);

        //Limiter la vitesse
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, velocityMagnetudeMax);

        charState = CharacterState.Running;
	}

	void CharacterBrakeRunning(){
		if (rb.velocity.magnitude > 0.1f) {	//Ralentir le depacement jusqu'au freinage					
			rb.AddForce (- rb.velocity * runningBrake * Time.deltaTime, ForceMode.Acceleration);
            charState = CharacterState.Braking;
        }
        else { //Bloquer le personnage					
			runningSpeedAct = 0f;
			charState = CharacterState.Idle;
		}
	}

    void CharacterJump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpPower, rb.velocity.z);
        SetNotOnGround();
        charState = CharacterState.Jumping;
    }

	void HandleGroundedMovement(Vector2 move, bool jump){

        if (move.magnitude > 0f)
        {
            CharacterRun();
        }
        else
        {
            CharacterBrakeRunning();
        }

        if (jump) {
            CharacterJump();
		}
	}

	void HandleAirborneMovement(Vector2 move){

        if(move.magnitude > 0f)
        {
            //Si on est pas a la vitesse maximum				
            Vector2 horizontalMovement = new Vector2(rb.velocity.x, rb.velocity.z);
            if(horizontalMovement.magnitude <= airMoveVelocityMagnetudeMax)
            {
                rb.AddForce(lastMoveDirection * 400 * Time.deltaTime, ForceMode.Acceleration);
            }

        }

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

	void CheckWallStatus(){

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

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;

        if (isGrounded)
        {
            //Voir si le personnage quitte le sol
            if (!Physics.Raycast(transform.position, Vector3.down, out hitInfo, groundCheckDistance))
            {
                SetNotOnGround();
            }
        }
        else if (!isGrounded)
        {
            //Voir si personnage atterit au sol
            if (rb.velocity.y < 0)
            {
                //Prediction de la position en Y a la prochaine frame
                //Faire la meme chose pour checker on WALLS
                float predictedY = -rb.velocity.y * Time.deltaTime;
                if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, predictedY))
                {
                    SetOnGround(hitInfo.point);
                }
            }
        }

        #if UNITY_EDITOR
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundCheckDistance), Color.black);
        #endif
    }

    void SetOnGround()
    {
        if (!isGrounded)
        {
            //Debug.Log("Switched to Grounded");
            isGrounded = true;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.angularVelocity = new Vector3(rb.angularVelocity.x, 0f, rb.angularVelocity.z);
        }
    }

    void SetOnGround(Vector3 hitPoint)
    {
        if (!isGrounded)
        {
            //Debug.Log("Switched to Grounded, Snapped");
            isGrounded = true;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.angularVelocity = new Vector3(rb.angularVelocity.x, 0f, rb.angularVelocity.z);

            //Snap to ground
            transform.position = hitPoint;
        }
    }
    void SetNotOnGround()
    {
        if (isGrounded)
        {
            //Debug.Log("Switched to NOT Grounded");
            isGrounded = false;
        }
    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            Vector3 extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
            rb.AddForce(extraGravityForce);
        }
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

        angle += 45;

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
