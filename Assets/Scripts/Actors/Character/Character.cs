using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Character : MonoBehaviour, IDamageable, IExplosable {

	public int health;
	public bool isExploding = false;
	public float explosionDuration = 1f;

	public Rigidbody rb;

    public CharacterCard characterCard;

    public string characterName;

    public bool dead;


	// Use this for initialization
	public virtual void Start () {

		rb = this.GetComponent<Rigidbody>();

		health = 1;

        InitializeCharacterCard();
    }

    //ZED

    void InitializeCharacterCard() {        
        
        if (characterCard != null) {
            characterName = characterCard.name;
            IsoCharacterMovements charMovement = this.GetComponent<IsoCharacterMovements>();
            if (charMovement != null){
                charMovement.runningSpeedMax = characterCard.runningSpeed * 1000f;
                charMovement.jumpPower = characterCard.jumpHeight * 150f;
            }
        }
        

    }

	public virtual void Damage(int damageValue){
		health = 0;
	}

	public void Explode(Vector3 posExplosion){
		
	}
}
