using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for simple menus that not need any parameters
public abstract class SimpleMenu<T> : Menu<T> where T : SimpleMenu<T> {

	public static void Show(){
		Open();
	}

	public static void Hide(){
		Close();
	}
}
