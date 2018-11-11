using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public BombCard bombCard;
    public BombTrigger bombTrigger;
    public BombMaterial bombMaterial;

    public Rigidbody rb;

	public float speed; 
	public Vector3 targetPosition;
	public float totaleDistanceToTarget;
	public float actualDistanceToTarget;
	public bool isSticked = false;
	public bool isZoned = false;
	public float expRadius = 100f;

	public Debris debris;
	public int debrisMax;

	public BombZone zone;

	public float expForce = 800f;

	public Color bombColor;

	public Renderer rendererB;

	// Use this for initialization
	void Start () {

		zone = (BombZone)Resources.Load ("Prefabs/BombZone", typeof(BombZone));
	
		totaleDistanceToTarget = Vector3.Distance (transform.position, targetPosition);
        
        //Truc pour changer la couleur des bombes, a ranger ailleurs
		rendererB = GetComponent<Renderer> ();
		rendererB.material.SetColor ("_Color", bombColor);
		Color bombColor2 = bombColor;
		bombColor2.a = 0.5f;
		rendererB.material.SetColor ("_OutlineColor", bombColor2);

        InitializeBombCard();

        rb.AddForce((targetPosition - transform.position).normalized * 2000, ForceMode.Acceleration);
        
    }

    void InitializeBombCard()
    {
        expForce = bombCard.explosionForce * 800f;

        switch (bombCard.bombMaterial)
        {
            case BombCard.BombMaterialEnum.BouncingBomb:
                bombMaterial = gameObject.AddComponent<BouncingBomb>();
                break;
            case BombCard.BombMaterialEnum.StickyBomb:
                bombMaterial = gameObject.AddComponent<StickyBomb>();
                break;
        }

        switch (bombCard.bombTrigger)
        {
            case BombCard.BombTriggerEnum.Trigger:
                bombTrigger = gameObject.AddComponent<ManualTrigger>();
                break;
            case BombCard.BombTriggerEnum.Timer:
                bombTrigger = gameObject.AddComponent<TimerTrigger>();
                TimerTrigger time = bombTrigger.GetComponent<TimerTrigger>();
                time.timer = bombCard.triggerTime;
                break;
        }
    }
	

	//Au declanchement de L'explosion
	public void Explode(){

		CreateDebris ();
		Vector3 posExplosion = transform.position;
		Collider[] colliders = Physics.OverlapSphere (posExplosion, expRadius);

		foreach (Collider hit in colliders) {
			IExplosable explosable = hit.GetComponent<IExplosable> ();

			//Si l'objet a l'interface IExplosable
			if (explosable != null) {				
				Rigidbody rb = hit.GetComponent<Rigidbody> ();
				//rb.AddExplosionForce (expForce, posExplosion, 12f, 10f);
                //Debug.Log ("EXPLOSABLE");
                explosable.Explode(expForce, posExplosion);
			}
		}

		Destroy (gameObject);
	}

	void CreateDebris(){
		for (int i = 0; i < debrisMax; i++) {
			Debris zzz = Instantiate (debris, transform.position + new Vector3 (Random.Range(-0.5f,0.5f), Random.Range(0,0.5f), Random.Range(-0.5f,0.5f)), transform.rotation);
			zzz.scale = Random.Range (0.1f, 4f);
		}
	}

	void MakeCameraShake(){
	}

    public void DisplayZone() {
        BombZone bomb = Instantiate(zone, transform.position, transform.rotation);

        Renderer rendererZ = bomb.GetComponent<Renderer>();
        Color bombZoneColor = bombColor;
        bombZoneColor.a = 0.25f;
        rendererZ.material.SetColor("_TintColor", bombZoneColor);

        bomb.transform.parent = gameObject.transform;
		bomb.transform.localScale = Vector3.one * (expRadius * 2);
		isZoned = true;
	}


}
