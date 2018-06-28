using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using InControl;

public class PlayerSelectionCanvas : UIControllableCanvas {

	public UIPlayerDetectionCanvas parentCanvas;
	//public int canvasNumber;
	public bool assignedController;

	public InputDevice controller2;

	public Image characterImage;
	public Text pressAnyButtonText;
	public Text readyQuestionText;
	public Text readyFinalText;
	public Text controllerText;

	public enum SelectionState {PressAny, CharSelect, ReadyFinal, Start}
	SelectionState selectionState;
	SelectionState nextState;
	SelectionState previousState;

	public GameObject pressButtonScreen;
	public GameObject colorSelectionScreen;
	public GameObject readyToStartScreen;

	List<Color> colorList;
	int selectedColorNbr;

	PlayerCard card;



	// Use this for initialization
	void Start () {		
		LoadAvailableCharacterColors ();
		GetPlayerCard ();
		DisableAllScreens ();
		selectionState = SelectionState.PressAny;
		GoToNextState ();
	}

	void GetPlayerCard(){
		string playerCardPath = UsefulPath.playerCardData + "PlayerCard_" + (canvasNumber + 1) + ".asset";
		card = (PlayerCard)AssetDatabase.LoadAssetAtPath (playerCardPath, typeof(PlayerCard));
	}

	void LoadAvailableCharacterColors(){
		string dataPath = UsefulPath.characterColorData + "CharacterColorList" + ".asset";
		CharacterColors colorListData = (CharacterColors)AssetDatabase.LoadAssetAtPath (dataPath, typeof(CharacterColors));
		colorList = colorListData.colorsList;
	}

	public override void Initialize(){
		assignedController = true;
		UIPlayerController controller = PlayersManager.playersManager.playersConfig [canvasNumber].uiController;
		controllerText.text = controller.controller.Name;
	}

	void Update(){

		if (button1) {
			GoToNextState ();
		}

		if (button2) {
			GoToPreviousState ();
		}

		switch (selectionState) {
		case SelectionState.PressAny:
			break;
		case SelectionState.CharSelect:			
			if (left) {
				SetSelectedColor (-1);
			}
				
			if (right) {
				SetSelectedColor (+1);
			}
			break;
		case SelectionState.ReadyFinal:			
			break;
		}

		ResetButtons ();
	}

	/*
	public void SetPressAnyState(){
		selectionState = SelectionState.PressAny;
		characterImage.gameObject.SetActive (false);
		pressAnyButtonText.gameObject.SetActive (true);
		readyQuestionText.gameObject.SetActive (false);
		readyFinalText.gameObject.SetActive (false);
	}

	public void SetCharSelectState(){
		selectionState = SelectionState.CharSelect;
		characterImage.gameObject.SetActive (true);
		pressAnyButtonText.gameObject.SetActive (false);
		readyQuestionText.gameObject.SetActive (true);
		readyFinalText.gameObject.SetActive (false);
	}

	public void SetReadyQuestionState(){
		//selectionState = SelectionState.ReadyQuestion;
		characterImage.gameObject.SetActive (false);
		pressAnyButtonText.gameObject.SetActive (false);
		readyQuestionText.gameObject.SetActive (false);
		readyFinalText.gameObject.SetActive (true);
	}

	public void SetReadyFinalState(){
		parentCanvas.AllPlayersReady ();
	}
	*/


	//A DELETER
	public void SetPressAnyState(){
		
	}

	void SetSelectedColor(){
		characterImage.color = colorList [selectedColorNbr];
		SetColorIntoPlayerCard ();
	}

	void SetSelectedColor(int i){
		selectedColorNbr += i;
		if (selectedColorNbr < 0) {
			selectedColorNbr = colorList.Count - 1;
		}
		if (selectedColorNbr > colorList.Count - 1) {
			selectedColorNbr = 0;
		}
		characterImage.color = colorList [selectedColorNbr];
		SetColorIntoPlayerCard ();
	}

	void SetColorIntoPlayerCard(){
		card.playerColor = characterImage.color;
	}


	void GoToNextState(){
		selectionState = nextState;
		InitializeState (selectionState);
	}

	void GoToPreviousState(){
		selectionState = previousState;
		InitializeState (selectionState);
	}


	void InitializeState(SelectionState selectedState){
		
		DisableAllScreens ();

		switch (selectedState) {
		case SelectionState.PressAny:
			pressButtonScreen.gameObject.SetActive (true);
			previousState = SelectionState.PressAny;
			nextState = SelectionState.CharSelect;
			break;

		case SelectionState.CharSelect:
			SetSelectedColor ();
			colorSelectionScreen.gameObject.SetActive (true);
			previousState = SelectionState.PressAny;
			nextState = SelectionState.ReadyFinal;
			break;

		case SelectionState.ReadyFinal:
			readyToStartScreen.gameObject.SetActive (true);
			previousState = SelectionState.CharSelect;
			nextState = SelectionState.Start;
			break;

		case SelectionState.Start:
			parentCanvas.AllPlayersReady ();
			break;
		}
		
	}

	void ActiveNewScreen(GameObject screen){
		screen.gameObject.SetActive (true);
		foreach (Transform child in screen.transform) {
			child.gameObject.SetActive (true);
		}
	}

	void DisableAllScreens(){
		pressButtonScreen.gameObject.SetActive (false);
		colorSelectionScreen.gameObject.SetActive (false);
		readyToStartScreen.gameObject.SetActive (false);
	}








}
