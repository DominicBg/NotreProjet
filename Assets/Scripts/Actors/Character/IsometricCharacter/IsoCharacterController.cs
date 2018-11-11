using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class IsoCharacterController : CharController {

	public IsoCharacterActions characterActions;
	public IsoCharacterMovements characterMovements;

	public Weapon weapon;

	//Camera Angles
	Vector3 cameraForward = new Vector3(-0.5f, 0f, 0.5f);
	public Vector3 cameraRight;

	// Use this for initialization
	void Start () {
		//Debug.Log (device);
		characterActions = new IsoCharacterActions (device);

        //Ces components doivent venir de PlayerConfig
		//characterMovements = GetComponent<IsoCharacterMovements> ();
        //weapon = GetComponentInChildren<Weapon> ();

		characterActions.Left.AddDefaultBinding (Key.LeftArrow);
		characterActions.Left.AddDefaultBinding (InputControlType.DPadLeft);
		characterActions.Left.AddDefaultBinding( InputControlType.LeftStickLeft );

		characterActions.Right.AddDefaultBinding (Key.RightArrow);
		characterActions.Right.AddDefaultBinding (InputControlType.DPadRight);
		characterActions.Right.AddDefaultBinding( InputControlType.LeftStickRight );

		characterActions.Down.AddDefaultBinding (Key.DownArrow);
		characterActions.Down.AddDefaultBinding (InputControlType.DPadDown);
		characterActions.Down.AddDefaultBinding( InputControlType.LeftStickDown );

		characterActions.Up.AddDefaultBinding (Key.UpArrow);
		characterActions.Up.AddDefaultBinding (InputControlType.DPadUp);
		characterActions.Up.AddDefaultBinding( InputControlType.LeftStickUp );

		characterActions.Left2.AddDefaultBinding( InputControlType.RightStickLeft );
		characterActions.Right2.AddDefaultBinding( InputControlType.RightStickRight );
		characterActions.Down2.AddDefaultBinding( InputControlType.RightStickDown );
		characterActions.Up2.AddDefaultBinding( InputControlType.RightStickUp );

		characterActions.Jump.AddDefaultBinding (Key.Space);
		characterActions.Jump.AddDefaultBinding (InputControlType.Action1);

        characterActions.Cancel.AddDefaultBinding(Key.B);
        characterActions.Cancel.AddDefaultBinding(InputControlType.Action2);

        characterActions.Trigger1.AddDefaultBinding (InputControlType.LeftTrigger);
		characterActions.Trigger2.AddDefaultBinding (InputControlType.RightTrigger);

		cameraRight = Quaternion.Euler (new Vector3 (0, 90, 0)) * cameraForward;

	}


	void FixedUpdate(){

        UseCharacter();
    }

    void UseCharacter()
    {
        Vector2 modifiedMove = Quaternion.Euler(0, 0, 45) * characterActions.Move;
        //modifiedMove.y = modifiedMove.y / 2;
        if (characterMovements != null)
        {
            characterMovements.Move(modifiedMove, characterActions.Jump, characterActions.Cancel);

        }
        else
        {
            
        }

        if (weapon != null)
        {
            weapon.UseWeapon(characterActions.Move, characterActions.Aim, characterActions.Trigger2, characterActions.Trigger1);

        }



    }

    // Update is called once per frame
    void Update () {
		
	}
}
