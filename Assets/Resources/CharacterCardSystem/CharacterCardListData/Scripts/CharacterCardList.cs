using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Character Card List", menuName = "Character Card System/Character Card List")]
public class CharacterCardList : ScriptableObject {
    
    public CharacterCard[] characterCardList = new CharacterCard[0];
    
}
