using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Modes;

public class MainMenuCanvas : MonoBehaviour {

	//Elements a afficher
	public Text textPlayersNumber;
	public Text textGameModeName;
	public Text textGameModeDescription;
	public Text textLevelName;
	public Image imageLevel;
	public Text textLevelDescription;

	public Button gameModeLeft;
	public Button gameModeRight;
	public Button playersNumberLeft;
	public Button playersNumberRight;
	public Button mapNameLeft;
	public Button mapNameRight;
	public Button start;

	//Player Number Selection
	private int intSelectedPlayerNumber;

	//Game Mode Selection
	public GameModeData[] gameModeList;
	public GameModeData[] availableGameModes;
	public GameModeData selectedGameModeData;
	public int intSelectedGameMode;

	//Level Data Selection
	public LevelData[] levelList;
	public LevelData[] availableLevels;
	public LevelData selectedlevelData;
	public int intSelectedLevel;

	public GMLaunchGameFromStart mainMenuManager;



	void Awake (){
		//Charger la list de LevelDatas
		LevelListData dataLevelList = (LevelListData)AssetDatabase.LoadAssetAtPath (UsefulPath.listLevelData, typeof(LevelListData));
		levelList = dataLevelList.leveldatas;

		//Charger la list de GameModeDatas
		GameModeListData dataGameModeList = (GameModeListData)AssetDatabase.LoadAssetAtPath ( UsefulPath.listGameModeData, typeof(GameModeListData));
		gameModeList = dataGameModeList.gameModeDatas;
	}

	// Use this for initialization
	void Start () {		
		gameModeLeft.onClick.AddListener(GameModeLeftPressed);
		gameModeRight.onClick.AddListener (GameModeRightPressed);
		playersNumberLeft.onClick.AddListener(PlayersNumberLeftPressed);
		playersNumberRight.onClick.AddListener(PlayersNumberRightPressed);
		mapNameLeft.onClick.AddListener(MapNameLeftPressed);
		mapNameRight.onClick.AddListener(MapNameRightPressed);
		start.onClick.AddListener (StartGame);

		intSelectedPlayerNumber = 0;
		intSelectedGameMode = 0;
		intSelectedLevel = 0;
		SelectPlayerNumber (+1);
	}

	void SelectPlayerNumber(int numberModif)
	{
		intSelectedPlayerNumber += numberModif;
		if (intSelectedPlayerNumber < 1)
			intSelectedPlayerNumber = 5;
		if (intSelectedPlayerNumber > 5)
			intSelectedPlayerNumber = 1;
		textPlayersNumber.text = intSelectedPlayerNumber.ToString ();		
		RefreshSelectableGameMode ();
	}

	void RefreshSelectableGameMode(){	
		//Regarder le nombre de modes compatibles
		//Pour creer un array de la bonne taille
		int availablesModes = 0;
		availableGameModes = new GameModeData[0];
		foreach(GameModeData zzz in gameModeList){
			if (zzz.gameModeMinPlayer <= intSelectedPlayerNumber && zzz.gameModeMaxPlayer >= intSelectedPlayerNumber){
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
				if (aaa.gameModeMinPlayer <= intSelectedPlayerNumber && aaa.gameModeMaxPlayer >= intSelectedPlayerNumber){
					availableGameModes [transferedModes] = aaa;
					transferedModes++;
				}
			}
			SelectGameMode (0);
		} else {
			//Si il n'y a aucun GameMode compatible avec le nombre de joueurs
			availableGameModes = null;
			availableLevels = null;
			textGameModeName.text = "Aucun";
			textLevelName.text = "Aucun";
		}
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
			textGameModeName.text = selectedGameModeData.gameModeName;
			/* Remettre au debut si le gameMode actuel n'est pas jouable au nombre de joueurs selectionne
		Exemple : Etre sur 3 joueurs, selectionner teamDeathmatch(joueurMin 3)
		Mettre le nombe de joueurs sur 1. TDM n'est plus jouable */
			if (selectedGameModeData.gameModeMinPlayer > intSelectedPlayerNumber) {
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
				&& intSelectedPlayerNumber >= ppp.levelMinPlayers
				&& intSelectedPlayerNumber <= ppp.levelMaxPlayers) {
				tempAvailableLevel++;
			}
		}
		//Si il y a au moins 1 level compatible
		if (tempAvailableLevel >= 1) {
			availableLevels = new LevelData [tempAvailableLevel];
			int transferedLevel = 0;
			foreach (LevelData qqq in levelList) {
				if (qqq.gameMode == selectedGameModeData 
					&& intSelectedPlayerNumber >= qqq.levelMinPlayers
					&& intSelectedPlayerNumber <= qqq.levelMaxPlayers) {
					availableLevels [transferedLevel] = qqq;
					transferedLevel++;
				}
			}
			SelectLevel (0);
		} else {
			availableLevels = null;
			textLevelName.text = "Aucun";		
		}
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
			textLevelName.text = selectedlevelData.mapName;
			if (selectedlevelData.gameMode != selectedGameModeData) {
				selectedlevelData = availableLevels [0];
			}
			textLevelDescription.text = selectedlevelData.levelDescription;
			imageLevel.sprite = selectedlevelData.imageLevel;
		}
	}

	void GameModeLeftPressed(){
		SelectGameMode (-1);
	}

	void GameModeRightPressed(){
		SelectGameMode (+1);
	}

	void PlayersNumberLeftPressed(){
		SelectPlayerNumber (-1);
	}

	void PlayersNumberRightPressed(){
		SelectPlayerNumber (+1);
	}

	void MapNameLeftPressed(){
		SelectLevel (-1);
	}

	void MapNameRightPressed(){
		SelectLevel (+1);
	}

	void StartGame(){

		if (availableGameModes != null && availableLevels != null) {
			//mainMenuManager.StartLevel (selectedlevelData);
		}
	}

}
