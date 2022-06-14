using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	//private set public get
	private static GameManager _instance;
	public static GameManager getInstance => _instance;

	[Header("References")]
	public RopePhysic rope;
	public GhostMovement ghost;
	public GhostAnimator getGhostAnim => ghost.GetComponent<GhostAnimator>();
	public MousePosition mouse;
	public Animator deathVortex;
	public int GameLevelsAmount;
	//information saved for resetting
	Vector2 ghostStartPos;



	[Header("Temp")]
	////////////////////////////
	//change to private later and make functions in the correct region that will auto search
	public List<Rune> runes;
	public List<PickUpInteraction> pickUps;
	public List<DoorInteraction> doors;
	public List<MovingPlatform> platforms;
	public List<RailObstacle> rails;
	public List<EnemyController> enemies;
	///////////////////////////


	public GameState currentState;
    public float ghostVortexTime;
	int currentScene;
	public static bool completeLVL1, completeLVL2, completeLVL3, completeLVL4, completeLVL5, completeLVL6, completeLVL7, completeLVL8, completeLVL9;

	private LineGFXManager lineGFXMan => LineGFXManager.LineGFXManage;

	private void Awake()
	{
		if (_instance == null)
			_instance = this;
		else if (_instance != this)
			Destroy(this.gameObject);

		ghostStartPos = ghost.transform.position;
		InitGameManager();
		Application.targetFrameRate = 60;
	}


	void InitGameManager()
	{
		runes.Clear();
		pickUps.Clear();
		doors.Clear();
		platforms.Clear();
		rails.Clear();
		enemies.Clear();


		Rune[] runesInScene = UnityEngine.Object.FindObjectsOfType<Rune>();
		foreach (var rune in runesInScene)
		{
			runes.Add(rune);
		}

		PickUpInteraction[] pickUpsInScene = UnityEngine.Object.FindObjectsOfType<PickUpInteraction>();
		foreach (var pickup in pickUpsInScene)
		{
			pickUps.Add(pickup);
		}

		DoorInteraction[] doorsInScene = UnityEngine.Object.FindObjectsOfType<DoorInteraction>();
		foreach (var door in doorsInScene)
		{
			doors.Add(door);
		}

		MovingPlatform[] platformsInScene = UnityEngine.Object.FindObjectsOfType<MovingPlatform>();
		foreach (var platform in platformsInScene)
		{
			platforms.Add(platform);
		}

		RailObstacle[] railsInScene = UnityEngine.Object.FindObjectsOfType<RailObstacle>();
		foreach (var Rail in railsInScene)
		{
			rails.Add(Rail);
		}

		EnemyController[] enemiesInScene = UnityEngine.Object.FindObjectsOfType<EnemyController>();
		foreach (var enemy in enemiesInScene)
		{
			enemies.Add(enemy);
		}
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
		currentScene = SceneManager.GetActiveScene().buildIndex;
		if(currentScene < GameLevelsAmount)
		{
			// Move to the next level
			StartCoroutine(NextLevelTransition());
		}
        else
        {
			// Move to some thank you screen cuz the whole game was beaten
			Debug.Log("The whole game was beaten!");
			StartCoroutine(GameCompleteTransition());
		}

        #region levelsWin
		if(SceneManager.GetActiveScene().buildIndex == 1)
        {
			completeLVL1 = true;

		}
		else if (SceneManager.GetActiveScene().buildIndex == 2)
		{
			completeLVL2 = true;
		}
		else if (SceneManager.GetActiveScene().buildIndex == 3)
		{
			completeLVL3 = true;
		}
		else if (SceneManager.GetActiveScene().buildIndex == 4)
		{
			completeLVL4 = true;
		}
		else if (SceneManager.GetActiveScene().buildIndex == 5)
		{
			completeLVL5 = true;
		}
		else if (SceneManager.GetActiveScene().buildIndex == 6)
		{
			completeLVL6 = true;
		}
		else if (SceneManager.GetActiveScene().buildIndex == 7)
		{
			completeLVL7 = true;
		}
		else if (SceneManager.GetActiveScene().buildIndex == 8)
		{
			completeLVL8 = true;
		}
		else if (SceneManager.GetActiveScene().buildIndex == 9)
		{
			completeLVL9 = true;
		}
		#endregion
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
            if (runes[i].enabled)
                runes[i].InitRune();
        }
        //resets the doors
        for (int i = 0; i < doors.Count; i++)
        {
            if (doors[i].enabled)
                doors[i].InitDoor();
        }
        //resets pickables
        for (int i = 0; i < pickUps.Count; i++)
        {
            if (pickUps[i].enabled)
                pickUps[i].InitPickUp();
        }

        for (int i = 0; i < platforms.Count; i++)
        {
            if (platforms[i].enabled)
                platforms[i].ResetMovingPlatform();
        }

        for (int i = 0; i < rails.Count; i++)
        {
            if (rails[i].enabled)
                rails[i].ResetRail();
        }


        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].enabled)
                enemies[i].InitEnemy();
        }

        StartCoroutine( VortexAnimation());
    }


    IEnumerator VortexAnimation()
    {
		lineGFXMan.ResetLineGFX();

		//resetting the player position to the start
		ghost.ResetGhost();

        deathVortex.transform.position = ghost.transform.position;

        deathVortex.SetTrigger("SwallowTrigger");

		yield return new WaitForSeconds(0.46f);

		ghost.spriteRenderer.enabled = false;
		
		ghost.transform.position = ghostStartPos;

		yield return new WaitForSeconds(0.2f);

		deathVortex.transform.position = ghost.transform.position;

		deathVortex.SetTrigger("SpitTrigger");

		yield return new WaitForSeconds(0.2f);

		ghost.spriteRenderer.enabled = true;

		yield return new WaitForSeconds(0.2f);

		ghost.rb.gravityScale = 5;
		ghost.ropeGFXBool = false;
        SetGameState(GameState.draggingRope);
    }

	IEnumerator NextLevelTransition()
    {
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(currentScene + 1);
	}
	IEnumerator GameCompleteTransition()
	{
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(0);
	}




	#endregion


	public void SetGameState(GameState newState)
	{
		switch (newState)
		{
			case GameState.draggingRope:
				rope.ClearHittedRunes();
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
		//rope.UseRunes();
		ghost.SetGhostPath(rope.getGhostPath(), true);
	}

}


public enum GameState
{
	draggingRope,
	ghostMovement,
	pause
}


