using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharactersInitialisation {

	private readonly PlayersManager playersManager;
	private SetupScene setupScene;
	public CharactersInitialisation (SetupScene setupSceneFrom){
		playersManager = PlayersManager.playersManager;
		setupScene = setupSceneFrom;
	}

    private LevelData lvlData;
    private Character[] charToSpawn;
    private SpawnPoint[] spawnList;

    public void Initialization(){       
        
        LoadLevelCharacterFromLevelData();

        GetSpawnPoints();

        InstantiateLoadedCharacters();
        
		//Fin de l'initialisation
		setupScene.PlayersInitializationFinished ();
	}

    void GetSpawnPoints()
    {
        spawnList = new SpawnPoint[0];
        spawnList = Object.FindObjectsOfType(typeof(SpawnPoint)) as SpawnPoint[];
    }

    void LoadLevelCharacterFromLevelData()
    {
        lvlData = GameManager.gameManager.levelData;
        charToSpawn = new Character[lvlData.startCharacter.Length];

        for (int i = 0; i < charToSpawn.Length; i++)
        {
            if (lvlData.startCharacter[i] != null)
            {
                string charCardPath = AssetDatabase.GetAssetPath(lvlData.startCharacter[i]);
                CharacterCard charCard = AssetDatabase.LoadAssetAtPath(charCardPath, typeof(CharacterCard)) as CharacterCard;
                charToSpawn[i] = charCard.characterPrefab; 
            }
            else if (lvlData.startCharacter[i] == null)
            {
                Debug.Log("PERSO MANQUANT DANS LEVEL CARD");
                charToSpawn[i] = (Character)Resources.Load("Prefabs/IsoCharacter", typeof(Character));
            }
        }
    }

    void InstantiateLoadedCharacters()
    {
        //Creer les 4 personnages d'origine
        Debug.Log(playersManager.playersNumber);
        Character[] characterToInstantiate = new Character[playersManager.playersNumber];

        for (int i = 0; i < playersManager.playersNumber; i++)
        {
            //Instantiation
            characterToInstantiate[i] = GameObject.Instantiate(charToSpawn[i], Vector3.zero, Quaternion.Euler(Vector3.zero));
            characterToInstantiate[i].characterCard = lvlData.startCharacter[i];


            //Mettre le controleur - A DEPLACER DANS CONTROLLER MANAGER
            IsoCharacterController zzz = characterToInstantiate[i].gameObject.AddComponent(typeof(IsoCharacterController)) as IsoCharacterController;
            zzz.device = playersManager.playersConfig[i].controller;

            //Placer les personnages sur le SpawnPoint
            characterToInstantiate[i].transform.position = spawnList[i].gameObject.transform.position;

            SetInformationFromPlayerCard(characterToInstantiate[i], i);
        }

        playersManager.charactersPlayedNow = new Character[characterToInstantiate.Length];
        playersManager.charactersPlayedNow = characterToInstantiate;
    }


     
	void SetInformationFromPlayerCard(Character character, int i){
		
		string playerCardPath = UsefulPath.playerCardData + "PlayerCard_" + (i+1) + ".asset";
		PlayerCard card = (PlayerCard)AssetDatabase.LoadAssetAtPath (playerCardPath, typeof(PlayerCard));

		SpriteRenderer renderer = character.GetComponentInChildren<SpriteRenderer> ();
		card.playerColor.a = 1f;
		renderer.color = card.playerColor;

		PlayableCharacter playablePart = character.GetComponent<PlayableCharacter> ();
		playablePart.playerNumber = i;
		playablePart.playerColor = card.playerColor;


	}


}
