using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MultiActivable : Activable {

    //Nouveau
    TriggerBase triggerPart;

    //Tableau des triggers relies 
    public List<TriggerBase> linkedTriggersList;
    public bool[] triggeredTriggers;



    void Start()
    {
        GetLinkedTriggers();
        triggerPart = GetComponent<TriggerBase>();
    }





    //Actualiser le tableau des elements relies
    //Aller chercher les triggers qui ont la partie "Activable" de cet objet
    //Pouvoir le faire dans l'editeur et en jeu
    void GetLinkedTriggers()
    {
        linkedTriggersList = new List<TriggerBase>();

        //Aller chercher les triggers
        TriggerBase[] linkedTriggers = FindObjectsOfType<TriggerBase>();
        Debug.Log(this +" From " + this.transform.parent +" linked Triggers : " + linkedTriggers.Length); 

        //Parcourir les triggers
        for(int i = 0; i < linkedTriggers.Length; i++)
        {
            Debug.Log("Trigger " + i + " " + linkedTriggers[i].name + " From " + linkedTriggers[i].transform.parent);            
            
            //Regarder ce qu'ils visent
            for (int j = 0; j < linkedTriggers[i].objectsToActive.Length; j ++)
            {
                Debug.Log("---- " + i + " Linked to Activable : " + linkedTriggers[i].objectsToActive[j].name + " From " + linkedTriggers[i].objectsToActive[j].transform.parent);

                //Si ils visent cet activateur
                if (this.gameObject == linkedTriggers[i].objectsToActive[j])
                {
                    Debug.Log("SAME OBJECT");
                    linkedTriggersList.Add(linkedTriggers[i]);
                }
            }
        }
    }

    //Quand un des triggers s'active
    public override void Activate()
    {
        bool allTriggered = true;
        //Checker si tout les objets sont actives
        for(int i = 0; i < linkedTriggersList.Count; i++)
        {
            Debug.Log("DIS " + linkedTriggersList[i] +" "+ linkedTriggersList[i].triggered);
            
            //Retenir si un des trigger n'est pas TRIGGERED
            if (!linkedTriggersList[i].triggered)
            {
                allTriggered = false;
            }
        }

        //Si tout est actif
        if (allTriggered)
        {
            triggerPart.TriggerActivables();
        }


    }








    /*

    // VIEUX CODE
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


		bool allActivated = true;
		for (int i = 0; i < activateds.Length; i++) {
			if (allActivated && activateds[i] == false) allActivated = false;			
		} 
		if (allActivated) ActiveActivables ();	

        

	}

	public void ActiveActivablesZZZ(){
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

    
    */
}
