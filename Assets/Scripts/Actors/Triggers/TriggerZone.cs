using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Trigger qui s'active par des collisions avec les joueurs
public class TriggerZone : TriggerBase {

	public Character[] inGamePlayers;

	[SerializeField]private bool initialized;
	public enum Type{Unique, AutoDesactivation, OnStaying}
	[SerializeField]private Type typeActivation;

	private enum PlayersNum{OnePlayer, MultiplePlayers, AllPlayers}
	[SerializeField]private PlayersNum playersNumberActivation = PlayersNum.OnePlayer;
    
	public bool[] charactersIn;
    
    public override void Initialize()
    {
        Debug.Log("Start");
        base.Initialize();

        inGamePlayers = GameManager.gameManager.playersManager.charactersPlayedNow;
        charactersIn = new bool[inGamePlayers.Length];
        for (int i = 0; i < charactersIn.Length; i++)
        {
            charactersIn[i] = false;
            //Debug.Log("Trigger zone : "+ this + " found " + inGamePlayers[i].characterName);
        }
    }

	void OnTriggerEnter(Collider other){

        //Activer les bool dans le tableau quand un joueur entre
        Character charEntering = other.GetComponent<Character>();
        if (charEntering != null)
        {
            for (int i = 0; i < charactersIn.Length; i++)
            {
                if (charEntering == inGamePlayers[i])
                {
                    Debug.Log("CHAR NUMBER : " + i);
                    charactersIn[i] = true;
                }
            }
        }

        switch (playersNumberActivation) {
	    	case PlayersNum.OnePlayer:
                //S'active directement si il n'est pas deja active
                if (!triggered)
                {
                    //TriggerActivables();
                    GetCharactersAndTrigger();
                }
                break;                
		    case PlayersNum.AllPlayers:
                //Checker si tout les joueurs sont dedans
				bool AllIn = true;
				for (int i = 0; i < charactersIn.Length; i++) {
					if (charactersIn [i] == false) {
						AllIn = false;
					}
				}

                //Si il n'est pas deja active
                if (AllIn) {
					if (!triggered) {
                        //TriggerActivables ();	
                        GetCharactersAndTrigger();
                    }
                }			
			    break;		
		} 
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
			if (!anyone) TriggerDesactivables ();	
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
			if (triggered)
				TriggerDesactivables ();	
			break;
		}
	}

    void GetCharactersAndTrigger()
    {
        bool triggerEmpty = true;
        Character[] charToSend = new Character[inGamePlayers.Length];
        for (int i = 0; i < inGamePlayers.Length; i++)
        {
            if (charactersIn[i])
            {
                charToSend[i] = inGamePlayers[i];
                triggerEmpty = false;
            }
        }
        if (!triggerEmpty)
        {
            TriggerActivables(charToSend);
        }

    }



	public void AutoDesactivation(){
		triggered = false;
	}
}

