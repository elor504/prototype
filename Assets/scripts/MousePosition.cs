using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
	public Rigidbody2D mouseRB;
	public RopePhysic rope;

	private LineGFXManager lineGFXMan => LineGFXManager.LineGFXManage;

	private void Update()
	{
		if (GameManager.getInstance.currentState != GameState.draggingRope)
			return;

		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			ResetMousePos();
			rope.InitRope();
			rope.isRopeActive = true;
		}

		if (Input.GetKey(KeyCode.Mouse0) && rope.isRopeActive)
		{
			
			Vector3 mouseWorldPos = Input.mousePosition;
			mouseWorldPos.z = Camera.main.nearClipPlane;

			Vector2 target = Camera.main.ScreenToWorldPoint(mouseWorldPos);

			mouseRB.MovePosition(target);



		}
		if (Input.GetKeyUp(KeyCode.Mouse0))
		{
			if (rope.GetLastRune() != null)
			{
				// HERE
				lineGFXMan.RemoveLastLineRenderer();
				GameManager.getInstance.SetGameState(GameState.ghostMovement);
			}
			ResetTotalController();
		}


		if (Input.GetKeyDown(KeyCode.R))
		{
			ResetTotalController();
		}

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
	public void ResetTotalController()
	{
		rope.ResetRope();
		ResetMousePos();
	}

	public void MoveMouseByTrans(Vector2 _pos)
	{
		mouseRB.transform.position = _pos;
	}
}
