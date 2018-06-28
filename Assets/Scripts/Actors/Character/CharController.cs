using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class CharController : MonoBehaviour {
	
	//Stick de gauche
	[HideInInspector]public string hautBas; //Deplacements
	[HideInInspector]public string gaucheDroite;
	//Stick de droite
	[HideInInspector]public string droitHautBas; //Deplacements
	[HideInInspector]public string droitGaucheDroite;
	//Boutons Face
	[HideInInspector]public string boutonA; //Saut
	[HideInInspector]public string boutonB; 
	[HideInInspector]public string boutonX; 
	[HideInInspector]public string boutonY; 
	//Boutons LB ET RB
	[HideInInspector]public string boutonLB; 
	[HideInInspector]public string boutonRB; 
	//Gachettes Trigger LT et RT
	[HideInInspector]public string triggerLT; 
	[HideInInspector]public string triggerRT; 
	// ^ VIRER TOUT CA

	public InputDevice device;

	public void Initialize(int playerNumber){
		InputDevice[] activeDevices = PlayersManager.playersManager.activeDevices;
		//hautBas = activeDevices [playerNumber].LeftStickY.Value; 
	}

}
