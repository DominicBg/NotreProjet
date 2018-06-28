using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


public class NewGameSelectionCanvas : UIControllableCanvas {

	public UINewGameCanvas parentCanvas;
	public int playersNumber;

	//GameMode variables
	public Text gameModeNameText;
	public Text gameModeDescriptionText;
	public Image gameModeTextButton;
	//GameMode Data Selection
	public GameModeData[] gameModeList;
	public GameModeData[] availableGameModes;
	public GameModeData selectedGameModeData;
	public int intSelectedGameMode;

	//Level Selection variables
	public Text levelNameText;
	public Image levelTextButton;
	public Text levelDescriptionText;
	public Image levelImage;
	//Level Data Selection
	public LevelData[] levelList;
	public LevelData[] availableLevels;
	public LevelData selectedlevelData;
	public int intSelectedLevel;

	//Start Mode
	//public Button start;
	public Text startTextButton;

	public enum SelectionState{GameMode, Level, Start}
	SelectionState selectionState;

	void Awake(){
		//Charger la list de LevelDatas
		LevelListData dataLevelList = (LevelListData)AssetDatabase.LoadAssetAtPath (UsefulPath.listLevelData, typeof(LevelListData));
		levelList = dataLevelList.leveldatas;

		//Charger la list de GameModeDatas
		GameModeListData dataGameModeList = (GameModeListData)AssetDatabase.LoadAssetAtPath ( UsefulPath.listGameModeData, typeof(GameModeListData));
		gameModeList = dataGameModeList.gameModeDatas;
	}


	// Use this for initialization
	void Start () {
		playersNumber = PlayersManager.playersManager.playersNumber;
		SelectionChange (SelectionState.GameMode);
		PlayersManager.playersManager.ConnectCanvasToControllers (this);

		intSelectedGameMode = 0;
		intSelectedLevel = 0;

		RefreshSelectableGameMode ();
	}
	
	// Update is called once per frame
	void Update () {

		switch (selectionState) {
		case SelectionState.GameMode:
			if (left) {
				GameModeLeftPressed ();
			}
			if (right) {
				GameModeRightPressed ();
			}		
			if (down) {
				//selectionState = SelectionState.Level;
				SelectionChange (SelectionState.Level);
			}
			break;
		case SelectionState.Level:
			if (left) {
				MapNameLeftPressed ();
			}
			if (right) {
				MapNameRightPressed ();
			}
			if (up) {
				//selectionState = SelectionState.GameMode;
				SelectionChange (SelectionState.GameMode);
			}
			if (down) {
				//selectionState = SelectionState.Start;
				SelectionChange (SelectionState.Start);
			}
			break;
		case SelectionState.Start:
			if (button1) {
				StartSelectedMode ();
			}
			if (up) {
				//selectionState = SelectionState.Level;
				SelectionChange (SelectionState.Level);
			}
			break;
		}

		ResetButtons ();
	}

	void SelectionChange(SelectionState newState){
		selectionState = newState;

		Color gameModeColor = gameModeTextButton.color;

		Color levelColor = levelTextButton.color;

		switch (selectionState) {
		case SelectionState.GameMode:
			gameModeTextButton.color = Color.cyan;
			levelTextButton.color = Color.white;
			break;
		case SelectionState.Level:
			levelTextButton.color = Color.cyan;
			gameModeTextButton.color = Color.white;
			break;
		case SelectionState.Start:
			levelTextButton.color = Color.white;
			gameModeTextButton.color = Color.white;
			break;
		}
	}



	void RefreshSelectableGameMode(){	
		//Regarder le nombre de modes compatibles
		//Pour creer un array de la bonne taille
		int availablesModes = 0;
		availableGameModes = new GameModeData[0];
		foreach(GameModeData zzz in gameModeList){
			if (zzz.gameModeMinPlayer <= playersNumber && zzz.gameModeMaxPlayer >= playersNumber){
				availablesModes++;
			}
		}
		//Si il y a au moins 1 GameMode
		if (availablesModes >= 1) {
			//Creer l'array de la bonne taille
			availableGameModes = new GameModeData[availablesModes];
			//Inserer les modes compatibles
			int transferedModes = 0;
			foreach(GameModeData aaa in gameModeList){
				if (aaa.gameModeMinPlayer <= playersNumber && aaa.gameModeMaxPlayer >= playersNumber){
					availableGameModes [transferedModes] = aaa;
					transferedModes++;
				}
			}
			SelectGameMode (0);
		} else {
			//Si il n'y a aucun GameMode compatible avec le nombre de joueurs
			availableGameModes = null;
			availableLevels = null;
			gameModeNameText.text = "Aucun";
			levelNameText.text = "Aucun";
		}
	}





	void GameModeLeftPressed(){
		SelectGameMode (-1);
	}

	void GameModeRightPressed(){
		SelectGameMode (+1);
	}

	void SelectGameMode(int numberModif){

		//Si il y a des GameModes disponibles
		if (availableGameModes != null) {
			intSelectedGameMode += numberModif;
			if (intSelectedGameMode < 0)
				intSelectedGameMode = availableGameModes.Length -1;
			if (intSelectedGameMode > availableGameModes.Length -1)
				intSelectedGameMode = 0;
			selectedGameModeData = availableGameModes [intSelectedGameMode];
			gameModeNameText.text = selectedGameModeData.gameModeName;
			/* Remettre au debut si le gameMode actuel n'est pas jouable au nombre de joueurs selectionne
		Exemple : Etre sur 3 joueurs, selectionner teamDeathmatch(joueurMin 3)
		Mettre le nombe de joueurs sur 1. TDM n'est plus jouable */
			if (selectedGameModeData.gameModeMinPlayer > playersNumber) {
				selectedGameModeData = availableGameModes [0];
			}
			RefreshSelectableLevel ();		
		}
	}

	void RefreshSelectableLevel(){			
		//Pareil que les GameModes mais avec les Levels
		int tempAvailableLevel = 0;
		availableLevels = new LevelData[0];
		foreach (LevelData ppp in levelList) {
			if (ppp.gameMode == selectedGameModeData 
				&& playersNumber >= ppp.levelMinPlayers
				&& playersNumber <= ppp.levelMaxPlayers) {
				tempAvailableLevel++;
			}
		}
		//Si il y a au moins 1 level compatible
		if (tempAvailableLevel >= 1) {
			availableLevels = new LevelData [tempAvailableLevel];
			int transferedLevel = 0;
			foreach (LevelData qqq in levelList) {
				if (qqq.gameMode == selectedGameModeData 
					&& playersNumber >= qqq.levelMinPlayers
					&& playersNumber <= qqq.levelMaxPlayers) {
					availableLevels [transferedLevel] = qqq;
					transferedLevel++;
				}
			}
			SelectLevel (0);
		} else {
			availableLevels = null;
			levelNameText.text = "Aucun";		
		}
	}

	void MapNameLeftPressed(){
		SelectLevel (-1);
	}

	void MapNameRightPressed(){
		SelectLevel (+1);
	}

	void SelectLevel(int numberModif){

		//Si il y a des Level disponibles
		if (availableLevels != null) {
			intSelectedLevel += numberModif;
			if (intSelectedLevel < 0)
				intSelectedLevel = availableLevels.Length - 1;
			if (intSelectedLevel > availableLevels.Length - 1)
				intSelectedLevel = 0;
			selectedlevelData = availableLevels [intSelectedLevel];
			levelNameText.text = selectedlevelData.mapName;
			if (selectedlevelData.gameMode != selectedGameModeData) {
				selectedlevelData = availableLevels [0];
			}
			levelDescriptionText.text = selectedlevelData.levelDescription;
			levelImage.sprite = selectedlevelData.imageLevel;
		}
	}

	void StartSelectedMode(){
		if (availableGameModes != null && availableLevels != null) {
			GameManager.gameManager.levelData = selectedlevelData;
			GameManager.gameManager.StartLevel ();

			//AJOUTER 

			UIManager.uiManager.ChangeMode (UIManager.uiManager.inGameCanvas);
		}
	}

}
