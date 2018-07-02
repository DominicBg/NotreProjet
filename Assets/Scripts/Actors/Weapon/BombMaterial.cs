using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMaterial : MonoBehaviour {

    public Bomb thisBomb;

    public virtual void Start()
    {
        thisBomb = GetComponent<Bomb>();
    }
}

public class StickyBomb : BombMaterial
{

    public bool isSticked = false;

    public override void Start()
    {
        base.Start();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isSticked)
        {
            thisBomb.rb.velocity = Vector3.zero;
            if (collision.rigidbody)
            {
                HingeJoint hinge = gameObject.AddComponent<HingeJoint>();
                hinge.connectedBody = collision.rigidbody;
                hinge.axis = Vector3.forward;
                hinge.anchor = Vector3.zero;
                hinge.useLimits = true;
            }
            isSticked = true;
        }
        

    }
}

public class BouncingBomb : BombMaterial
{
    public override void Start()
    {
        base.Start();

        Collider bombCollider = thisBomb.GetComponent<Collider>();
        bombCollider.material = Resources.Load("PhysicsMaterials/Bouncing") as PhysicMaterial;
    }


}


