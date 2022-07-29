using UnityEngine;
using UnityEngine.EventSystems;

public class MousePosition : MonoBehaviour
{
	public Rigidbody2D mouseRB;
	public RopePhysic rope;

	public float clamp;

	private LineGFXManager lineGFXMan => LineGFXManager.LineGFXManage;

	public float mouseSpeed;
	public bool rigidbodyMouse;
	Vector2 lastFrameMousePos;

	float distanceLerp;
	float distance;
	private void Update()
	{
		if (GameManager.getInstance.currentState != GameState.draggingRope)
			return;

		if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
		{
			ResetMousePos();
			rope.InitRope();
			//GameManager.getInstance.getGhostAnim.SetAnimBool("DragActive", true);
			//GameManager.getInstance.getGhostAnim.SetAnimBool("Idle", false);
			rope.isRopeActive = true;
		}

		if (Input.GetKey(KeyCode.Mouse0) && rope.isRopeActive)
		{
			if (!rigidbodyMouse)
			{
				Vector3 mouseWorldPos = Input.mousePosition;
				mouseWorldPos.z = Camera.main.nearClipPlane;
				Vector2 target = Camera.main.ScreenToWorldPoint(mouseWorldPos);
				mouseRB.MovePosition(target);
			}
			else
			{
				Vector3 mouseWorldPos = Input.mousePosition;
				mouseWorldPos.z = Camera.main.nearClipPlane;
				Vector2 target = Camera.main.ScreenToWorldPoint(mouseWorldPos);

				Vector2 dir = target - (Vector2)mouseRB.transform.position;
				if (Vector2.Distance(target, (Vector2)mouseRB.transform.position) > 0.05f)
				{
					//Vector2 velocity = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
					//velocity = new Vector2(Map(velocity.x, -2, 2, 0, 1), Map(velocity.y, -2, 2, 0, 1));


					//Debug.LogError("Mouse velocity: " + velocity);

					mouseRB.MovePosition(Vector2.Lerp(mouseRB.position, target, Time.deltaTime * mouseSpeed));
					//mouseRB.velocity =  dir * mouseSpeed * Time.deltaTime * velocity;
				}
				else
					mouseRB.velocity = Vector2.zero;
			}

		}
		if (Input.GetKeyUp(KeyCode.Mouse0))
		{
			rope.isRopeActive = false;
			if (rope.GetLastRune() != null)
			{
				// HERE
				if (lineGFXMan != null)
				{
					//lineGFXMan.RemoveLastLineRenderer();
					lineGFXMan.RemoveLastLinesBeforeLastRune(rope.getGhostPath());
				}
				GameManager.getInstance.SetGameState(GameState.ghostMovement);
				GameManager.getInstance.getGhostAnim.SetAnimBool("Movement", true);
				GameManager.getInstance.getGhostAnim.SetAnimBool("Idle", false);
			}
			else
			{
				GameManager.getInstance.getGhostAnim.SetAnimBool("Movement", false);
				GameManager.getInstance.getGhostAnim.SetAnimBool("Idle", true);
				if (lineGFXMan != null)
					lineGFXMan.ResetLineGFX();
			}



			//GameManager.getInstance.getGhostAnim.SetAnimBool("DragActive", false);

			ResetTotalController();
		}


		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			ResetTotalController(true);
			rope.ClearHittedRunes();
		}

	}

	float ScrollLimit(float dist, float min, float max)
	{
		if (dist < min)
			dist = min;
		if (dist > max)
			dist = max;
		return dist;
	}
	public float Map(float value, float inMin, float inMax, float OutMin, float outMax)
	{
		return (value - inMin) * (outMax - OutMin) / (inMax - inMin) + OutMin;
	}

	/// <summary>
	///  reset the mouse postiion to the player position
	/// </summary>
	void ResetMousePos()
	{
		mouseRB.transform.position = rope.playerTrans.position;
	}
	/// <summary>
	/// resets the mouse position to the player and disable the rope and reset it
	/// </summary>
	public void ResetTotalController(bool _TotalReset = false)
	{
		rope.ResetRope();
		ResetMousePos();
		if (_TotalReset)
		{
			LineGFXManager.LineGFXManage.ResetLineGFX();
		}
	}

	public void MoveMouseByTrans(Vector2 _pos)
	{
		//mouseRB.transform.position = _pos;
	}
}
