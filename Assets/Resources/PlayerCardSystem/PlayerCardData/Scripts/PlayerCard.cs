using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerCard : ScriptableObject {

	public int playerNumber;

	public bool isConnected;

	public string playerName;

	public Color playerColor;

	public string usingCharacter;

	public string usingController;
}
