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

    public bool restartButton;

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

        //STate : Looking for a canvas
		if (controller != null && connectedCanvas == null && controller.AnyButton) {
			GetCanvas ();
		}
        
        //STATE :Utilisation de Canvas connecte
        if (controller != null && connectedCanvas != null)
        {
            //Se deplacer dans un Canvas connecte
            UseConnectedCanvas();
        }


        if (controller != null && controller.CommandIsPressed)
        {
            Debug.Log("Restart asked from : " + this);
            RestartLevel();
        }


    }

    //UseConnectedCanvas
    void UseConnectedCanvas()
    {
        //Se deplacer dans un Canvas connecte
        if (controller.Action1.WasPressed && button1Ready)
        {
            connectedCanvas.button1 = true;
            StartCoroutine("Button1Wait");
        }

        if (controller.Action2.WasPressed && button2Ready)
        {
            connectedCanvas.button2 = true;
            StartCoroutine("Button2Wait");
        }

        if (controller.Direction.WasPressed && directionReady)
        {
            TwoAxisInputControl zzz = controller.Direction;

            //Conversion de l'angle du stick en direction Haut Droite Bas Gauche
            int angle = Mathf.RoundToInt(zzz.Angle);

            if (angle > 315 || angle <= 45)
            {
                //Debug.Log("1");
                connectedCanvas.up = true;
            }
            else if (angle > 45 && angle <= 135)
            {
                //Debug.Log("2");
                connectedCanvas.left = true;
            }
            else if (angle > 135 && angle <= 215)
            {
                //Debug.Log("3");
                connectedCanvas.down = true;
            }
            else if (angle > 215 && angle <= 315)
            {
                //Debug.Log("4");
                connectedCanvas.right = true;
            }
            StartCoroutine("DirectionWait");
        }

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


    void RestartLevel()
    {
        GameManager.gameManager.RestartLevel();
    }

}
