using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modes;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LevelData : ScriptableObject {

	public GameModeData gameMode;
	public string gameModeName;

	public Scene scene;
	public string mapName;

	public int levelMinPlayers;
	public int levelMaxPlayers;

	public Sprite imageLevel;
    public Texture textureLevel;
	public string levelDescription;

    public CharacterCard[] startCharacter;
}
