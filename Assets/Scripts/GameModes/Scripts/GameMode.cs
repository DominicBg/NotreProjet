using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour {

	public GameManager gameManager;

	public InGameUI inGameUI;

	public enum GameModeState
	{
		Starting, Playing, Ending
	}
	public GameModeState gameModeState;

	bool startingDone;
	bool playingDone;
	bool endingDone;

	public void Initialize(){
		//Obligatoire pour l'instant
		gameManager = GameManager.gameManager;
		inGameUI = (InGameUI)FindObjectOfType (typeof(InGameUI));

		startingDone = false;
		playingDone = false;
		endingDone = false;

		//Execute dans le script enfant
		ModeInitialize ();

	} 

	public virtual void ModeInitialize(){	
	}

	//Une fois que tout est charge et que le niveau peut etre lance
	public void GameStart () {
		//Initiation de la stateMachine
		gameModeState = GameModeState.Starting;
	}
	
	public void UpdateMode () {
		switch (gameModeState) {
		case GameModeState.Starting:
			StartingMode ();
			break;
		case GameModeState.Playing:
			PlayingMode ();
			break;
		case GameModeState.Ending:	
			EndingMode ();
			break;
		}


	}

	void StartingMode(){
		switch (startingDone) {
		case false:
			//Ne faire qu'une seule fois au lancement
			inGameUI.FadeToBlack (false, 1f);
			ModeStartingStart ();
			startingDone = true;
			break;
		case true:
			//A la fin du fade, changer de mode
			//Ajouter Activation des personnages
			if (!inGameUI.isFading) {
				gameModeState = GameModeState.Playing;
			}
			//Repeter pendant qu'on reste dans ce mode
			ModeStartingUpdate ();
			break;
		}
	}

	void PlayingMode(){
		switch (playingDone) {
		case false:
			//Activer le controle des personnages
			gameManager.playersManager.EnableAllCharacters ();
			ModePlayingStart ();
			playingDone = true;
			break;
		case true:
			ModePlayingUpdate ();
			break;
		}
	}

	void EndingMode(){
		switch (endingDone) {
		case false:			
			inGameUI.FadeToBlack (true, 1f);
			gameManager.playersManager.DisableAllCharacters ();
			ModeEndingStart();
			endingDone = true;
			break;
		case true:
			if (!inGameUI.isFading) {
				EndMode ();
			}
			ModeEndingUpdate();
			break;
		}
	}

	public virtual void EndMode(){		
		gameManager.EndLevel ();
	}

    public virtual void RestartMode()
    {
        //Can only be called from PlayingMode
        if (gameModeState == GameModeState.Playing)
        {
            //Called to reset bool values
            gameManager.RestartLevel();


        }

    }

	//Fonction realisees dans les scripts des gameModes
	public virtual void ModeStartingStart(){	
	}

	public virtual void ModeStartingUpdate(){
	}

	public virtual void ModePlayingStart(){
	}

	public virtual void ModePlayingUpdate(){	
	}

	public virtual void ModeEndingStart(){
	}

	public virtual void ModeEndingUpdate (){	
	}
}
