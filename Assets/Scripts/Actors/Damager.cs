using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour {

	public int damageValue;

	public void OnTriggerEnter(Collider other){

		IDamageable zzz = other.gameObject.GetComponent<IDamageable> ();
		if (zzz != null) {
			zzz.Damage (damageValue);
		}
	}
}
