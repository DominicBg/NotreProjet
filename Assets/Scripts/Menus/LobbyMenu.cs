using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMenu : Menu<LobbyMenu> {

    public static void Show(bool multiplayer){
        if(multiplayer){
            Instance.ShowMultiLobby();
        } else {
            Instance.ShowSoloLobby();
        }
    }

    void ShowSoloLobby(){

    }

    void ShowMultiLobby(){

    }

    public override void OnBackPressed(){
		MenuManager.Instance.CloseMenu(this);
	}	
}
