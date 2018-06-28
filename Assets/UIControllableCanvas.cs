using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllableCanvas : MonoBehaviour {

	public int canvasNumber;

	public bool button1;
	public bool button2;

	public bool up;
	public bool down;
	public bool left;
	public bool right;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//ResetButtons ();

		/*
		if (button1)
			Validate ();

		if (button2)
			Cancel ();
		*/
	}

	public void Validate(){
	
	}

	public void Cancel(){
	
	}

	public void ResetButtons(){
		
		if (button1)
			button1 = false;

		if (button2)
			button2 = false;

		if (up)
			up = false;

		if (down)
			down = false;

		if (left) 
			left = false;			

		if (right)
			right = false;

	}

	public virtual void Initialize(){

	}

}


