using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : SimpleMenu<StartMenu> {

	public Button SoloButton;

	public Button MultiplayerButton;

	public Button OptionsButton;

	public override void OnBackPressed(){
		Application.Quit();
	}
	public void OnSoloButtonPressed(){
		bool multiplayer = false;
		ShowStartMenu(multiplayer);
	}
	public void OnMultiplayerButtonPressed(){
		bool multiplayer = true;
		ShowStartMenu(multiplayer);
	}
	public void OnOptionsButtonPressed(){
		OptionsMenu.Show();
	}

	void ShowStartMenu(bool isMultiplayer){
		Debug.Log("Show");
		LobbyMenu.Show(isMultiplayer);
	}
}
