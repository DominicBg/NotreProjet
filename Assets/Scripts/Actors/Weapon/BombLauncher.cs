﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BombLauncher : Weapon {

	public GameObject spawner;
	public GameObject cible;

    public BombCard bombCard;

	public Bomb[] bombes;
	public Bomb usedBomb;
	public int nBombesAct;
	public int nBombesMax;

	public float rateOfShooting = 1f;
	public bool canShoot = true;

	public float rateOfPlacing;
	public bool canPlace;

	public float aimAngle;

	//direction de l'arme
	public float launcherDirection;

	public Vector3 autoAimPlace;
	public float shootDistance = 10f;
	public enum AutoAimStates{AimingFace, AimingDown, Searching}
	public AutoAimStates aimingState;
	public IEnumerator searchingDown;
	public float aimingDistance;
	public float leftStickAimDistance; //Valeur du stick de gauche, a utiliser si celui de droite n'est pas utilise
	public float aimingDistanceMax = 0.7f; 
	public float inclinaison;

	public Color charColor;



	// Use this for initialization
	void Start () {
        InitializeBomb();

		//Create Bomb list
		bombes = new Bomb[nBombesMax];

		charColor = GetComponentInParent<PlayableCharacter> ().playerColor;
	}

    void InitializeBomb()
    {
        Character zzz = (Character)GetComponentInParent<Character>();

        if(zzz != null)
        {
            string assetPath = AssetDatabase.GetAssetPath(zzz.characterCard);
            CharacterCard charCard = AssetDatabase.LoadAssetAtPath(assetPath, typeof(CharacterCard)) as CharacterCard;

            assetPath = AssetDatabase.GetAssetPath(charCard.characterBombCard);
            bombCard = AssetDatabase.LoadAssetAtPath(assetPath, typeof(BombCard)) as BombCard;
        }

        //Debug.Log("WEAPON BOMB CARD" + bombCard.name);
        usedBomb = (Bomb)Resources.Load("Prefabs/Bomb", typeof(Bomb));
        
    }
	


	void FixedUpdate(){
		AutoAim ();
	}

	void AutoAim(){	

		//Vecteurs Position de depart, position maximum visee, position au sol 
		//Position et direction de depart
		Vector3 posDepart = spawner.transform.position;
		Vector3 dirDepart = spawner.transform.right;

        //Shootdistance, recuperer la valeur depuis la CharCard
        //Tracer cette ligne
        shootDistance = 8f;
		Debug.DrawRay (posDepart, dirDepart * shootDistance, Color.blue);

        //Position sur l'axe de depart en fonction de shootDistance et magnitude du stick 
        float disActuelle = shootDistance * (aimingDistance / shootDistance);
		if (disActuelle <= 0) 
			disActuelle = 0.1f;
		
		//Postion du ray a envoyer vers le bas
		Vector3 posDowning = posDepart + (dirDepart.normalized * disActuelle);

		//Position du lieu trouve vers le bas 
		Debug.DrawRay(posDowning, Vector3.down * 10f, Color.cyan);

		RaycastHit hit;
		if (Physics.Raycast(posDepart, dirDepart, out hit, disActuelle)){		
			//si il y'a une cible a hauteur du mur
			cible.transform.position = hit.point;
		} else {
			//Si il est possible de viser le sol
			if (Physics.Raycast (posDowning, Vector3.down, out hit)) {
				cible.transform.position = hit.point;
			} else {
			//Si il n'est pas possible de viser le sol 
			}
		}

		Debug.DrawLine (posDepart, cible.transform.position,Color.red);	
	}
    

    /*
	public void AimAndUse(Vector3 leftInput, float leftStickDistance, Vector3 aim, float aimDistance, bool explode, bool shoot){

        Debug.Log("AIMANDUSE");

        leftStickAimDistance = leftStickDistance;
		aimingDistance = aimDistance;

		//If Right Stick is used. Else Use Left Stick
		if (aim.magnitude > 0f) {			
			aimAngle = Mathf.RoundToInt (Mathf.Atan2 (aim.x, aim.z) * Mathf.Rad2Deg);
		} else {
			if (leftInput.magnitude > 0f) {
				aimAngle = Mathf.RoundToInt(Mathf.Atan2 (leftInput.x, leftInput.z) * Mathf.Rad2Deg);
			}
		}

		//launcherDirection = aimAngle - 90;

		// -45f is for Camera Rotation, modify this later on Controller
		transform.rotation = Quaternion.Euler (0f, launcherDirection, 0f);



		if (shoot) 
			ShootBomb (cible.transform.position);		

		if (explode)
			ExplodeAllBombs();

	}
    */

	//NOUVEAU
    //Called by IsoCharacterController
    //This code should be a IsoCharacterWeapon
	public override void UseWeapon (Vector2 leftInput, Vector2 rightInput, bool shoot, bool explode){
        
		aimingDistance = rightInput.magnitude * shootDistance;
		//Viser avec le stick de gauche ou celui de droite
		if (rightInput.magnitude > 0f) {
			aimAngle = Mathf.RoundToInt (Mathf.Atan2 (rightInput.x, rightInput.y) * Mathf.Rad2Deg);
		} else if (leftInput.magnitude > 0f) {
            aimingDistance = leftInput.magnitude * shootDistance;
            aimAngle = Mathf.RoundToInt(Mathf.Atan2 (leftInput.x, leftInput.y) * Mathf.Rad2Deg);
		}

		//Modifier pour prendre en compte l'ange de la camera
		launcherDirection = aimAngle + 180 + 45;
		transform.rotation = Quaternion.Euler (0f, launcherDirection, 0f);

		if (shoot) 
			ShootBomb (cible.transform.position);		

		if (explode)
			ExplodeAllBombs();
	}





	void ShootBomb (Vector3 targetPosition){

		if (canShoot) {	
			if (nBombesAct < nBombesMax) {
				Bomb tempBomb = InstantiateNewBomb ();
                tempBomb.bombCard = bombCard;
                Debug.Log(tempBomb.bombCard);
                canShoot = false;
				tempBomb.targetPosition = targetPosition;
				tempBomb.bombColor = charColor;
				StartCoroutine (ResetCanShoot ());			
			} else {
				Debug.Log ("Max de bmombe atteind");
			}
		}
	}


	IEnumerator ResetCanShoot(){		
		yield return new WaitForSeconds (rateOfShooting);
		canShoot = true;
	}


	//Instantiate and add bomb to the list
	Bomb InstantiateNewBomb(){
		//Instantier bomb
		Bomb newBomb = Instantiate(usedBomb,spawner.transform.position,spawner.transform.rotation);
		//Set bomb in list
		bombes [nBombesAct] = newBomb;
		nBombesAct++;

		//Return bomb to modify position
		return newBomb;
	}



	void PlaceBomb(){
		
	}

	void ExplodeOlderBomb(){

		if (nBombesAct > 0) {
			bombes [0].Explode ();
			bombes [0] = null;
			nBombesAct--;
			for (int i = 0; i < bombes.Length; i++) {
				if (bombes [i] != null) {
					bombes [i - 1] = bombes [i];
					bombes [i] = null;
				}
			}
		}
	}

	public void ExplodeAllBombs(){
		if (nBombesAct > 0) {
			nBombesAct = 0;

			for (int i = 0; i < bombes.Length; i++) {
				if (bombes [i] != null) {
					bombes [i].Explode ();
				}
				bombes [i] = null;
			}
		} 
	}
}
