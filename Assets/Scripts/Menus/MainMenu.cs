using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : SimpleMenu<MainMenu> {
	public override void OnBackPressed(){
		MenuManager.Instance.CloseMenu(this);
	}	
}
