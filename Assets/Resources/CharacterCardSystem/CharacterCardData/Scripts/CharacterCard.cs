using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Character Card", menuName = "CharacterCard")]
public class CharacterCard : ScriptableObject {

    public new string characterName;
    public string description;

    public Character characterPrefab;

    public Bomb characterBomb;

    public float runningSpeed;
    public float jumpHeight;
}
