using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectoire : MonoBehaviour {


	public int pointsNumber;
	public TrajectoireTracer tracer;
	public Rigidbody tracerRb;
	public bool traceNew;
	public bool tracerIsPlaced;


	public Vector3 bombPosition;
	public Vector3 characterPosition;

	public LineRenderer lineRenderer;
	public float tracingDuration;
	public float timeBeforePoint;

	public float forceExp;

	Coroutine createPoints; 

	// Use this for initialization
	void Start () {
		tracer = Instantiate (tracer);
		tracerRb = tracer.GetComponent<Rigidbody> ();

		//createPoints = CreatePoints ();
	}
	
	// Update is called once per frame
	void Update () {
		traceNew = Input.GetButtonDown ("ButY0");
		if (traceNew) {
			CreateTrajectoire ();
		}
	}

	void CreateTrajectoire(){

		if (!tracerIsPlaced) {
			if (createPoints != null) 
				StopCoroutine(createPoints);
			SearchForBombs ();
			PlaceTracer ();
			//Mettre a zero la ligne
			lineRenderer.positionCount = 0;

		} else {
			tracerRb.AddExplosionForce (forceExp, bombPosition, 12f, 10f);
			tracerIsPlaced = false;
			createPoints = StartCoroutine (CreatePoints());
		}
	}

	void SearchForBombs(){
		Collider[] colliders = Physics.OverlapSphere (transform.position, 10);
		foreach (Collider hit in colliders) {
			Bomb bomb = hit.GetComponent<Bomb> ();
			//Si l'objet est une bomb
			if (bomb != null) {
				Debug.Log ("Bomb Found :" + hit); 
				bombPosition = bomb.transform.position;
				forceExp = bomb.expForce;
			}
		}
	}

	void PlaceTracer(){
		tracerRb.velocity = Vector3.zero;
		tracer.transform.position = transform.position;
		tracerIsPlaced = true;
	}



	IEnumerator CreatePoints(){
		timeBeforePoint = tracingDuration / pointsNumber;
		for (int i = 0; i < pointsNumber; i++) {			
			lineRenderer.positionCount++;
			lineRenderer.SetPosition (i, tracer.transform.position);
			yield return new WaitForSeconds (timeBeforePoint);
		}
		yield return null;
	}





}
