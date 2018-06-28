using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeAdventure : GameMode {

	// OBLIGATOIRE : Recuperer le constructeur du parent GameMode 
	//Ne fonctionne pas pour l'instant

	public ExitTrigger exitTrigger;

	//Pendant le chargement
	public override void ModeInitialize(){
		//Specifique a ce gamemode: Trigger de sortie
		ExitTrigger trigger = (ExitTrigger)FindObjectOfType (typeof(ExitTrigger));
		if (trigger != null) 
			exitTrigger = trigger;
	}

	public override void ModePlayingUpdate(){
		//Declancher la fin du niveau quand on active le Exit Trigger 
		if (exitTrigger != null && exitTrigger.activated) {
			gameModeState = GameModeState.Ending;
		}
	}
}
