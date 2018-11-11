using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Character Card", menuName = "Character Card System/Character Card")]
public class CharacterCard : ScriptableObject {

    public string characterName;
    public string description;

    public Character characterPrefab;

    public Bomb characterBomb;
    public BombCard characterBombCard;
    public float runningSpeed;
    public float jumpHeight;
}
