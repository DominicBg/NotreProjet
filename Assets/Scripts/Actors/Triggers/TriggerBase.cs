using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBase : MonoBehaviour {

    public bool triggered;
    public bool staysTriggered;
    public float disablingDelay;
    public GameObject[] objectsToActive;
    public IActivable[] activables;

    IEnumerator resetTrigger;

    public virtual void Initialize()
    {
        //Si il y a des objets dans le tableau
        if(objectsToActive != null)
        {
            IActivable[] temp = new IActivable[objectsToActive.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                IActivable tempz = objectsToActive[i].GetComponent<IActivable>();
                if (tempz != null)
                {
                    temp[i] = tempz;
                }
            }
            activables = temp;
        } else if (objectsToActive == null || objectsToActive.Length == 0)
        {
            Debug.Log(this + " Tableau d'objet a activer vide!");
        }


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

    IEnumerator DelayBeforeDisable()
    {
        Debug.Log("Delay before disable Start : " + Time.time);

        if(disablingDelay != 0f)
        {
            yield return new WaitForSeconds(disablingDelay);
            TriggerDesactivables();
        }
        else
        {
            yield return null;
        }


    }



}
