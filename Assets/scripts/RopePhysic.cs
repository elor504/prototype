﻿using System.Collections.Generic;
using UnityEngine;

public class RopePhysic : MonoBehaviour
{
	public bool isRopeActive;
	public List<Vector2> ropePositions = new List<Vector2>();
	[Header("Collider Masks")]
	public LayerMask colliderMask;
	public LayerMask colliderMouseMask;
	public LayerMask colliderMouseBlockerMask;
	public LayerMask colliderMapMask;
	[Header("References")]
	public MousePosition mouse;
	public Transform playerTrans;
	public Transform mouseTrans;
	[Header("Runes related")]
	public Dictionary<Vector2, Rune> hittedRunes = new Dictionary<Vector2, Rune>();

	// Update is called once per frame
	void Update()
	{
		if (isRopeActive)
		{
			if (ropePositions.Count == 0)
				return;


			if (ropePositions.Count > 1)
			{
				if (CheckIfThereIsABlocker() && ropePositions.Count == 2)
				{
					//put mouse on hit position
					Vector2 dir = ropePositions[0] - ropePositions[ropePositions.Count - 1];
					RaycastHit2D hit = Physics2D.Raycast(ropePositions[0], dir, 20);

					mouse.MoveMouseByTrans(hit.point);
				}


				UpdateRopePositions();
			}
			if (ropePositions.Count > 2) // has an anchor
			{
				DetectRopeExit();
			}



			DetectRopeCollision();
		}
	}

	public void InitRope()
	{
		ropePositions.Add(playerTrans.position);
		ropePositions.Add(mouseTrans.position);
	}
	public void ResetRope()
	{
		isRopeActive = false;
		ropePositions.Clear();
		hittedRunes.Clear();
	}

	void UpdateRopePositions()
	{
		ropePositions[0] = playerTrans.position;
		ropePositions[ropePositions.Count - 1] = mouseTrans.position;
	}

	public void AddNewRopePos(Vector2 _newPos)
	{
		Vector2 lastPos = ropePositions[ropePositions.Count - 1];
		ropePositions.Add(lastPos);
		if (ropePositions.Count - 2 >= 0)
			ropePositions[ropePositions.Count - 2] = _newPos;
	}
	void RemoveRopePosAt(Vector2 _pos)
	{
		if (ropePositions.Contains(_pos))
			ropePositions.Remove(_pos);
		Debug.Log("Removing at: " + _pos);
	}


	void DetectRopeCollision()
	{
		RaycastHit2D[] hit;
		hit = Physics2D.LinecastAll(ropePositions[ropePositions.Count - 2], ropePositions[ropePositions.Count - 1], colliderMask);

		RaycastHit2D hittedGrip;
		if (hit.Length == 0)
			return;

		if (hit.Length == 1)
		{
			hittedGrip = hit[0];
		}
		else
		{
			hittedGrip = hit[hit.Length - 1];
		}

		if (hittedGrip.collider.GetComponent<grip>())
		{
			grip hitGrip = hittedGrip.collider.GetComponent<grip>();

			if (hitGrip as Rune)
			{
				Rune hitRune = (Rune)hitGrip;
				if (hitRune.isTurnedOff)
					return;

				if (CheckIfGripCanBeAdded(hitGrip, hittedGrip) && !hittedRunes.ContainsKey(new Vector2(hitGrip.transform.position.x, hitGrip.transform.position.y)))
				{
					hittedRunes.Add(new Vector2(hitGrip.transform.position.x, hitGrip.transform.position.y), (Rune)hitGrip);
					AddNewRopePos(hitGrip.transform.position);
				}
			}
			else
			{
				if (CheckIfGripCanBeAdded(hitGrip, hittedGrip))
				{
					//	Debug.Log("adding collider: " + hittedGrip.point);
					AddNewRopePos(hitGrip.transform.position);
				}
			}


		}


		//if (!ropePositions.Contains(hittedGrip.point) && CheckIfCanEnterPosition(hittedGrip.point))
		//{
		////	Debug.Log("adding collider: " + hittedGrip.point);
		//	AddNewRopePos(hittedGrip.point);
		//}
		//else
		//{
		//	//Debug.Log("detecting the same collider: " + hittedGrip.collider.name);
		//}

	}
	void DetectRopeExit()
	{
		if (CheckIfBeforeLastRopePositionIsARune())
		{
			Debug.Log("cannot detach before last is a rune");
			return;
		}


		RaycastHit2D hit;
		Vector2 dir = (ropePositions[ropePositions.Count - 1] - ropePositions[ropePositions.Count - 3]).normalized;

		if (hit = Physics2D.Raycast(ropePositions[ropePositions.Count - 3], dir, 20, colliderMouseBlockerMask))
		{
			Debug.Log("detecting a blocker");
		}
		else if (hit = Physics2D.Raycast(ropePositions[ropePositions.Count - 3], dir, 20, colliderMouseMask))
		{
			

			Debug.Log("Detach");
			RemoveRopePosAt(ropePositions[ropePositions.Count - 2]);
		}
	}


	public Vector2 GetAnchorPoint => ropePositions[ropePositions.Count - 2];
	public List<Vector2> getAnchorLine()
	{
		List<Vector2> anchorPos = new List<Vector2>();
		anchorPos.Add(ropePositions[ropePositions.Count - 2]);
		anchorPos.Add(ropePositions[ropePositions.Count - 3]);
		return anchorPos;
	}

	#region bools


	bool CheckIfThereIsABlocker()
	{
		RaycastHit2D hit = Physics2D.Linecast(ropePositions[0], ropePositions[ropePositions.Count - 1], colliderMouseBlockerMask);

		return hit;
	}
	bool CheckIfGripCanBeAdded(grip hitGrip, RaycastHit2D hittedGrip)
	{
		return !hitGrip.isMouseOnGrip && !ropePositions.Contains(hittedGrip.collider.gameObject.transform.position);
	}



	bool CheckIfBeforeLastRopePositionIsARune()
	{
		Vector3 BeforeLastRopePos = ropePositions[ropePositions.Count - 2];

		Vector2 ropePos = new Vector2(BeforeLastRopePos.x, BeforeLastRopePos.y);


		return hittedRunes.ContainsKey(ropePos);
	}

	#endregion
	/// <summary>
	/// get the ghost path by taking the rope positions ingnoring the player position and the last grips before the last runes 
	/// </summary>
	/// <returns></returns>
	public List<Vector2> getGhostPath()
	{
		List<Vector2> ghostPath = new List<Vector2>();


		for (int i = 0; i < ropePositions.Count; i++)
		{
			if (ropePositions[i] == GetLastRunePostionInRopePositions())
			{
				ghostPath.Add(ropePositions[i]);
				break;
			}
			else if (i != 0)
			{
				ghostPath.Add(ropePositions[i]);
			}
		}
		return ghostPath;
	}

	public void UseRunes()
	{
		foreach (var rune in hittedRunes)
		{
			hittedRunes[rune.Key].UseRune();
		}
	}


	public Vector2 GetLastRunePostionInRopePositions()
	{
		return GetLastRune().transform.position;
	}


	public Rune GetLastRune()
	{
		Rune lastRune = null;
		for (int i = 0; i < ropePositions.Count; i++)
		{
			if (hittedRunes.ContainsKey(ropePositions[i]))
			{
				lastRune = hittedRunes[ropePositions[i]];
			}
		}
		return lastRune;
	}
	#region not usefull code but worth saving
	bool CheckIfCanEnterPosition(Vector2 _point)
	{
		//preventing points that are really close to eachother to be added
		bool canEnter = false;

		Vector2 bottomLeft = new Vector2(_point.x - 0.1f, _point.y - 0.1f);
		Vector2 BottomRight = new Vector2(_point.x + 0.1f, _point.y - 0.1f);
		Vector2 TopRight = new Vector2(_point.x + 0.1f, _point.y + 0.1f);
		Vector2 TopLeft = new Vector2(_point.x - 0.1f, _point.y + 0.1f);

		if (_point.x > bottomLeft.x && _point.y > bottomLeft.y &&
			_point.x < BottomRight.x && _point.y > BottomRight.y &&
			_point.x < TopRight.x && _point.y < TopRight.y &&
			_point.x > TopLeft.x && _point.y < TopLeft.y)
		{
			canEnter = true;
		}


		return canEnter;
	}
	#endregion



	private void OnDrawGizmos()
	{

		Gizmos.color = Color.red;

		for (int i = 0; i < ropePositions.Count - 1; i++)
		{
			Gizmos.DrawLine(ropePositions[i], ropePositions[i + 1]);
		}

		Gizmos.DrawWireCube(mouseTrans.position, Vector2.zero * 20f);




		if (ropePositions.Count > 2)
		{
			Gizmos.color = Color.blue;
			//Vector2 dir = (getAnchorLine()[0] - getAnchorLine()[1]).normalized;
			Vector2 dir = (ropePositions[ropePositions.Count - 1] - ropePositions[ropePositions.Count - 3]).normalized;
			Gizmos.DrawRay(ropePositions[ropePositions.Count - 3], dir * 20);


			Gizmos.color = Color.green;
			Gizmos.DrawLine(ropePositions[ropePositions.Count - 2], ropePositions[ropePositions.Count - 1]);
		}







	}

}
