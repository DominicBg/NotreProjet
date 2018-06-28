using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IDamageable, IExplosable {

	public int health;
	public bool isExploding = false;
	public float explosionDuration = 1f;

	public Rigidbody rb;




	// Use this for initialization
	void Start () {

		rb = this.GetComponent<Rigidbody>();

		health = 1;
	}

	public virtual void Damage(int damageValue){
		health = 0;
	}

	public void Explode(Vector3 posExplosion){
		
	}
}
