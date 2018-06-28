using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameManager
{
	void Start();
	void UpdateState ();
}

public interface IUIManager
{
	void Enable();
	void Disable();
}

public interface IActivable
{
	void Activate ();
	void Deactivate();
}

public interface IDamageable
{
	void Damage (int damageValue);
}
