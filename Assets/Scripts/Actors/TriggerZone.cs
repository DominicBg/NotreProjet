using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour {

	public Character[] inGamePlayers;

	[SerializeField]private bool initialized;
	public enum Type{Unique, AutoDesactivation, OnStaying}
	[SerializeField]private Type typeActivation;

	private enum PlayersNum{OnePlayer, MultiplePlayers, AllPlayers}
	[SerializeField]private PlayersNum playersNumberActivation = PlayersNum.OnePlayer;

	public bool activated;

	public GameObject[] activables;

	[SerializeField]private bool[] charactersIn;

	public void Initialize (){
        
		inGamePlayers = GameManager.gameManager.playersManager.charactersPlayedNow;
		charactersIn = new bool[inGamePlayers.Length];
		for(int i = 0; i < charactersIn.Length; i ++){
			charactersIn[i] = false;
            Debug.Log("Trigger zone : "+ this + " found " + inGamePlayers[i].characterName);
		} 
		
	}

	void OnTriggerEnter(Collider other){



		switch (playersNumberActivation) {
		case PlayersNum.OnePlayer:
			//S'active si il n'est pas deja active
			PlayableCharacter charEntering = other.GetComponent<PlayableCharacter> ();
			if (charEntering != null) {

				//Si il n'est pas deja active
				if (!activated) {
					Activate ();			
				}
			}
			break;




			//A REFAIRE POUR QUE ÇA FONCTIONNE A PLUSIEURS

			
		case PlayersNum.AllPlayers:
			//S'active si il n'est pas deja active
			PlayableCharacter charEntering2 = other.GetComponent<PlayableCharacter> ();
			if (charEntering2 != null) {
				charactersIn [charEntering2.playerNumber] = true;

				bool AllIn = true;
				for (int i = 0; i < charactersIn.Length; i++) {
					if (charactersIn [i] == false) {
						AllIn = false;
					}
				} 

				if (AllIn) {
					if (!activated) {
						Activate ();			
					}				
				}
				//Si il n'est pas deja active
			}
			break;
			

		} // FIN DU SWITCH






		/*
		if (other.gameObject.tag == "Player") {
			activated = true;

			foreach (GameObject zzz in activables) {
				IActivable acti = zzz.GetComponent<IActivable> ();
				if (acti != null) {
					acti.Activate ();
				}
			}
			if (typeActivation == Type.AutoDesactivation)
			Invoke ("AutoDesactivation", 0.2f);			
		}
		*/


	}

	void OnTriggerStay(Collider other){

		/*
		if (typeActivation == Type.OnStaying) {
			if (other.gameObject.tag == "Player") {
				activated = true;

				foreach (GameObject zzz in activables) {
					IActivable acti = zzz.GetComponent<IActivable> ();
					if (acti != null) {
						acti.Activate ();
					}
				}
			}
		}
		*/
	}

	void OnTriggerExit(Collider other){

		switch (playersNumberActivation) {
		case PlayersNum.OnePlayer:
			// Desactiver seulement si il n'y a plus de personnage a l'interieur
			//Enlever le personnage sortant de la liste des perso presents
			PlayableCharacter charExiting = other.GetComponent<PlayableCharacter> ();
			if (charExiting != null) {
				charactersIn [charExiting.playerNumber] = false;
			}

			//Checker si il reste du monde
			bool anyone = false;
			for (int i = 0; i < charactersIn.Length; i++) {
				if (!anyone && charactersIn[i] == true) anyone = true;			
			} 
			if (!anyone) Deactivate ();	

			break;
		case PlayersNum.AllPlayers:
			// Desactiver seulement si il n'y a plus de personnage a l'interieur
			//Enlever le personnage sortant de la liste des perso presents
			PlayableCharacter charExiting2 = other.GetComponent<PlayableCharacter> ();
			if (charExiting2 != null) {
				charactersIn [charExiting2.playerNumber] = false;
			}

			//Desactiver puisqu'il ne sont plus tous dedans
			//Desactiver si il etait active
			if (activated)
				Deactivate ();	
			break;
		}



	}

	public virtual void Activate(){
		//Activer les elements
		activated = true;
		foreach (GameObject zzz in activables) {
			IActivable acti = zzz.GetComponent<IActivable> ();
			if (acti != null) {
				acti.Activate ();
			}
		}
	}

	public virtual void Deactivate(){

		activated = false;
		Debug.Log ("DEACTIVATION");
		foreach (GameObject zzz in activables) {
			IActivable acti = zzz.GetComponent<IActivable> ();
			if (acti != null) {
				acti.Deactivate ();
			}
		}
	}


	public void AutoDesactivation(){
		activated = false;
	}
}

