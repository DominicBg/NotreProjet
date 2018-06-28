using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

	public float speed; 
	public Vector3 targetPosition;
	public float totaleDistanceToTarget;
	public float actualDistanceToTarget;
	public bool isSticked = false;
	public bool isZoned = false;
	public float expRadius = 100f;

	public Debris debris;
	public int debrisMax;

	public BombZone zone;

	public float expForce = 800f;

	public Color bombColor;

	public Renderer rendererB;

	// Use this for initialization
	void Start () {

		zone = (BombZone)Resources.Load ("Prefabs/BombZone", typeof(BombZone));
	
		totaleDistanceToTarget = Vector3.Distance (transform.position, targetPosition);

		rendererB = GetComponent<Renderer> ();

		Debug.Log (rendererB);
		//rendererB.material.shader = Shader.Find ("_Color");
		Debug.Log (rendererB.material);
		Debug.Log (rendererB.material.shader);

		rendererB.material.SetColor ("_Color", bombColor);
		Color bombColor2 = bombColor;
		bombColor2.a = 0.5f;
		rendererB.material.SetColor ("_OutlineColor", bombColor2);
	}
	
	// Update is called once per frame
	void Update () {	
		rendererB.material.SetColor ("MainColor", Color.blue);

		actualDistanceToTarget = Vector3.Distance (transform.position, targetPosition);

		if (isSticked && !isZoned) {
			DisplayZone ();
		}

		if (actualDistanceToTarget <= 0.01) {
			isSticked = true;
		}

		if (!isSticked) {	
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, targetPosition, step);					
		} 
	}

	//Au declanchement de L'explosion
	public void Explode(){

		CreateDebris ();
		Vector3 posExplosion = transform.position;
		Collider[] colliders = Physics.OverlapSphere (posExplosion, expRadius);

		foreach (Collider hit in colliders) {
			IExplosable explosable = hit.GetComponent<IExplosable> ();

			//Si l'objet a l'interface IExplosable
			if (explosable != null) {				
				Rigidbody rb = hit.GetComponent<Rigidbody> ();
				rb.AddExplosionForce (expForce, posExplosion, 12f, 10f);
				Debug.Log ("EXPLOSABLE");
			}
		}

		Destroy (gameObject);
	}

	void CreateDebris(){
		for (int i = 0; i < debrisMax; i++) {
			Debris zzz = Instantiate (debris, transform.position + new Vector3 (Random.Range(-0.5f,0.5f), Random.Range(0,0.5f), Random.Range(-0.5f,0.5f)), transform.rotation);
			zzz.scale = Random.Range (0.1f, 4f);
		}
	}

	void MakeCameraShake(){
	}

	void DisplayZone(){
		BombZone bomb = Instantiate (zone, transform.position, transform.rotation);
		bomb.transform.parent = gameObject.transform;
		bomb.transform.localScale = Vector3.one * (expRadius * 2);
		isZoned = true;
	}


}
