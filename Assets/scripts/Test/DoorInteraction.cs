using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : ObjectInteraction
{
	public bool hasKey;
	private Animator anim;
	public override void Awake()
	{
		anim = GetComponent<Animator>();
		base.Awake();
		hasKey = false;
		anim.SetBool("IsOpen", hasKey);
	}

	public void InitDoor()
	{
		hasKey = false;
		anim.SetBool("IsOpen", hasKey);
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
	public void OnGettingKey()
	{
		hasKey = true;
		anim.SetBool("IsOpen", hasKey);
	}

	public override void OnTriggerEnter2D(Collider2D collision)
	{
		base.OnTriggerEnter2D(collision);
	}
}
