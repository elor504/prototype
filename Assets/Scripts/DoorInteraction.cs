using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : ObjectInteraction
{
	public bool hasKey;
	public override void Awake()
	{
		base.Awake();
		hasKey = false;
	}

	public void InitDoor()
	{
		hasKey = false;
	}

	public override void OnInteraction()
	{
		if (hasKey)
		{
			GameManager.getInstance.WinScreen();
			//win condition
		}
		else
		{
			GameManager.getInstance.LoseScreen();

		}
	}


	public override void OnTriggerEnter2D(Collider2D collision)
	{
		base.OnTriggerEnter2D(collision);
	}
}
