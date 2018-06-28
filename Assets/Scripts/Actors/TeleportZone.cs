using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportZone : TriggerZone {

	//public TeleportPoint[] teleportPoints;

	public TeleportZone teleportationZone;

	public bool isTeleportedIn = false;




	public override void Activate(){
		activated = true;

		Debug.Log ("TELEPORT - ZONE CHANGING");
		/*
		if (teleportPoints != null) {
			if (inGamePlayers.Length <= teleportPoints.Length) {
				//Deplacer chaque joueur sur le nouveau spawnPoint
				for (int i = 0; i < inGamePlayers.Length; i++) {
					inGamePlayers [i].transform.position = teleportPoints [i].transform.position;
				}
			} else {
				Debug.Log ("TeleportZone Error : Pas assez de TeleportPoints");			
			}
		} else {
			Debug.Log ("TeleportZone Error : pas de TeleportPoints");		
		}
		*/

		if (!isTeleportedIn) { //Si les joueurs ne viennent pas d'etre TP a ce point		
			if (teleportationZone != null) {

				//Pour que les joueurs ne soient pas reteleportes directement 
				teleportationZone.isTeleportedIn = true;

				//Deplacer chaque joueur sur le nouveau spawnPoint
				for (int i = 0; i < inGamePlayers.Length; i++) {
					inGamePlayers [i].transform.position = teleportationZone.transform.position;
				}
			} else {
				Debug.Log ("Pas de Teleporteur relie");
			}
		}







	}

	public override void Deactivate(){
		
		activated = false;

		Debug.Log ("TELEPORTZONE DEACTIVATION");

		//Desactiver isTeleportedIn quand les joueurs sortent du TP 
		isTeleportedIn = false;

	}




}
