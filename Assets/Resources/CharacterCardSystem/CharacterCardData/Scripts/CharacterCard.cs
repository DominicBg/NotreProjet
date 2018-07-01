using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Character Card", menuName = "Character Card System/Character Card")]
public class CharacterCard : ScriptableObject {

    public string characterName;
    public string description;

    public Character characterPrefab;

    public Bomb characterBomb;

    public float runningSpeed;
    public float jumpHeight;
}
