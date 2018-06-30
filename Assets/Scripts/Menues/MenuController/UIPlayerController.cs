using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using InControl;

public class UIPlayerController : MonoBehaviour {

	public int playerNumber;
	
	public InputDevice controller;

	float actionButtonRate;

	float directionButtonRate;

	public bool button1;
	public bool button1Ready;
	public bool button2;
	public bool button2Ready;

	public bool up;
	public bool down;
	public bool left;
	public bool right;
	public bool directionReady;

	public UIControllableCanvas connectedCanvas;


	// Use this for initialization
	void Start () {

		button1Ready = true;
		button2Ready = true;
		directionReady = true;

	}
	
	// Update is called once per frame
	void Update () {


		if (controller != null && connectedCanvas == null && controller.AnyButton) {
			GetCanvas ();
		}

		if (controller != null && connectedCanvas != null) {			

			if (controller.Action1.WasPressed && button1Ready) {
				connectedCanvas.button1 = true;
				StartCoroutine ("Button1Wait");
			}

			if (controller.Action2.WasPressed && button2Ready) {
				connectedCanvas.button2 = true;
				StartCoroutine ("Button2Wait");
			}

			if (controller.Direction.WasPressed && directionReady) {
				TwoAxisInputControl zzz = controller.Direction;
				if (zzz.Down.IsPressed)
					connectedCanvas.down = true;
				if (zzz.Up.IsPressed)
					connectedCanvas.up = true;
				if (zzz.Left.IsPressed)
					connectedCanvas.left = true;
				if (zzz.Right.IsPressed)
					connectedCanvas.right = true;
				StartCoroutine ("DirectionWait");
			}
		}
		// FIN DU IF CONTROLLER != NULL
	}

	IEnumerator Button1Wait(){	
		button1Ready = false;
		yield return new WaitForSeconds (0.1f);
		button1Ready = true;
	}

	IEnumerator Button2Wait(){
		button2Ready = false;
		yield return new WaitForSeconds (0.1f);
		button2Ready = true;
	}

	IEnumerator DirectionWait(){			
		directionReady = false;
		yield return new WaitForSeconds (0.1f);
		directionReady = true;
	}


	public void GetCanvas(){
		 
		UIControllableCanvas[] zzz = Object.FindObjectsOfType(typeof(UIControllableCanvas)) as UIControllableCanvas[] ;

		foreach (UIControllableCanvas canvas in zzz) {
			if (canvas.canvasNumber == playerNumber) {
				ConnectToCanvas (canvas);
			}
		}
	}




	public void ConnectToCanvas(UIControllableCanvas canvas){		
		connectedCanvas = canvas;
		canvas.Initialize ();
	}
}
