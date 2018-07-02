using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bomb Card", menuName = "Bomb Card System/Bomb Card")]
public class BombCard : ScriptableObject {

    public string bombName;

    public string description;

    public Bomb bombGameObject;

    public float explosionForce;

    public int maxBombNumber;

    public enum BombMaterialEnum { StickyBomb, BouncingBomb};
    public BombMaterialEnum bombMaterial;

    public enum BombTriggerEnum { Trigger, Timer};
    public BombTriggerEnum bombTrigger;

    public float triggerTime;

}
