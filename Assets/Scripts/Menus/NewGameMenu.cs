using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameMenu : SimpleMenu<NewGameMenu> {

	public override void OnBackPressed(){
		MenuManager.Instance.CloseMenu(this);
	}	
}
