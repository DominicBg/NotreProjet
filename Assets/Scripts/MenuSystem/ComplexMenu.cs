using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Menus that need parameters can declar their own show and hide methods
public class ComplexMenu : Menu<ComplexMenu> {

	public static void Show(string foo){
		Open();
		//Do something special
	}

	public static void Hide(int result){
		//gr8
		Close();
	}
}
