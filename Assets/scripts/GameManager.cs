using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	//private set public get
	private static GameManager _instance;
	public static GameManager getInstance => _instance;

	[Header("References")]
	public RopePhysic rope;
	public GhostMovement ghost;

	//information saved for resetting
	Vector2 ghostStartPos;



	[Header("Temp")]
	////////////////////////////
	//change to private later and make functions in the correct region that will auto search for those three lists
	public List<Rune> runes;
	public List<PickUpInteraction> pickUps;
	public List<DoorInteraction> doors;
	public List<MovingPlatform> platforms;
	public List<RailObstacle> rails;
	///////////////////////////


	public GameState currentState;



	private void Awake()
	{
		if (_instance == null)
			_instance = this;
		else if (_instance != this)
			Destroy(this.gameObject);

		ghostStartPos = ghost.transform.position;
	//	QualitySettings.vSyncCount = 0;
	//	Application.targetFrameRate = 30;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.D))
		{
		//	ResetGame();
		}
	}

	#region Game Conditions
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
		//resets the runes
		for (int i = 0; i < runes.Count; i++)
		{
			runes[i].InitRune();
		}
		//resets the doors
		for (int i = 0; i < doors.Count; i++)
		{
			doors[i].InitDoor();
		}
		//resets pickables
		for (int i = 0; i < pickUps.Count; i++)
		{
			pickUps[i].InitPickUp();
		}

		for (int i = 0; i < platforms.Count; i++)
		{
			platforms[i].ResetMovingPlatform();
		}

		for (int i = 0; i < rails.Count; i++)
		{
			rails[i].ResetRail();
		}


		//resetting the player position to the start
		ghost.transform.position = ghostStartPos;

	}





	#endregion


	public void SetGameState(GameState newState)
	{
		switch (newState)
		{
			case GameState.draggingRope:
				break;
			case GameState.ghostMovement:
				StartGhostMovement();
				break;
			case GameState.pause:
				break;
			default:
				break;
		}
		currentState = newState;
	}

	void StartGhostMovement()
	{
		rope.UseRunes();
		ghost.SetGhostPath( rope.getGhostPath(), true);
	}

}


public enum GameState
{
	draggingRope,
	ghostMovement,
	pause
}


