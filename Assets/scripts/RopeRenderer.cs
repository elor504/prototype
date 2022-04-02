using System.Collections.Generic;
using UnityEngine;

public class RopeRenderer : MonoBehaviour
{
	[SerializeField] Transform mousePosition;
	[SerializeField] Transform player;


	[SerializeField] LineRenderer rope;
	[SerializeField] LayerMask collMask;
	[SerializeField] LayerMask blockMask;

	public List<Vector3> ropePositions { get; set; } = new List<Vector3>();
	public List<grip> grips;
	public Rune currentRune;


	[Header("Colliders")]
	EdgeCollider2D edgeCollider;
	List<Vector2> colliderPoints = new List<Vector2>();
	[SerializeField] PolygonCollider2D pollygonCollider2D;

	public bool isActive;

	private void Awake()
	{
		AddPosToRope(Vector3.zero);
		edgeCollider = this.GetComponent<EdgeCollider2D>();
	}

	private void Update()
	{
		if (isActive)
		{
			DebugRopeAngle();
			//colliderPoints = CalculateColliderPoints();
			//pollygonCollider2D.SetPath(0, colliderPoints.ConvertAll(p => (Vector2)transform.InverseTransformPoint(p)));
			UpdateRopePositions();
			LastSegmentGoToPlayerPos();
			DetectRopeCollision();

			if (ropePositions.Count > 2)
			{
				DetectCollisionExits();
			}
		}
	}
	public void SetRopeActive(bool _isActive, Vector2 _startingPos)
	{
		isActive = _isActive;
		rope.enabled = _isActive;
		ropePositions[0] = _startingPos;
	}
	void DetectRopeCollision()
	{

		if (Physics2D.Linecast(mousePosition.position, rope.GetPosition(ropePositions.Count - 2), blockMask))
		{
			Debug.Log("Hitting block with collider");
			RaycastHit2D hit = Physics2D.Linecast(mousePosition.position, rope.GetPosition(ropePositions.Count - 2), blockMask);
			//SetLastRopePoisition(hit.transform.position);
			//RopePosToMouse.getInstance.SetMousePos(hit.collider.ClosestPoint(rope.GetPosition(ropePositions.Count - 2)));

		}
		else if (Physics2D.Linecast(mousePosition.position, rope.GetPosition(ropePositions.Count - 2), collMask))
		{
			//if the hit is grip
			if (Physics2D.Linecast(mousePosition.position, rope.GetPosition(ropePositions.Count - 2), collMask).collider.gameObject.GetComponent<grip>())
			{
				grip hit = Physics2D.Linecast(mousePosition.position, rope.GetPosition(ropePositions.Count - 2), collMask).collider.gameObject.GetComponent<grip>();



				if (hit.getIsBeingUsed && grips.Find(x => x.gameObject == hit.gameObject))
					return;

				if (hit as Rune)
				{
					Rune hittedRune = hit.GetComponent<Rune>();
					if (!hittedRune.getIsTurnedOff)
					{

						hittedRune.DisableRune();
						ropePositions.RemoveAt(ropePositions.Count - 1);
						AddPosToRope(hit.transform.position);
						grips.Add(hit);
						hit.Attach(true, isRopeOnRightSide(ropePositions[ropePositions.Count - 1], grips[grips.Count - 1].GetGripPosition));
					}
				}
				else
				{
					ropePositions.RemoveAt(ropePositions.Count - 1);
					AddPosToRope(hit.transform.position);
					grips.Add(hit);
					hit.Attach(true, isRopeOnRightSide(ropePositions[ropePositions.Count - 1], hit.GetGripPosition));
				}

			}
		}

	}


	private void AddPosToRope(Vector3 _pos)
	{
		ropePositions.Add(_pos);
		ropePositions.Add(mousePosition.position); //Always the last pos must be the player
	}
	private void DetectCollisionExits()
	{
		Vector2 lastpos = mousePosition.position;
		Vector2 previousPos = rope.GetPosition(ropePositions.Count - 2);

		float angle = Mathf.Atan2(previousPos.y - lastpos.y, previousPos.x - lastpos.x) * 180 / Mathf.PI;
		angle *= -1;

		//	Debug.Log("Angle: " + angle);

		if (grips.Count != 0)
		{
			grip lastGrip = grips[grips.Count - 1];
			if (lastGrip as Rune)
			{
				return;
			}

			if (DoTheRopeNeedsToDetach(rope.GetPosition(ropePositions.Count - 3), rope.GetPosition(ropePositions.Count - 2), mousePosition.position))
			{
				DetachRope();
			}

			//if (lastGrip.IsOnBreakableAngleRange(angle))
			//{
			//	//Debug.LogError("Detach");
			//	DetachRope();
			//}
		}


		//if (angle >= 5)
		//{
		//	Debug.Log("Exit ");
		//	ropePositions.RemoveAt(ropePositions.Count - 2);
		//}

	}
	private void DetachRope()
	{
		grips[grips.Count - 1].Attach(false, isRopeOnRightSide(ropePositions[ropePositions.Count - 1], grips[grips.Count - 1].GetGripPosition));
		ropePositions.RemoveAt(ropePositions.Count - 2);
		grips.RemoveAt(grips.Count - 1);
	}
	private void UpdateRopePositions()
	{
		rope.SetPosition(0, player.position);
		rope.positionCount = ropePositions.Count;
		rope.SetPositions(ropePositions.ToArray());
	}
	private void LastSegmentGoToPlayerPos() => rope.SetPosition(rope.positionCount - 1, mousePosition.position);
	private void SetLastRopePoisition(Vector2 _pos)
	{
		ropePositions[ropePositions.Count - 1] = _pos;
	}
	public void ResetRope()
	{
		ropePositions.Clear();

		for (int i = 0; i < grips.Count; i++)
		{
			grips[i].Attach(false, false);
		}
		grips.Clear();
		AddPosToRope(RopePosToMouse.getInstance.currentPos);
	}

	public bool hasRuneAttached()
	{
		if (grips.Count == 0)
			return false;

		if (grips[0] as Rune)
		{
			return true;
		}
		for (int i = grips.Count - 1; i > 0; i--)
		{
			if (grips[i] as Rune)
			{
				return true;
			}
		}
		return false;
	}
	public Vector2 GetLastRunePosition()
	{
		if (grips[grips.Count - 1] as Rune && currentRune != grips[grips.Count - 1])
		{
			currentRune = (Rune)grips[grips.Count - 1];
			currentRune.DisableRune();
			return grips[grips.Count - 1].GetGripPosition;
		}
		for (int i = grips.Count - 1; i > 0; i--)
		{
			if (grips[i] as Rune && currentRune != grips[i])
			{
				currentRune = (Rune)grips[i];
				currentRune.DisableRune();
				return grips[i].GetGripPosition;
			}
		}

		return RopePosToMouse.getInstance.currentPos;
	}



	private List<Vector2> CalculateColliderPoints()
	{
		List<Vector3> positions = new List<Vector3>();
		//get all positions on the line renderer
		for (int i = 0; i < rope.positionCount; i++)
		{
			positions.Add(rope.GetPosition(i));
		}

		//get the width of the line
		float width = rope.widthMultiplier;

		//m = (y2 -y1) / (x2 - x1)
		float m = (positions[1].y - positions[0].y) / (positions[1].x - positions[0].x);
		float deltaX = (width / 2f) * (m / Mathf.Pow(m * m + 1, 0.5f));
		float deltaY = (width / 2f) * (1 / Mathf.Pow(1 + m * m, 0.5f));

		//Calcualte the offset from each point to the collision vertex
		Vector3[] offsets = new Vector3[2];
		offsets[0] = new Vector3(-deltaX, deltaY);
		offsets[1] = new Vector3(deltaX, -deltaY);

		//Generate the colliders Vertices
		List<Vector2> colliderPositions = new List<Vector2>
		{
			positions[0] + offsets[0],
			positions[1] + offsets[0],
			positions[1] + offsets[1],
			positions[0] + offsets[1]
		};

		return colliderPositions;
	}
	public Vector2 GetBeforeLastPosition()
	{
		return ropePositions[ropePositions.Count - 2];
	}

	private bool isRopeOnRightSide(Vector2 ropePos, Vector2 gripPos)
	{
		return ropePos.x > gripPos.x;
	}


	//visual
	public void SetPlayerPosition(Vector2 _pos)
	{
		player.transform.position = _pos;
	}


	//debug
	void DebugRopeAngle()
	{
		//Vector2 lastpos = mousePosition.position;
		//Vector2 previousPos = rope.GetPosition(ropePositions.Count - 2);

		//float angle = Mathf.Atan2(previousPos.y - lastpos.y, previousPos.x - lastpos.x) * 180 / Mathf.PI;
		//angle *= -1;


		Vector3 direction = rope.GetPosition(ropePositions.Count - 2) - mousePosition.position;

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		float bearing = (angle + 360) % 360;




		//	Debug.Log("Angle: " + angle + " bearing: " + bearing);
	}
	public float GetAngle(Vector2 _firstPos, Vector2 _lastPos)
	{
		Vector3 direction = _firstPos - _lastPos;

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		float bearing = (angle + 360) % 360;

		return bearing;
	}

	public bool DoTheRopeNeedsToDetach(Vector2 _firstPos, Vector2 _secondPos, Vector2 _mousePos)
	{
		bool detach = false;

		float lastLineAngle = GetAngle(_firstPos, _secondPos);
		float mouseLineAngle = GetAngle(_secondPos, _mousePos);

		//Debug.LogError("Last Line Angle: " + lastLineAngle + " Mouse Line Angle: " + mouseLineAngle);

		float angleOffset = 0;
		if (grips[grips.Count - 1].getIsRopeOnRight)
		{
			angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
		}
		else
		{
			angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
		}



		float offsetToBreak = lastLineAngle + angleOffset;
		if (mouseLineAngle != 0)
		{
			//Debug.LogError("Offset: " + offsetToBreak + " Last Line Angle: " + lastLineAngle + " Mouse Line Angle: " + mouseLineAngle + " is on right?:  " + grips[grips.Count - 1].getIsRopeOnRight);
			if (grips[grips.Count - 1].getIsRopeOnRight)
			{
				Debug.Log("");
				//Debug.LogError("Right Side Mouse Line Angle: " + mouseLineAngle + " < Mouse Offset: " + offsetToBreak);
				if (mouseLineAngle < offsetToBreak)
				{
					//Debug.LogError("Is grip on the right?: " + grips[grips.Count - 1].getIsRopeOnRight + " MouseLineAngle: " + mouseLineAngle + " Required to break: " + (angleOffset + lastLineAngle));
					//Debug.LogError("Detach");
					detach = true;
				}
			}
			else
			{
			//	Debug.LogError("Left Side Mouse Line Angle: " + mouseLineAngle + " < Mouse Offset: " + offsetToBreak);
				if (mouseLineAngle < offsetToBreak)
				{
					//Debug.LogError("Is grip on the right?: " + grips[grips.Count - 1].getIsRopeOnRight + " MouseLineAngle: " + mouseLineAngle + " Required to break: " + (angleOffset + lastLineAngle));
					//Debug.LogError("Detach");
					detach = true;
				}
			}
		}

		//float angle = GetAngle(_secondPos, _mousePos);
		//float angleFirstLine = GetAngle(_firstPos, _secondPos);


		//float angleToBreak = angle;

		//if (angleToBreak + angleOffset > 360)
		//{
		//	float leftOvers = (angleToBreak + angleOffset) - 360;
		//	angleToBreak = leftOvers;
		//}
		//else if (angleToBreak - angleOffset < 0)
		//{
		//	float leftOvers = (angle - angleToBreak) + 360;
		//	angleToBreak = leftOvers;
		//}
		//else
		//{
		//	angleToBreak += angleOffset;
		//}



		//if(grips[grips.Count - 1].getIsRopeOnRight)
		//{

		//}
		//else
		//{
		//	if (angleToBreak < angle)
		//		detach = true;
		//}


		//Debug.Log("Angle at: " + angle + " Angle to break: " + angleToBreak + " First Line Angle: " + angleFirstLine + " Detach: " + detach);
		return detach;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.black;
		if (colliderPoints != null) colliderPoints.ForEach(p => Gizmos.DrawSphere(p, 0.1f));
	}

}
