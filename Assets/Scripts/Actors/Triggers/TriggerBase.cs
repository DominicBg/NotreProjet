using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBase : MonoBehaviour {

    public bool triggered;
    public GameObject[] objectsToActive;
    public IActivable[] activables;

    public virtual void Initialize()
    {
        IActivable[] temp = new IActivable[objectsToActive.Length];
        for(int i = 0; i < temp.Length; i++)
        {
            IActivable tempz = objectsToActive[i].GetComponent<IActivable>();
            if (tempz != null)
            {
                temp[i] = tempz;
                Debug.Log("OBJET AJOUTE : " + tempz);
            }
        }
        activables = temp;
    }

    public virtual void TriggerActivables()
    {
        //Activer les elements
        triggered = true;
        Debug.Log("Trigger Activation");
        foreach (IActivable zzz in activables)
        {         
            zzz.Activate();            
        }
    }

    public virtual void TriggerDesactivables()
    {
        triggered = false;
        Debug.Log("Trigger Desactivation");
        foreach (IActivable zzz in activables)
        {
            zzz.Deactivate();            
        }
    }

    public virtual void TriggerActivables(Character[] charTriggered)
    {
        //Activer les elements
        triggered = true;
        Debug.Log("Trigger Activation with Char");
        foreach (IActivable zzz in activables)
        {
            zzz.Activate(charTriggered);
        }
    }

    public virtual void TriggerDesactivables(Character[] charTriggered)
    {
        triggered = false;
        Debug.Log("Trigger Desactivation with Char");
        foreach (IActivable zzz in activables)
        {
            zzz.Deactivate(charTriggered);
        }
    }
}
