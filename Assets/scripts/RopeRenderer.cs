using System.Collections.Generic;
using UnityEngine;

public class RopeRenderer : MonoBehaviour
{
	[SerializeField] Transform mousePosition;
	[SerializeField] Transform player;


	[SerializeField] LineRenderer rope;
	[SerializeField] LayerMask collMask;
	[SerializeField] LayerMask blockMask;

	public List<Vector3> ropePositions = new List<Vector3>();
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
			//DebugRopeAngle();
			//colliderPoints = CalculateColliderPoints();
			//pollygonCollider2D.SetPath(0, colliderPoints.ConvertAll(p => (Vector2)transform.InverseTransformPoint(p)));

		
			UpdateRopePositions();
			LastSegmentGoToPlayerPos();
			//DetectRopeCollision();

			if (ropePositions.Count > 2)
			{
				//Debug.Log("Test: " + GetAngle(grips[grips.Count - 1].GetGripPosition, mousePosition.position));
				DetectCollisionExits();
			}
		}
	}

	private void FixedUpdate()
	{
		if (isActive)
		{
			DetectRopeCollision();
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
		//RaycastHit2D hit;
		//if (Physics2D.Linecast(player.position, rope.GetPosition(ropePositions.Count - 2), collMask))
		//{
		//	hit = Physics2D.Linecast(player.position, rope.GetPosition(ropePositions.Count - 2), collMask);
		//	if (System.Math.Abs(Vector3.Distance(rope.GetPosition(ropePositions.Count - 2), hit.point)) > 0.4f)
		//	{
		//		ropePositions.RemoveAt(ropePositions.Count - 1);
		//		AddPosToRope(hit.point);
		//	}
		//}



		if (Physics2D.Linecast(mousePosition.position, ropePositions[ropePositions.Count - 2], collMask))
		{

			//if the hit is grip
			if (Physics2D.Linecast(mousePosition.position, ropePositions[ropePositions.Count - 2], collMask).collider.gameObject.GetComponent<grip>())
			{
				grip hit = Physics2D.Linecast(mousePosition.position, ropePositions[ropePositions.Count - 2], collMask).collider.gameObject.GetComponent<grip>();

				if (hit.getIsBeingUsed && grips.Find(x => x.gameObject == hit.gameObject))
					return;

				//if (!IsTheRopeOnTheRightAngleToConnect(rope.GetPosition(ropePositions.Count - 1), rope.GetPosition(ropePositions.Count - 2), mousePosition.position, hit.gameObject.transform.position))
				//	return;

				if (hit.isMouseOnGrip)
					return;

				if (hit as Rune)
				{
					Rune hittedRune = hit.GetComponent<Rune>();
					if (!hittedRune.getIsTurnedOff)
					{

						//making sure that this ruse cannot be useable again
						hittedRune.DisableRune();

						ropePositions.RemoveAt(ropePositions.Count - 1);
						AddPosToRope(hit.transform.position);
						grips.Add(hit);
						hit.Attach(true, isRopeOnRightSide(ropePositions[ropePositions.Count - 1], hit.GetGripPosition), IsRopeAbove(ropePositions[ropePositions.Count - 2], hit.GetGripPosition),
											IsRopeAbove(ropePositions[ropePositions.Count - 1], grips[grips.Count - 1].GetGripPosition), isRopeOnRightSide(ropePositions[ropePositions.Count - 1], grips[grips.Count - 1].GetGripPosition));
					}
				}
				else
				{
					ropePositions.RemoveAt(ropePositions.Count - 1);
					AddPosToRope(hit.transform.position);
					grips.Add(hit);
					hit.Attach(true, isRopeOnRightSide(ropePositions[ropePositions.Count - 1], hit.GetGripPosition), IsRopeAbove(ropePositions[ropePositions.Count - 2], hit.GetGripPosition),
						IsRopeAbove(ropePositions[ropePositions.Count - 1], grips[grips.Count - 1].GetGripPosition), isRopeOnRightSide(ropePositions[ropePositions.Count - 1], grips[grips.Count - 1].GetGripPosition));
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

		if (grips.Count != 0)
		{
			grip lastGrip = grips[grips.Count - 1];
			if (lastGrip as Rune)
			{
				return;
			}

			if (DoTheRopeNeedsToDetach(ropePositions[ropePositions.Count - 3], ropePositions[ropePositions.Count - 2], mousePosition.position, lastGrip))
			{
				DetachRope();
			}
		}

	}
	private void DetachRope()
	{
		grips[grips.Count - 1].Attach(false, isRopeOnRightSide(ropePositions[ropePositions.Count - 2], grips[grips.Count - 1].GetGripPosition), IsRopeAbove(ropePositions[ropePositions.Count - 2], grips[grips.Count - 1].GetGripPosition)
			, IsRopeAbove(ropePositions[ropePositions.Count - 1], grips[grips.Count - 1].GetGripPosition), isRopeOnRightSide(ropePositions[ropePositions.Count - 1], grips[grips.Count - 1].GetGripPosition));
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
			grips[i].Attach(false, false, false, false, false);
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

	private bool isRopeOnRightSide(Vector2 _ropePos, Vector2 _gripPos)
	{
		return _ropePos.x > _gripPos.x;
	}
	private bool IsRopeAbove(Vector2 _ropePos, Vector2 _gripPos)
	{
		return _ropePos.y > _gripPos.y;
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

	public bool DoTheRopeNeedsToDetach(Vector2 _firstPos, Vector2 _secondPos, Vector2 _mousePos, grip _grip)
	{


		bool detach = false;

		float lastLineAngle = GetAngle(_firstPos, _secondPos);
		float mouseLineAngle = GetAngle(_secondPos, _mousePos);

		bool isLastPosAbove = _firstPos.y > _mousePos.y;
		bool isLastPosRight = _firstPos.x > _mousePos.y;
		//Debug.Log("Is Last Pos Above: " + isLastPosAbove);

	//	Debug.LogError("Last Line Angle: " + lastLineAngle + " Mouse Line Angle: " + mouseLineAngle);

		float angleOffset = 0;

		bool isGripUpperCorner = _grip.getIsUpperCorner;
		bool isGripRightcorner = _grip.getIsRightCorner;


		if (isGripUpperCorner)
		{

			if (grips[grips.Count - 1].getIsRopeOnRight)
			{
				if (isGripRightcorner)
				{
					if (grips[grips.Count - 1].isMouseAbove)
					{
						if (grips[grips.Count - 1].getIsRopeAbove)
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki1");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki2");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
						else
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki3");
								angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki4");
								angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
					}
					else
					{
						if (grips[grips.Count - 1].getIsRopeAbove)
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki5");
								angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki6");
								angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
						else
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki7");
								angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki8");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
					}
				}
				else
				{
					//
					if (grips[grips.Count - 1].isMouseAbove)
					{
						if (grips[grips.Count - 1].getIsRopeAbove)
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki9");
								angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki10");
								angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
						else
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki11");
								angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki12");
								angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
					}
					else
					{
						if (grips[grips.Count - 1].getIsRopeAbove)
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki13");
								angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki14");
								angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
						else
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki15");
								angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki16");
								angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
					}
				}
			}
			else
			{
				if (isGripRightcorner)
				{
					if (grips[grips.Count - 1].isMouseAbove)
					{
						if (grips[grips.Count - 1].getIsRopeAbove)
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kak17");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kak18");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
						else
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kak19");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kak20");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
					}
					else
					{

						if (grips[grips.Count - 1].getIsRopeAbove)
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki21");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki22");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
						else
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki23");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki24");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
					}
				}
				else
				{
					if (grips[grips.Count - 1].isMouseAbove)
					{
						if (grips[grips.Count - 1].getIsRopeAbove)
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki25");
								angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki26");
								angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
						else
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki27");
								angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki28");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
					}
					else
					{
						if (grips[grips.Count - 1].getIsRopeAbove)
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki29");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki30");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
						else
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki31");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki32");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
					}
				}
			}
		}
		else
		{
			if (grips[grips.Count - 1].getIsRopeOnRight)
			{
				if (isGripRightcorner)
				{
					if (grips[grips.Count - 1].isMouseAbove)
					{
						if (grips[grips.Count - 1].getIsRopeAbove)
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki33");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki34");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
						else
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki35");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki36");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
					}
					else
					{
						if (grips[grips.Count - 1].getIsRopeAbove)
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki37");
								angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki38");
								angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
						else
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki39");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki40");
								angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
					}
				}
				else
				{
					if (grips[grips.Count - 1].isMouseAbove)
					{
						if (grips[grips.Count - 1].getIsRopeAbove)
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki41");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki42");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
						else
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki43");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki44");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
					}
					else
					{
						if (grips[grips.Count - 1].getIsRopeAbove)
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki45");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki46");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
						else
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki47");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki48");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
					}
				}
			}
			else
			{
				if (isGripRightcorner)
				{
					if (isLastPosRight)
					{
						if (grips[grips.Count - 1].isMouseAbove)
						{
							if (grips[grips.Count - 1].getIsRopeAbove)
							{
								if (grips[grips.Count - 1].isMouseOnRight)
								{
									Debug.Log("Kaki49");
									angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
								}
								else
								{
									Debug.Log("Kaki50");
									angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
								}
							}
							else
							{
								if (grips[grips.Count - 1].isMouseOnRight)
								{
									Debug.Log("Kaki51");
									angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
								}
								else
								{
									Debug.Log("Kaki52");
									angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
								}
							}
						}
						else
						{
							if (grips[grips.Count - 1].getIsRopeAbove)
							{
								if (grips[grips.Count - 1].isMouseOnRight)
								{
									Debug.Log("Kaki53");
									angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
								}
								else
								{
									Debug.Log("Kaki54");
									angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
								}
							}
							else
							{
								if (grips[grips.Count - 1].isMouseOnRight)
								{
									Debug.Log("Kaki55");
									angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
								}
								else
								{
									Debug.Log("Kaki56");
									angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;

								}
							}
						}
					}
					else
					{
						if (grips[grips.Count - 1].isMouseAbove)
						{
							if (grips[grips.Count - 1].getIsRopeAbove)
							{
								if (grips[grips.Count - 1].isMouseOnRight)
								{
									Debug.Log("Kaki57");
									angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
								}
								else
								{
									Debug.Log("Kaki58");
									angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
								}
							}
							else
							{
								if (grips[grips.Count - 1].isMouseOnRight)
								{
									Debug.Log("Kaki59");
									angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
								}
								else
								{
									Debug.Log("Kaki60");
									angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
								}
							}
						}
						else
						{
							if (grips[grips.Count - 1].getIsRopeAbove)
							{
								if (grips[grips.Count - 1].isMouseOnRight)
								{
									Debug.Log("Kaki61");
									angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
								}
								else
								{
									Debug.Log("Kaki62");
									angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
								}
							}
							else
							{
								if (grips[grips.Count - 1].isMouseOnRight)
								{
									Debug.Log("Kaki63");
									angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
								}
								else
								{
									Debug.Log("Kaki64");
									angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
								}
							}
						}
					}
				}
				else
				{
					if (grips[grips.Count - 1].isMouseAbove)
					{
						if (grips[grips.Count - 1].getIsRopeAbove)
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki65");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki66");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
						else
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki67");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki68");
								angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
					}
					else
					{
						if (grips[grips.Count - 1].getIsRopeAbove)
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki69");
								angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{

								Debug.Log("Kaki70");
								angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
						else
						{
							if (grips[grips.Count - 1].isMouseOnRight)
							{
								Debug.Log("Kaki71");
								angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
							else
							{
								Debug.Log("Kaki72");
								angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
							}
						}
					}
				}
			}
		}



		float offsetToBreak = 0;
		bool isNegative = true;
		if (angleOffset > 0)
			isNegative = false;


		if (lastLineAngle + angleOffset > 360)
		{
			float leftOver = (lastLineAngle + angleOffset) - 360;
			offsetToBreak = leftOver;

			
		}
		else if(lastLineAngle + angleOffset < 0)
		{
			float leftOver = (lastLineAngle + angleOffset) + 360;
			offsetToBreak = leftOver;

			
		}
		else
		{
			offsetToBreak = lastLineAngle + angleOffset;
		}

		


		Debug.Log("offsetToBreak: " + offsetToBreak + " Offset: " + angleOffset + " Mouse Line Angle: " + mouseLineAngle);

		detach = CanDetach(grips[grips.Count - 1].getIsRopeOnRight, grips[grips.Count - 1].getIsRopeAbove, grips[grips.Count - 1].isMouseOnRight, grips[grips.Count - 1].isMouseAbove,
			isLastPosAbove, isGripUpperCorner, isGripRightcorner, mouseLineAngle, offsetToBreak);


		return detach;
	}
	//public float CalculateOffSet()
	//{

	//	if (isGripUpperCorner)
	//	{

	//		if (grips[grips.Count - 1].getIsRopeOnRight)
	//		{
	//			if (isGripRightcorner)
	//			{
	//				if (grips[grips.Count - 1].getIsRopeAbove)
	//				{
	//					angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
	//				}
	//				else
	//				{
	//					angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
	//				}
	//			}
	//			else
	//			{
	//				//
	//				if (grips[grips.Count - 1].getIsRopeAbove)
	//				{
	//					Debug.Log("Kaki");
	//					angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
	//				}
	//				else
	//				{
	//					Debug.Log("Kaki");
	//					angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
	//				}
	//			}
	//		}
	//		else
	//		{
	//			if (isGripRightcorner)
	//			{
	//				if (grips[grips.Count - 1].getIsRopeAbove)
	//				{
	//					Debug.Log("Kaki");
	//					angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
	//				}
	//				else
	//				{
	//					Debug.Log("Kaki");
	//					angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
	//				}
	//			}
	//			else
	//			{
	//				if (grips[grips.Count - 1].getIsRopeAbove)
	//				{
	//					Debug.Log("Kaki");
	//					angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
	//				}
	//				else
	//				{
	//					Debug.Log("Kaki");
	//					angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
	//				}
	//			}
	//		}
	//	}
	//	else
	//	{
	//		if (grips[grips.Count - 1].getIsRopeOnRight)
	//		{
	//			if (isGripRightcorner)
	//			{
	//				if (grips[grips.Count - 1].getIsRopeAbove)
	//				{
	//					Debug.Log("Kaki");
	//					angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
	//				}
	//				else
	//				{
	//					Debug.Log("Kaki");
	//					angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
	//				}
	//			}
	//		}
	//		else
	//		{
	//			if (isGripRightcorner)
	//			{
	//				if (grips[grips.Count - 1].getIsRopeAbove)
	//				{
	//					Debug.Log("Kaki");
	//					angleOffset = -GameManager.getInstance.getGameSettings.getRopeDetachOffset;
	//				}
	//				else
	//				{
	//					Debug.Log("Kaki");
	//					angleOffset = GameManager.getInstance.getGameSettings.getRopeDetachOffset;
	//				}
	//			}
	//		}
	//	}


	//}

	public bool CanDetach(bool _isGripOnRight, bool _isRopeAbove, bool isMouseAbove, bool ismouseRight, bool _isLastPosAbove, bool _isGripUpperCorner, bool _isGripRightcorner, float _mouseLineAngle, float _offsetToBreak)
	{
		bool detach = false;

		if (grips[grips.Count - 1].isMouseOnGrip)
			return true;

		if (_mouseLineAngle != 0)
		{
			//Debug.LogError("Offset: " + _offsetToBreak + " Mouse Line Angle: " + _mouseLineAngle + " is on right?:  " + grips[grips.Count - 1].getIsRopeOnRight);
			if (_isGripOnRight)
			{
			
				if (_isGripUpperCorner)
				{
					if (_isGripRightcorner)
					{
						if (_isRopeAbove)
						{
							if (isMouseAbove)
							{

								if (ismouseRight)
								{
									if (_mouseLineAngle < _offsetToBreak)
									{
										Debug.Log("Test1");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " < Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
								else
								{
									if (_mouseLineAngle < _offsetToBreak)
									{
										Debug.Log("Test2");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " < Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
							}
							else
							{
								if (ismouseRight)
								{
									if (_mouseLineAngle < _offsetToBreak)
									{
										Debug.Log("Test3");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " < Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
								else
								{
									if (_mouseLineAngle < _offsetToBreak)
									{
										Debug.Log("Test4");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " < Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
							}

						}
						else
						{
							if (isMouseAbove)
							{
								if (ismouseRight)
								{
									
									if (_mouseLineAngle > _offsetToBreak)
									{
										Debug.Log("Test5");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
								else
								{
									if (_mouseLineAngle > _offsetToBreak)
									{
										Debug.Log("Test6");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
							}

							else
							{
								if (ismouseRight)
								{
									if (_mouseLineAngle < _offsetToBreak)
									{
										Debug.Log("Test7");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
								else
								{
									if (_mouseLineAngle > _offsetToBreak)
									{
										Debug.Log("Test8");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
							}
						}
					}
					else
					{
						if (_isRopeAbove)
						{
							//
							if (isMouseAbove)
							{
								if (ismouseRight)
								{
							
									if (_mouseLineAngle > _offsetToBreak)
									{
										Debug.Log("Test9");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
								else
								{
									if (_mouseLineAngle > _offsetToBreak)
									{
										Debug.Log("Test10");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
							}
							else
							{
								if (ismouseRight)
								{
									if (_mouseLineAngle > _offsetToBreak)
									{
										Debug.Log("Test11");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
								else
								{
									if (_mouseLineAngle > _offsetToBreak)
									{
										Debug.Log("Test12");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
							}
						}
						else
						{
							if (isMouseAbove)
							{
								if (ismouseRight)
								{
									Debug.Log("Test13");
									if (_mouseLineAngle > _offsetToBreak)
									{
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
								else
								{
									if (_mouseLineAngle > _offsetToBreak)
									{
										Debug.Log("Test14");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
							}
							else
							{
								if (ismouseRight)
								{
									if (_mouseLineAngle > _offsetToBreak)
									{
										Debug.Log("Test15");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
								else
								{
									if (_mouseLineAngle > _offsetToBreak)
									{
										Debug.Log("Test16");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
							}
						}
					}

				}
				else
				{

					if (_isRopeAbove)
					{
						if (isMouseAbove)
						{
							if (ismouseRight)
							{
								if (_mouseLineAngle < _offsetToBreak)
								{
									Debug.Log("Test17");
									Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " < Mouse Offset: " + _offsetToBreak);
									detach = true;
								}
							}
							else
							{
								if (_mouseLineAngle < _offsetToBreak)
								{
									Debug.Log("Test18");
									Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " < Mouse Offset: " + _offsetToBreak);
									detach = true;
								}
							}
						}
						else
						{
							if (ismouseRight)
							{
								if (_mouseLineAngle < _offsetToBreak)
								{
									Debug.Log("Test19");
									Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " < Mouse Offset: " + _offsetToBreak);
									detach = true;
								}
							}
							else
							{
								if (_mouseLineAngle > _offsetToBreak)
								{
									Debug.Log("Test20");
									Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
									detach = true;
								}
							}
						}
					}
					else
					{
						if (isMouseAbove)
						{
							if (ismouseRight)
							{
								if (_mouseLineAngle < _offsetToBreak)
								{
									Debug.Log("Test21");
									Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
									detach = true;
								}
							}
							else
							{
								if (_mouseLineAngle > _offsetToBreak)
								{
									Debug.Log("Test22");
									Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
									detach = true;
								}
							}
						}
						else
						{
							if (ismouseRight)
							{
								if (_mouseLineAngle < _offsetToBreak)
								{
									Debug.Log("Test23");
									Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " < Mouse Offset: " + _offsetToBreak);
									detach = true;
								}
							}
							else
							{
								if (_mouseLineAngle < _offsetToBreak)
								{
									Debug.Log("Test24");
									Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " < Mouse Offset: " + _offsetToBreak);
									detach = true;
								}
							}
						}
					}

				}
			}
			else
			{
				if (_isGripUpperCorner)
				{
					if (_isGripRightcorner)
					{
						if (_isRopeAbove)
						{
							if (isMouseAbove)
							{
								if (ismouseRight)
								{
									if (_mouseLineAngle < _offsetToBreak)
									{
										Debug.Log("Test25");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " < Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
								else
								{
									if (_mouseLineAngle < _offsetToBreak)
									{
										Debug.Log("Test26");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " < Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
							}
							else
							{
								if (ismouseRight)
								{
									if (_mouseLineAngle < _offsetToBreak)
									{
										Debug.Log("Test27");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " < Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
								else
								{
									if (_mouseLineAngle < _offsetToBreak)
									{
										Debug.Log("Test28");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " < Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
							}

						}
						else
						{
							if (isMouseAbove)
							{
								if (ismouseRight)
								{
									if (_mouseLineAngle < _offsetToBreak)
									{
										Debug.Log("Test29");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " < Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
								else
								{
									if (_mouseLineAngle < _offsetToBreak)
									{
										Debug.Log("Test30");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
							}

							else
							{
								if (ismouseRight)
								{
									if (_mouseLineAngle < _offsetToBreak)
									{
										//Debug.Log("Test31");
									//	Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " < Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
								else
								{
									if (_mouseLineAngle > _offsetToBreak)
									{
										Debug.Log("Test32");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
							}
						}
					}
					else
					{
						if (_isRopeAbove)
						{
							//
							if (isMouseAbove)
							{
								if (ismouseRight)
								{
									if (_mouseLineAngle > _offsetToBreak)
									{
										Debug.Log("Test33");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
								else
								{
									if (_mouseLineAngle > _offsetToBreak)
									{
										Debug.Log("Test34");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
							}
							else
							{
								if (ismouseRight)
								{
									if (_mouseLineAngle > _offsetToBreak)
									{
										Debug.Log("Test35");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
								else
								{
									if (_mouseLineAngle > _offsetToBreak)
									{
										Debug.Log("Test36");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
							}
						}
						else
						{
							if (isMouseAbove)
							{
								if (ismouseRight)
								{
									if (_mouseLineAngle > _offsetToBreak)
									{
										Debug.Log("Test37");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
								else
								{
									if (_mouseLineAngle > _offsetToBreak)
									{
										Debug.Log("Test38");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
							}
							else
							{
								if (ismouseRight)
								{
									if (_mouseLineAngle < _offsetToBreak)
									{
										Debug.Log("Test39");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " < Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
								else
								{
								

									if (_mouseLineAngle < _offsetToBreak)
									{
										Debug.Log("Test40");
										Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " < Mouse Offset: " + _offsetToBreak);
										detach = true;
									}
								}
							}
						}
					}

				}
				else
				{

					if (_isRopeAbove)
					{
						if (isMouseAbove)
						{
							if (ismouseRight)
							{
								if (_mouseLineAngle < _offsetToBreak)
								{
									Debug.Log("Test41");
									Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " < Mouse Offset: " + _offsetToBreak);
									detach = true;
								}
							}
							else
							{
								if (_mouseLineAngle < _offsetToBreak)
								{
									Debug.Log("Test42");
									Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " < Mouse Offset: " + _offsetToBreak);
									detach = true;
								}
							}
						}
						else
						{
							if (ismouseRight)
							{
								if (_mouseLineAngle < _offsetToBreak)
								{
									Debug.Log("Test43");
									Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " < Mouse Offset: " + _offsetToBreak);
									detach = true;
								}
							}
							else
							{
								if (_mouseLineAngle < _offsetToBreak)
								{
									Debug.Log("Test44");
									Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " < Mouse Offset: " + _offsetToBreak);
									detach = true;
								}
							}
						}
					}
					else
					{
						if (isMouseAbove)
						{
							if (ismouseRight)
							{
								if (_mouseLineAngle > _offsetToBreak)
								{
									Debug.Log("Test45");
									Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
									detach = true;
								}
							}
							else
							{
								if (_mouseLineAngle < _offsetToBreak)
								{
									Debug.Log("Test46");
									Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " < Mouse Offset: " + _offsetToBreak);
									detach = true;
								}
							}
						}
						else
						{
							if (ismouseRight)
							{
								if (_mouseLineAngle < _offsetToBreak)
								{
									Debug.Log("Test47");
									Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " < Mouse Offset: " + _offsetToBreak);
									detach = true;
								}
							}
							else
							{
								if (_mouseLineAngle > _offsetToBreak)
								{
									Debug.Log("Test48");
									Debug.LogError("Right Side Mouse Line Angle: " + _mouseLineAngle + " > Mouse Offset: " + _offsetToBreak);
									detach = true;
								}
							}
						}
					}

				}
			}
		}
	
	
		return detach;

	}

	public bool IsTheRopeOnTheRightAngleToConnect(Vector2 _firstPos, Vector2 _secondPos, Vector2 _mousePos, Vector2 _gripPos)
	{
		bool canConnect = false;

		float lastLineAngle = GetAngle(_firstPos, _secondPos);
		float mouseLineAngle = GetAngle(_secondPos, _mousePos);

		bool isOnRight = isRopeOnRightSide(_secondPos, _gripPos);
		bool isOnAbove = IsRopeAbove(_secondPos, _gripPos);

		float minAngle = 0;

		if (isOnRight)
		{
			if (isOnAbove)
			{
				minAngle = -10;
			}
			else
			{
				minAngle = 10;
			}
		}
		else
		{
			if (isOnAbove)
			{
				minAngle = 10;
			}
			else
			{
				minAngle = -10;
			}
		}

		float minToConnect = lastLineAngle + minAngle;


		if (isOnRight)
		{
			if (isOnAbove)
			{
				if (mouseLineAngle > minToConnect)
				{
					canConnect = true;
				}
			}
			else
			{
				if (mouseLineAngle < minToConnect)
				{
					canConnect = true;
				}
			}
		}
		else
		{
			if (isOnAbove)
			{
				if (mouseLineAngle < minToConnect)
				{
					canConnect = true;
				}
			}
			else
			{
				if (mouseLineAngle > minToConnect)
				{
					canConnect = true;
				}
			}
		}



		return canConnect;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.black;
		if (colliderPoints != null) colliderPoints.ForEach(p => Gizmos.DrawSphere(p, 0.1f));
	}

}
