using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modes {
	
	public enum GameManagerMode
	{
		MainMenu,
		StartingGame,
		InGame,
		EndedGame
	}

	public enum UIManagerMode
	{
		StartCanvas,
		PlayerDetectionCanvas,
		NewGameCanvas,
		InGameCanvas,
		EndGameCanvas
	}




}

namespace ManagerEnums{

	public enum LaunchingFrom
	{
		ThisLevel,
		StartMenu,
		EndScreen,
		Other
	}
}