using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modes;
using ManagerEnums;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

	public static GameManager gameManager;
	public GameManagerMode gameManagerMode;
	public IGameManager actualMode;

	//Donnees dans le manager
	public LevelData levelData;
	public GameMode currentGameMode;

	public PlayersManager playersManager;
	public UIManager uiManager;

	[SerializeField]public LaunchingFrom launchingFrom;
	[SerializeField]public int nbrJoueursTemp;

	void Awake ()
	{
		//Ne pas detruire le chargement en changeant de scene
		DontDestroyOnLoad (gameObject);
		//Empecher que le Manager se duplique
		if (gameManager == null) {
			gameManager = this;
		} else {
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		
		playersManager = PlayersManager.playersManager;
		uiManager = UIManager.uiManager;

		playersManager.playersNumber = nbrJoueursTemp;

		BootingGame ();
	}
	
	// Update is called once per frame
	void Update () {
		actualMode.UpdateState ();
	}

	void ChangerMode(IGameManager newMode){
		actualMode = newMode;
		actualMode.Start ();
	}

	void BootingGame(){
		ChangerMode (new GMBootingGame ());
	}

	public void SceneLoading(string nameScene){
		SceneLoader loader = new SceneLoader (nameScene);
		loader.LoadScene ();
	}

	public void StartGame(){
		ChangerMode (new GMLaunchGameFromStart());
	}

	public void StartLevel(){
		ChangerMode (new GMLoadLevel ());
	}

    public void RestartLevel()
    {
        ChangerMode(new GMLoadLevel());
    }

    public void PlayLevel(){
		ChangerMode (new GMPlayingLevel ());
	}

	public void EndLevel(){
		ChangerMode(new GMEndedGame());
	}

	public void StartGMCoroutine(IEnumerator coroutineMethod){
		StartCoroutine (coroutineMethod);
	}
}



public class GMBootingGame : IGameManager {

	private readonly GameManager gameManager;
	public GMBootingGame (){
		gameManager = GameManager.gameManager;
	}

	public void Start(){
		//Launched from Menu or Level
		switch (gameManager.launchingFrom) {
		case LaunchingFrom.StartMenu:
			gameManager.StartGame ();
			break;
		case LaunchingFrom.ThisLevel:
			gameManager.StartLevel ();
			break;
		}
	}

	public void UpdateState(){
		
	}


}



// GMMAINMENU est en fait le START MENU
public class GMLaunchGameFromStart : IGameManager {

	private readonly GameManager gameManager;
	private SceneLoader sceneLoader;

	public GMLaunchGameFromStart (){
		gameManager = GameManager.gameManager;
	}

	public void Start (){		
		gameManager.SceneLoading ("MainMenuInControl");
	}

	public void UpdateState(){

	}
}





public class GMLoadLevel : IGameManager {
	
	private readonly GameManager gameManager;
	SetupScene setupScene = null;
	public GMLoadLevel (){
		gameManager = GameManager.gameManager;
	}

	public void Start (){
		setupScene = gameManager.gameObject.AddComponent<SetupScene> ();
		setupScene.StartLevelLoading ();
	}

	public void UpdateState(){

	}

	//Utiliser la Coroutine par le gameManager qui est MonoBehaviour
	void CoroutineExemple(){
		gameManager.StartGMCoroutine (CoroutineTest());
	}

	IEnumerator CoroutineTest(){		
		yield return new WaitForSeconds (1);
	}
}




public class GMPlayingLevel : IGameManager {
	
	private readonly GameManager gameManager;


	public GMPlayingLevel (){
		gameManager = GameManager.gameManager;
	}

	public void Start (){
		//Jouer le GAME MODE
		gameManager.currentGameMode.GameStart();
	}

	public void UpdateState(){

		//Jouer le GAME MODE
		gameManager.currentGameMode.UpdateMode();
	}
}




public class GMEndedGame : IGameManager {
	
	private readonly GameManager gameManager;
	private SceneLoader sceneLoader;
	public GMEndedGame (){
		gameManager = GameManager.gameManager;
	}

	public void Start (){
		gameManager.SceneLoading ("EndMenu");
	}

	public void UpdateState(){

	}

	public void ReturnToMainMenu(){
		gameManager.StartGame ();
	}
}