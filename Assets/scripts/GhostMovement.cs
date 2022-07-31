using System.Collections.Generic;
using UnityEngine;


public class GhostMovement : MonoBehaviour
{
	[SerializeField]GhostAnimator animator;
	[Header("movement")]
	[SerializeField] List<Vector2> ghostPaths = new List<Vector2>();
	
	public float movementSpeed;

	[Header("References")]
	[SerializeField] public Rigidbody2D rb;
	[SerializeField] public SpriteRenderer spriteRenderer;

	[Header("Colliders")]
	[SerializeField] BoxCollider2D platformCollider;
	[SerializeField] BoxCollider2D hitableCollider;

	bool isLookingRight;

	//vars for saving info on start
	float ghostGravity;
	public bool startMovement;
	int pathIndex;
	bool finishedMovement;
	public bool ropeGFXBool = false;

	private void Awake()
	{
		InitPlayer();
	}

	private void FixedUpdate()
	{
		playerMovement();
	}

	void playerMovement()
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
					rb.velocity = (dir * movementSpeed * Time.deltaTime);


					GameManager.getInstance.getGhostAnim.RotateSpriteTowardDirection(ghostPaths[pathIndex]);


					if (rb.transform.position.x < ghostPaths[pathIndex].x)
					{
						if (!isLookingRight)
						{
							isLookingRight = true;
							Flip();
						}
					}
					else
					{
						if (isLookingRight)
						{
							isLookingRight = false;
							Flip();
						}
					}

				}
				else
				{
					Debug.Log("path index");
					if (RopePhysic.getInstance.GetRuneByPosition(ghostPaths[pathIndex]) != null)
					{
						RopePhysic.getInstance.GetRuneByPosition(ghostPaths[pathIndex]).UseRune();
					}

					if (pathIndex + 1 > ghostPaths.Count - 1)
					{
						rb.velocity = Vector2.zero;
						finishedMovement = true;
						GameManager.getInstance.getGhostAnim.RotateToNormal();
						GameManager.getInstance.getGhostAnim.SetAnimBool("Movement", false);
						GameManager.getInstance.getGhostAnim.SetAnimBool("Idle", true);
						//RopePhysic.getInstance.ResetRope();
						return;
					}
					pathIndex++;
				}
			}
			else
			{
				startMovement = false;
				ropeGFXBool = false;
				rb.gravityScale = ghostGravity;
				SetGhostCollider(false);
				GameManager.getInstance.SetGameState(GameState.draggingRope);
			}
		}
		else
		{
			if(GameManager.getInstance.currentState == GameState.draggingRope)
			{
				isLookingRight = this.transform.position.x > GameManager.getInstance.mouse.mouseRB.position.x
					? false : true;
				//Debug.Log("test looking right:" + isLookingRight);
				Flip();
			}
		}


	}

	void InitPlayer()
	{
		platformCollider.enabled = true;
		hitableCollider.enabled = false;
		ghostGravity = rb.gravityScale;
		isLookingRight = true;
		Flip();
		ResetPlayerGFX();
	}
	public void ResetPlayerGFX()
	{
		animator.SetAnimBool("Idle", true);
		animator.SetAnimBool("Movement", false);
	}

	public void ResetGhost()
	{
		rb.velocity = Vector2.zero;
		rb.gravityScale = 0;
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

		//    HERE

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



	void Flip()
	{
		spriteRenderer.flipX = isLookingRight;
	}

	public void SwitchOnRopeGFXBool()
	{
		ropeGFXBool = true;

	}

}
