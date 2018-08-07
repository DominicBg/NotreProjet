using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBomb : TriggerBase, IExplosable {

    public override void Initialize()
    {
        base.Initialize();
    }

    public void Explode(Vector3 zed) {
        TriggerActivables();
    }

    //TIMER - Pendant combien de temps reste-t-il TRIGGERED
}
