using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activable : MonoBehaviour, IActivable {

	public bool activated; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public virtual void Activate(){
	
	}

	public virtual void Deactivate(){
	
	}

    public virtual void Activate(Character[] applyToTheseCharacters)
    {
        //Declancer les elements qui s'activent sans la liste de char
        Activate();
    }

    public virtual void Deactivate(Character[] applyToTheseCharacters)
    {
        //Declancer les elements qui s'activent sans la liste de char
        Deactivate();
    }
}
