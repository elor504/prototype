using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager _instance;
	public static GameManager getInstance => _instance;

	public List<grip> grips;
	public List<PickUpInteraction> pickUps;
	public List<DoorInteraction> doors;

	private void Awake()
	{
		if (_instance == null)
			_instance = this;
		else if (_instance != this)
			Destroy(this.gameObject);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.D))
		{
			ResetGame();
		}
	}

	public void WinScreen()
	{
		Debug.Log("Win!");
		ResetGame();
	}

	public void LoseScreen()
	{
		Debug.Log("Lose!");
		ResetGame();
	}

	public void ResetGame()
	{
		for (int i = 0; i < grips.Count; i++)
		{
			grips[i].Attach(false);
			if (grips[i] as Rune)
			{
				Rune rune= (Rune)grips[i];
				rune.InitRune();
			}
		}
		for (int i = 0; i < doors.Count; i++)
		{
			doors[i].InitDoor();
		}
		for (int i = 0; i < pickUps.Count; i++)
		{
			pickUps[i].InitPickUp();
		}


		

		RopePosToMouse.getInstance.ResetCurrentSpot();
		RopePosToMouse.getInstance.ReturnToCurrentPos();
		RopePosToMouse.getInstance.UpdateCurrentPos(RopePosToMouse.getInstance.currentPos);

	}
}
