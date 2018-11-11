using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnZone : Activable {

	// Use this for initialization
	void Start () {
		
	}
	

    public override void Activate(Character[] charToRespawn)
    {


        for(int i = 0; i < charToRespawn.Length; i ++)
        {
            if (charToRespawn[i] != null)
            {
                charToRespawn[i].Respawn();
            }
        }
         

    }

    public override void Activate()
    {
        Debug.Log("ACTIVATE SANS CHAR from : " + this);
    }
}
