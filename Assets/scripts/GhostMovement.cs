using System.Collections.Generic;
using UnityEngine;


public class GhostMovement : MonoBehaviour
{
	[Header("movement")]
	[SerializeField] List<Vector2> ghostPaths = new List<Vector2>();
	public float movementSpeed;

	[Header("References")]
	[SerializeField] Rigidbody2D rb;


	[Header("Colliders")]
	[SerializeField] BoxCollider2D platformCollider;
	[SerializeField] BoxCollider2D hitableCollider;



	//vars for saving info on start
	float ghostGravity;
	bool startMovement;
	int pathIndex;
	bool finishedMovement;

	private void Awake()
	{
		InitPlayer();
	}

	private void Update()
	{
		if (startMovement)
		{

			if (!finishedMovement)
			{
				float distanceToPath = Vector2.Distance(rb.position, ghostPaths[pathIndex]);
				if (distanceToPath > 0.1f)
				{
					Debug.Log("Moving");
					Vector2 dir = (ghostPaths[pathIndex] - rb.position).normalized;
					rb.MovePosition(rb.position + dir * Time.deltaTime * movementSpeed);
				}
				else
				{
					Debug.Log("path index");

					if (pathIndex + 1 > ghostPaths.Count - 1)
					{
						finishedMovement = true;
						return;
					}

					pathIndex++;
				}
			}
			else
			{
				startMovement = false;
				rb.gravityScale = ghostGravity;
				SetGhostCollider(false);
				GameManager.getInstance.SetGameState(GameState.draggingRope);
			}
		}
	}
	void InitPlayer()
	{
		platformCollider.enabled = true;
		hitableCollider.enabled = false;
		ghostGravity = rb.gravityScale;

	}

	public void ResetGhost()
	{
		finishedMovement = true;
		startMovement = false;
		ghostPaths.Clear();
	}


	public void SetGhostPath(List<Vector2> _paths, bool startMovement = false)
	{
		ghostPaths = new List<Vector2>();
		for (int i = 0; i < _paths.Count; i++)
		{
			ghostPaths.Add(_paths[i]);
		}
		if (startMovement)
			MoveGhostOnPath();
	}
	public void MoveGhostOnPath()
	{
		if (ghostPaths.Count == 0)
		{
			Debug.LogError("The ghost has no path to go on");
			return;
		}
		rb.gravityScale = 0;
		pathIndex = 0;
		finishedMovement = false;
		startMovement = true;
		SetGhostCollider(true);
	}
	void SetGhostCollider(bool _isMoving)
	{
		if (_isMoving)
		{
			platformCollider.enabled = false;
			hitableCollider.enabled = true;
		}
		else
		{
			platformCollider.enabled = true;
			hitableCollider.enabled = false;
		}
	}


}
