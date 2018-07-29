using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour, IExplosable {

	public float scale;

	public Material blue;
	public Material yellow;
	public Material red;

	public Rigidbody rb;
	public Renderer rend;
	public Material mat;

	public float r;
	public float g;
	public float b;

	public Vector3 direction;

	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody> ();
		rend = GetComponent<Renderer> ();
		mat = rend.GetComponent<Material> ();

		Destroy (gameObject,0.3f);
		transform.localScale = Vector3.one * scale;
		ChangeColor ();

		direction = new Vector3 (Random.Range (-1f, 1f), Random.Range (-1f, 1f), Random.Range (-1f, 1f));

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){

	//	Quaternion zzz = rb.rotation;
		transform.Translate(direction, Space.Self);
	}


	public void Explode(Vector3 zzz){
	
	}

	void ChangeColor(){

		if (scale <= 1) {
			//rend.material = blue;
			r = 0f;
			g = 0f;
			b = 1f;

		} else if (scale <= 2) {
			//rend.material = blue;
			r = 1f;
			g = 1f;
			b = 0f;
		} else if (scale <= 3) {
			//rend.material = yellow;
			r = 1f;
			g = 0f;
			b = 0f;
		} else if (scale <= 4) {
			//rend.material = red;
			r = 1f;
			g = 0f;
			b = 0f;
		} else if (scale <= 5) {
			//rend.material = blue;
			r = 0f;
			g = 0f;
			b = 0f;
		} else if (scale <= 6) {

		}

		StartCoroutine(FadeOut());
	}

	IEnumerator FadeOut(){
		for (float i = 0; i < 1; i += Time.deltaTime / 0.3f) {
			//Debug.Log ("COUROUTRINE");
			float j = 1 - i;
			rend.material.color = new Color (r, g, b, j);
			yield return null;

		}

	}
}
