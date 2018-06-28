using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiActivable : MonoBehaviour, IActivable {

	public bool activated;

	public GameObject[] activateurs;
	public bool[] activateds;
	public GameObject[] activables;

	public float activationTimer;

	// Use this for initialization
	void Start () {
		activated = false;
		activateds = new bool[activateurs.Length];
	}

	// Update is called once per frame
	void Update () {

	}

	//Prevenir quand un objet s'active
	public void Activate(){

		SetActivateds();
		CheckForAllActivateds ();
		//Desactiver les activateds actives par bombes
		//Les objets actives par detecteur se desactivent tout seul
		Invoke ("ResetActivation", activationTimer);
	}

	//Prevenir quand un objet se desactive
	public void Deactivate(){
		for (int i = 0; i < activateurs.Length; i++) {
			TriggerZone detectActuel = activateurs [i].GetComponent<TriggerZone> ();
			if (detectActuel != null) {
				activateds [i] = false;
			}
		}
	}

	public void SetActivateds(){

		//Actualiser la liste d'activables a chaque fois qu'on active un objet du multirupteur
		for (int i = 0; i < activateurs.Length; i++) {
			Bombrupteur bombActuel = activateurs [i].GetComponent<Bombrupteur> ();
			Debug.Log ("YOYOYO" + i);
			if (bombActuel != null) {
				activateds [i] = bombActuel.activated;
			}

			TriggerZone detectActuel = activateurs [i].GetComponent<TriggerZone> ();
			if (detectActuel != null) {
				activateds [i] = detectActuel.activated;
			}

		}

	}

	public void CheckForAllActivateds(){

		/*
		int activatedsNow = 0;

		foreach (bool maybeActivated in activateds) {
			if (maybeActivated == true)
				activatedsNow++;		
		}

		if (activatedsNow == activateds.Length) {
			ActiveActivables ();
		}
		*/


		bool allActivated = true;
		for (int i = 0; i < activateds.Length; i++) {
			if (allActivated && activateds[i] == false) allActivated = false;			
		} 
		if (allActivated) ActiveActivables ();	




	}

	public void ActiveActivables(){
		activated = true;
		foreach (GameObject zzz in activables) {
			IActivable acti = zzz.GetComponent<IActivable> ();
			if (acti != null) acti.Activate ();			
		}
		activated = false;
	}

	public void ResetActivation(){
		for (int i = 0; i < activateurs.Length; i++) {
			Bombrupteur bombActuel = activateurs [i].GetComponent<Bombrupteur> ();

			if (bombActuel != null) {
				activateds [i] = false;
			}
		}

	}



}
