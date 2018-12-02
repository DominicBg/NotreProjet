using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreenMenu : SimpleMenu<SplashScreenMenu> {

	public Button startButton;		

	public override void OnBackPressed(){
		MenuManager.Instance.CloseMenu(this);
	}
	public override void OnAnyButtonPressed(){
		ShowStartMenu();
	}

	public void OnStartButtonPressed(){
		ShowStartMenu();
	}

	void ShowStartMenu(){
		StartMenu.Show();
	}





}
