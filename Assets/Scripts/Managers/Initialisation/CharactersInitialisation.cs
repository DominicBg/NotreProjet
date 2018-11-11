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

        GetSpawnPoints();

        LoadLevelCharacterFromLevelData();
        
        InstantiateLoadedCharacters();
        
		//Fin de l'initialisation
		setupScene.PlayersInitializationFinished ();
	}

    void GetSpawnPoints()
    {
        spawnList = new SpawnPoint[0];
        spawnList = Object.FindObjectsOfType(typeof(SpawnPoint)) as SpawnPoint[];
    }

    //Set an array of needed Characters prefab from characterCards from Level Data 
    void LoadLevelCharacterFromLevelData()
    {

        //Register Characters prefab. 
        //In case of different Characters.
        charToSpawn = new Character[GameManager.gameManager.levelData.startCharacter.Length];        

        for (int i = 0; i < charToSpawn.Length; i++)
        {
            if (GameManager.gameManager.levelData.startCharacter[i] != null)
            {
                //Set Character prefab from Character Card
                string charCardPath = AssetDatabase.GetAssetPath(GameManager.gameManager.levelData.startCharacter[i]);
                CharacterCard charCard = AssetDatabase.LoadAssetAtPath(charCardPath, typeof(CharacterCard)) as CharacterCard;
                charToSpawn[i] = charCard.characterPrefab;
            }
            else if (GameManager.gameManager.levelData.startCharacter[i] == null)
            {
                Debug.Log("PERSO MANQUANT DANS LEVEL CARD");
                charToSpawn[i] = (Character)Resources.Load("Prefabs/IsoCharacter", typeof(Character));
            }
        }
    }


    //Instantiate Characters prefab from previously made array
    void InstantiateLoadedCharacters()
    {
        //Creer les 4 personnages d'origine
        Character[] characterToInstantiate = new Character[playersManager.playersNumber];

        for (int i = 0; i < playersManager.playersNumber; i++)
        {
            //Instantiation
            characterToInstantiate[i] = GameObject.Instantiate(charToSpawn[i], Vector3.zero, Quaternion.Euler(Vector3.zero));

            PlayerConfig[] playerConfigs = playersManager.playersConfig;

            //Mettre le controleur - A DEPLACER DANS CONTROLLER MANAGER
            IsoCharacterController isoCharControler = playerConfigs[i].gameObject.AddComponent(typeof(IsoCharacterController)) as IsoCharacterController;
            isoCharControler.device = playerConfigs[i].controller;
            isoCharControler.characterMovements = characterToInstantiate[i].GetComponent<IsoCharacterMovements>();
            isoCharControler.weapon = characterToInstantiate[i].GetComponentInChildren<Weapon>();
            

            //Placer les personnages sur le SpawnPoint
            characterToInstantiate[i].transform.position = spawnList[i].gameObject.transform.position;
            characterToInstantiate[i].respawnPlace = characterToInstantiate[i].transform.position;
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
