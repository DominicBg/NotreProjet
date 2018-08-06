using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : Activable {

	public override void Activate(){
		gameObject.SetActive (false);
	}

	public override void Deactivate(){
		
	}
}
