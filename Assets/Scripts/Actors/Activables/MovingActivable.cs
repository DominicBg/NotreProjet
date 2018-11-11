using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingActivable : Activable {


	private enum Etat{AttenteAller, Aller, AttenteRetour, Retour};
	private Etat etatActuel;

	public enum Mode{AllerSimple, AllerRetour, Boucle}
	public Mode modeActuel;

	private enum Comportement{SoloActivation, StayActivated}
	private Comportement comportementActuel;

	public bool reactivable;
	public bool desactivable;

	public enum DesactivationEffect{BackToStartPosition, StopHereAndWait, GoToLastPosition}
	public DesactivationEffect desactivationEffect;
    
	private Vector3 positionStart;
	public Vector3 offset;
	private Vector3 positionCible;
	public float speed;
	public float speedReturn;
	public float timer; 
	private bool timerActivated;

	public GameObject movingObject;

	// Use this for initialization
	void Start () {
		positionStart = movingObject.transform.localPosition;
		positionCible = movingObject.transform.localPosition + offset;
		etatActuel = Etat.AttenteAller;
	}

	// Update is called once per frame
	void FixedUpdate () {
		switch (modeActuel) {
		case Mode.AllerSimple:
			AllerSimple ();
			break;
		case Mode.AllerRetour:
			AllerRetour ();
			break;
		case Mode.Boucle:
			Boucle ();
			break;
		}
	}

	public void AllerSimple(){

		switch (etatActuel) {
		case Etat.AttenteAller:
			if (activated) {
				etatActuel = Etat.Aller;
				activated = false;
			}
			break;
		case Etat.Aller:
			DeplacementAller ();
			break;
		case Etat.AttenteRetour:
			if (reactivable && activated) {
				etatActuel = Etat.Retour;
				activated = false;
			}
			break;
		case Etat.Retour:
			DeplacementRetour ();
			break;
		}
	}

	public void AllerRetour(){

		switch (etatActuel) {
		case Etat.AttenteAller:
			if (activated) {
				etatActuel = Etat.Aller;
				activated = false;
			}
			break;
		case Etat.Aller:
			DeplacementAller ();
			break;
		case Etat.AttenteRetour:
			if (!timerActivated) {
				Invoke ("StartReturning", timer);
				timerActivated = true;
			}
			break;
		case Etat.Retour:
			DeplacementRetour ();
			break;
		}
	}

	public void Boucle(){

	}



	public void DeplacementAller(){
		if (movingObject.transform.localPosition != positionCible) {
			//float step = speed * Time.deltaTime;
			movingObject.transform.localPosition = Vector3.MoveTowards (movingObject.transform.localPosition, positionCible, speed);
		} else {			
			etatActuel = Etat.AttenteRetour;
		}

	}

	public void StartReturning(){
		etatActuel = Etat.Retour;
		timerActivated = false;
	}

	public void DeplacementRetour(){
		if (movingObject.transform.localPosition != positionStart) {
			//float step = speedReturn * Time.deltaTime;
			movingObject.transform.localPosition = Vector3.MoveTowards (movingObject.transform.localPosition, positionStart, speedReturn);
		} else {
			etatActuel = Etat.AttenteAller;
		}
	}

	public override void Activate() {
		if (etatActuel == Etat.AttenteAller) {
			activated = true;
		}
		if (modeActuel == Mode.AllerSimple && etatActuel == Etat.AttenteRetour) {
			activated = true;
		}
	}

	public override void Deactivate(){
		switch (desactivationEffect) {
		case DesactivationEffect.BackToStartPosition:
			StartReturning ();
			break;
		case DesactivationEffect.GoToLastPosition:
			break;
		case DesactivationEffect.StopHereAndWait:
			break;
		}
	}




}
