using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : SimpleMenu<OptionsMenu> {
	public override void OnBackPressed(){
		MenuManager.Instance.CloseMenu(this);
	}	
}
