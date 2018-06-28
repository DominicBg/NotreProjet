using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour, IActivable {

	public void Activate(){
		gameObject.SetActive (false);
	}

	public void Deactivate(){
		
	}
}
