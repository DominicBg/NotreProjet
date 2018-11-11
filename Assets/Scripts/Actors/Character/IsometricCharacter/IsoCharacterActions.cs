using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class IsoCharacterActions : PlayerActionSet {

	public PlayerAction Left;
	public PlayerAction Right;
	public PlayerAction Up;
	public PlayerAction Down;

	public PlayerAction Jump;
    public PlayerAction Cancel;

	public PlayerTwoAxisAction Move;

	public PlayerAction Trigger1;
	public PlayerAction Trigger2;

	public PlayerAction Left2;
	public PlayerAction Right2;
	public PlayerAction Up2;
	public PlayerAction Down2;

	public PlayerTwoAxisAction Aim;

	public IsoCharacterActions (){
		Left = CreatePlayerAction ("Move Left");
		Right = CreatePlayerAction ("Move Right");
		Up = CreatePlayerAction ("Move Up");
		Down = CreatePlayerAction ("Move Down");

        Jump = CreatePlayerAction ("Jump");
        Cancel = CreatePlayerAction("Cancel");

		Left2 = CreatePlayerAction ("Aim Left");
		Right2 = CreatePlayerAction ("Aim Right");
		Up2 = CreatePlayerAction ("Aim Up");
		Down2 = CreatePlayerAction ("Aim Down");

		Move = CreateTwoAxisPlayerAction (Left, Right, Down, Up);
		Aim = CreateTwoAxisPlayerAction (Left2, Right2, Down2, Up2);

		Trigger1 = CreatePlayerAction ("Left Trigger");
		Trigger2 = CreatePlayerAction ("Right Trigger");
	}

	public IsoCharacterActions (InputDevice device){

		this.Device = device;

		Left = CreatePlayerAction ("Move Left");
		Right = CreatePlayerAction ("Move Right");
		Up = CreatePlayerAction ("Move Up");
		Down = CreatePlayerAction ("Move Down");

		Left2 = CreatePlayerAction ("Aim Left");
		Right2 = CreatePlayerAction ("Aim Right");
		Up2 = CreatePlayerAction ("Aim Up");
		Down2 = CreatePlayerAction ("Aim Down");

		Jump = CreatePlayerAction ("Jump");
        Cancel = CreatePlayerAction("Cancel");

        Move = CreateTwoAxisPlayerAction (Left, Right, Down, Up);
		Aim = CreateTwoAxisPlayerAction (Left2, Right2, Down2, Up2);
		Trigger1 = CreatePlayerAction ("Left Trigger");
		Trigger2 = CreatePlayerAction ("Right Trigger");


	}
}
