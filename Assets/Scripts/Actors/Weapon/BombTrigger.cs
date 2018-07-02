using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTrigger : MonoBehaviour {

    Bomb triggeredBomb;

    public virtual void Start()
    {
        triggeredBomb = GetComponent<Bomb>();
    }

    public void Explode()
    {
        triggeredBomb.Explode();
    }
}

public class ManualTrigger : BombTrigger
{


}

public class TimerTrigger : BombTrigger
{
    public float timer = 1f;
    
    public override void Start()
    {
        base.Start();

        Debug.Log("Time Trigger Start");

        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        Debug.Log("Time Trigger COroutine Start");
        yield return new WaitForSeconds(timer);
        Explode();
    }
}


