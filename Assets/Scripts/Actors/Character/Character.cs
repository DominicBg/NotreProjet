using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Character : MonoBehaviour, IDamageable, IExplosable {

	public int health;
	public bool isExploding = false;
	public float explosionDuration = 1f;

	public Rigidbody rb;

    public CharacterCard characterCard;

    public string characterName;

    public bool dead;

    public Vector3 respawnPlace;


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

	public virtual void Explode(float expForce, Vector3 posExplosion){
        rb.velocity = Vector3.zero;

        IEnumerator coroutine = PauseOnExplosion(expForce, posExplosion);

        StartCoroutine(coroutine);
    }

    IEnumerator PauseOnExplosion(float expForce, Vector3 posExplosion)
    {       
        yield return new WaitForSeconds(0.1f);

        rb.AddExplosionForce(expForce, posExplosion, 12f, 10f);
    }

    public virtual void Respawn()
    {
        rb.velocity = Vector3.down;
        if( respawnPlace != null)
        {
            transform.position = respawnPlace;
        }
        else
        {
            Debug.Log("No Respawn place in : " + this.name);
        }
    }
}
