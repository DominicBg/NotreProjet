using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Bomb Card List", menuName = "Bomb Card System/Bomb Card List")]
public class BombCardList : ScriptableObject {

    public BombCard[] bombCardList = new BombCard[0];

}
