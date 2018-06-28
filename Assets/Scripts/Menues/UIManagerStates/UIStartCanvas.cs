using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStartCanvas : UIState, IUIManager {

	public Button pressAnyButton;





	//ON ACTIVATION
	void Start () {
		pressAnyButton.onClick.AddListener (AnyButtonPressed);
	}
	
	void Update () {
		
	}


	void AnyButtonPressed(){
		UIManager.uiManager.ChangeMode (UIManager.uiManager.playerDetectionCanvas);
	}

	//Interface methods
	public void Enable(){
	}

	public void Disable(){

	}
}
