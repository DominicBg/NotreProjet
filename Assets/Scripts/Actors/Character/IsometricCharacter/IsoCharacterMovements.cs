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

    public float ledgeCheckDistance = 1f;
    public float ledgeClimbingSpeed = 1f;
    public bool isOnLedge;
    public bool canClimbLedge;
    public float canClimbTime;
	Animator animator;

    public bool jumpButtonReleased;
    Vector3 lastMoveDirection;
    public GameObject lastLedgeGrabbedObject;

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
        LedgeGrabbing,
	}
	public CharacterState charState;
    CharacterState lastCharState;


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
    public void Move(Vector2 move, bool jump, bool cancel){

        ActualizeLastMoveDirection(move);

        if (isGrounded) {	
			HandleGroundedMovement (move, jump);
		} else {
            if (isOnLedge)
            {
                HandleLedgedMovement(jump, cancel);
            }
            else
            {
                HandleAirborneMovement(move);
            }
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

        //Wait For Jump Button to be Released to be able to Jump again
        jumpButtonReleased = false;
    }

    void CharacterLedgeGrab(Vector3 hitPoint)
    {
        //Happens only at grabbing moment
        if (!isOnLedge)
        {
            SetOnLedge();

            //transform.Translate(Vector3.down * (hitPoint.y - 2));
            transform.position = new Vector3(transform.position.x, hitPoint.y - 2, transform.position.z);

            //Send a Grab Direction that won't change to the Animation
            Vector3 animGrabDirection = Quaternion.AngleAxis(45, Vector3.up) * lastMoveDirection;
            animator.SetFloat("AnimGrabDirectionX", animGrabDirection.x);
            animator.SetFloat("AnimGrabDirectionY", animGrabDirection.z);

            //Trigger the animation only once
            charState = CharacterState.LedgeGrabbing;

            //Wait For Jump Button to be Released to be able to Jump From Ledge
            jumpButtonReleased = false;
        }        
    }

    void CharacterLedgeClimb()
    {
        rb.velocity = new Vector3(rb.velocity.x, ledgeClimbingSpeed, rb.velocity.z);
        isOnLedge = false;

        charState = CharacterState.Jumping;
        // ^  A remplacer par 
        //charState = CharacterState.LedgeClimbing;

    }

    void CharacterLedgeDrop()
    {
        SetNotOnLedge();
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

        if (jumpButtonReleased && jump) {
            CharacterJump();
		}
        else if (!jump)
        {
            jumpButtonReleased = true;
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

        //Character deviendra onLedge si necessaire
        CheckLedgeStatus();

    }

    void HandleLedgedMovement(bool jump, bool cancel)
    {
        rb.velocity = Vector3.zero;     
        
        if (jumpButtonReleased && jump)
        {
            CharacterLedgeClimb();
        }
        else if (!jumpButtonReleased && jump)
        {

        }
        else if (!jump)
        {
            jumpButtonReleased = true;
        }

        if (cancel)
        {
            CharacterLedgeDrop();
        }
    }

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
            if (rb.velocity.y < -0.001f)
            {
                //Prediction de la position en Y a la prochaine frame
                //Faire la meme chose pour checker on WALLS
                float predictedY = -rb.velocity.y * Time.deltaTime;
                if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, predictedY))
                {
                    SetOnGround(hitInfo.point);
                }
            }
            //Si le personnage n'est pas "OnGround" mais ne descend pas ou peu
            //Situation ou le perso est stuck on edge
            else if (rb.velocity.y == 0f)
            {
                if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 0.1f))
                {
                    SetOnGround();
                }
            }
            else if (rb.velocity.y >= -0.001f && rb.velocity.y <= 0f)
            {
                Debug.Log("Stuck avec Y plus petit que zero");
                SetOnGround();
            }
        }

#if UNITY_EDITOR
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundCheckDistance), Color.red);
        #endif
    }

    void CheckLedgeStatus()
    {
        RaycastHit hitInfo;        
        Vector3 ledgeDetectionStart = transform.position + lastMoveDirection + Vector3.up * 2 ;

        Debug.DrawRay(ledgeDetectionStart,(Vector3.down), Color.red);

        //Start ledge detection from characters height
        if (!Physics.Raycast(ledgeDetectionStart, Vector3.down, out hitInfo, 0.1f))
        {
            //If there is nothing higher than character
            //Look down for a ledge
            //Il n'y a pas de mur au dessus, on peut regarder vers le bas
            if (Physics.Raycast(ledgeDetectionStart, Vector3.down, out hitInfo, ledgeCheckDistance))
            {

                //Cannot Ledge again at same place
                //Last LedgeGrabbed Object is Reseted From "SetOnGround"
                if (hitInfo.collider.gameObject != lastLedgeGrabbedObject)
                {
                    Debug.Log("Different Ledge");

                    //Il y a un mur en bas, on peut s'accrocher                
                    if (hitInfo.normal == Vector3.up)
                    {
                        CharacterLedgeGrab(hitInfo.point);
                    }
                    else if (hitInfo.normal.y > -0.75f && hitInfo.normal.y < 0.25f)
                    {
                        Debug.Log("Ledge Normal != 1 : " + hitInfo.normal.y);
                        CharacterLedgeGrab(hitInfo.point);
                    }

                    lastLedgeGrabbedObject = hitInfo.collider.gameObject;
                }
                else
                {
                    Debug.Log("Cannot Ledge because of Last LedgeGrabbed Object");
                }
            }
            else
            {
                //There is no ledge to climb
            }
        } else
        {
            Debug.Log("Wall Blocking a Ledge");
            SetNotOnLedge();
        }

    }

    void SetOnLedge()
    {
        if (!isOnLedge)
        {
            isOnLedge = true;
        }
    }

    void SetNotOnLedge()
    {
        if (isOnLedge)
        {
            isOnLedge = false;
        }
    }

    void SetOnGround()
    {
        if (!isGrounded)
        {
            //Debug.Log("Switched to Grounded");
            isGrounded = true;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.angularVelocity = new Vector3(rb.angularVelocity.x, 0f, rb.angularVelocity.z);

            isOnLedge = false;

            ResetLastLedgeGrabbedObject();
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

            isOnLedge = false;

            ResetLastLedgeGrabbedObject();
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

    void ResetLastLedgeGrabbedObject()
    {
        lastLedgeGrabbedObject = null;
    }

    void ApplyGravity()
    {
        if (!isGrounded && !isOnLedge)
        {
            Vector3 extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
            rb.AddForce(extraGravityForce);
        }
    }

	void UpdateAnimator (){


        animator.SetInteger ("Direction", directionAnim);

        //Rotation du vecteur de direction
        //Exemple
        //vector = Quaternion.AngleAxis(-45, Vector3.up) * vector;
        Vector3 animDirection;
        animDirection = Quaternion.AngleAxis(45, Vector3.up) * lastMoveDirection;
        animator.SetFloat("AnimDirectionX", animDirection.x);
        animator.SetFloat("AnimDirectionY", animDirection.z);
        
        //Si il y a un changement de CharState
        if ( charState != lastCharState)
        {
            switch (charState)
            {
                case CharacterState.Idle:
                    animator.SetTrigger("Idle");
                    break;
                case CharacterState.Running:
                    animator.SetTrigger("Run");
                    break;
                case CharacterState.Braking:
                    animator.SetTrigger("Idle");
                    break;
                case CharacterState.Jumping:
                    animator.SetTrigger("Jump");
                    break;
                case CharacterState.Falling:
                    //animator.SetTrigger ("Falling");
                    break;
                case CharacterState.LedgeGrabbing:
                    animator.SetTrigger("LedgeGrab");
                    break;

            }

            lastCharState = charState;
        }
	}

    // A supprimer quand toute les animation utiliserons un BLEND TREE
	void SetCharacterDirectionForAnimation(){
		
		int angle = Mathf.RoundToInt(Vector3.SignedAngle(lastMoveDirection, Vector3.forward, Vector3.up));

        angle += 45;


        if (angle > -22.5 && angle < 22.5)
        {
            directionAnim = 0; //Droite
        }
        else if (angle > 22.5 && angle < 67.5)
        {
            directionAnim = 7;
        }
        else if (angle > 67.5 && angle < 112.5)
        {
            directionAnim = 6; //Haut
        }
        else if (angle > 112.5 && angle < 157.5)
        {
            directionAnim = 5;
        }
        else if (angle > 157.5 || angle < -157.5)
        {
            directionAnim = 4; // Gauche
        }
        else if (angle > -157.5 && angle < -112.5)
        {
            directionAnim = 3;
        }
        else if (angle > -112.5 && angle < -67.5)
        {
            directionAnim = 2; //Bas
        }
        else if (angle > -67.5 && angle < -22.5)
        {
            directionAnim = 1;
        }
    }

}
