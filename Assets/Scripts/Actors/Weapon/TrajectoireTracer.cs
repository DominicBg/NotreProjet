using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoireTracer : MonoBehaviour {

	public Rigidbody rb;
	[Range(1f, 100f)][SerializeField] float gravityMultiplier = 2f;

	[SerializeField] float groundCheckDistance = 100f;
	public float origGroundCheckDistance;
	public bool isGrounded;
	Vector3 groundNormal;

	// Use this for initialization
	void Start () {
		origGroundCheckDistance = groundCheckDistance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){

		if (isGrounded) {
		
		} else {
			HandleAirborneMovement ();
		}
		CheckGroundStatus ();
	}

	void HandleAirborneMovement(){
		Vector3 extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
		rb.AddForce (extraGravityForce);
		groundCheckDistance = rb.velocity.y < 0 ? origGroundCheckDistance : 0.01f;

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
