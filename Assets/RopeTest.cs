using System.Collections.Generic;
using UnityEngine;

public class RopeTest : MonoBehaviour
{
	[SerializeField] Transform mousePosition;


	[SerializeField] LineRenderer rope;
	[SerializeField] LayerMask collMask;

	public List<Vector3> ropePositions { get; set; } = new List<Vector3>();
	public List<grip> grips;

	public bool isActive;

	private void Awake() => AddPosToRope(Vector3.zero);

	private void Update()
	{
		if (isActive)
		{
			UpdateRopePositions();
			LastSegmentGoToPlayerPos();

			DetectRopeCollision();

			if (ropePositions.Count > 2)
			{
				DetectCollisionExits();
			}
		}
	}

	public void SetRopeActive(bool _isActive)
	{
		isActive = _isActive;

		rope.enabled = _isActive;

	}

	void DetectRopeCollision()
	{
		grip hit;
		if (Physics2D.Linecast(mousePosition.position, rope.GetPosition(ropePositions.Count - 2), collMask))
		{
			hit = Physics2D.Linecast(mousePosition.position, rope.GetPosition(ropePositions.Count - 2), collMask).collider.gameObject.GetComponent<grip>();
			if (hit.getIsBeingUsed && grips.Find(x => x.gameObject == hit.gameObject))
				return;

			hit.Attach(true);
			ropePositions.RemoveAt(ropePositions.Count - 1);
			AddPosToRope(hit.transform.position);
			grips.Add(hit);
			if(hit as Rune)
			{
				Rune hittedRune = hit.GetComponent<Rune>();
				hittedRune.isTurnedOff = true;
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
		Debug.Log("Direction: " + angle);

		if(grips.Count != 0)
		{
			grip lastGrip = grips[grips.Count - 1];
			if(lastGrip as Rune)
			{
				return;
			}



			if (lastGrip.IsOnBreakableAngleRange(angle))
			{
				Debug.LogError("Detach");
				DetachRope();
			}
		}


		//if (angle >= 5)
		//{
		//	Debug.Log("Exit ");
		//	ropePositions.RemoveAt(ropePositions.Count - 2);
		//}

	}
	private void DetachRope()
	{
		ropePositions.RemoveAt(ropePositions.Count - 2);
		grips[grips.Count - 1].Attach(false);
		grips.RemoveAt(grips.Count - 1);
	}
	private void UpdateRopePositions()
	{
		rope.positionCount = ropePositions.Count;
		rope.SetPositions(ropePositions.ToArray());
	}
	private void LastSegmentGoToPlayerPos() => rope.SetPosition(rope.positionCount - 1, mousePosition.position);

	public void ResetRope()
	{
		ropePositions.Clear();

		for (int i = 0; i < grips.Count; i++)
		{
			grips[i].Attach(false);
		}
		grips.Clear();
		AddPosToRope(RopePosToMouse.getInstance.currentPos);
	}

	public bool hasRuneAttached()
	{
		if (grips[0] as Rune)
		{
			return true;
		}
		for (int i = grips.Count -1; i > 0; i--)
		{
			if(grips[i] as Rune)
			{
				return true;
			}
		}
		return false;
	}
	public Vector2 GetLastRunePosition()
	{
		if (grips[0] as Rune)
		{
			return grips[0].GetGripPosition;
		}
		for (int i = grips.Count - 1; i > 0; i--)
		{
			if (grips[i] as Rune)
			{
				return grips[i].GetGripPosition;
			}
		}
		return RopePosToMouse.getInstance.startingPos;
	}
	
}
