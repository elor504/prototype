using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
	public Transform player;

	public LineRenderer rope;
	public LayerMask collMask;

	public List<Vector3> ropePositions { get; set; } = new List<Vector3>();

	private void Awake() => AddPosToRope(Vector3.zero);

	private void Update()
	{
		UpdateRopePositions();
		LastSegmentGoToPlayerPos();

		DetectCollisionEnter();
		if (ropePositions.Count > 2) DetectCollisionExits();
	}

	private void DetectCollisionEnter()
	{
		RaycastHit2D hit;
		if (Physics2D.Linecast(player.position, rope.GetPosition(ropePositions.Count - 2), collMask))
		{
			hit = Physics2D.Linecast(player.position, rope.GetPosition(ropePositions.Count - 2), collMask);
			ropePositions.RemoveAt(ropePositions.Count - 1);
			AddPosToRope(hit.point);
		}
	}

	private void DetectCollisionExits()
	{
		//RaycastHit2D hit;
		if (!Physics2D.Linecast(player.position, rope.GetPosition(ropePositions.Count - 3), collMask))
		{
			ropePositions.RemoveAt(ropePositions.Count - 2);
		}

		//Vector2 lastpos = ropePositions[ropePositions.Count - 1];
		//Vector2 previousPos = ropePositions[ropePositions.Count - 2];

		//float angle = Vector2.Angle(previousPos, lastpos);

		//if (angle >= 5)
		//{
		//	Debug.Log("Exit ");
		//	ropePositions.RemoveAt(ropePositions.Count - 1);
		//}
		//Debug.Log("Direction: " + angle);
	}

	private void AddPosToRope(Vector3 _pos)
	{
		if (ropePositions.Count <= 2)
		{
			ropePositions.Add(_pos);
			ropePositions.Add(player.position); //Always the last pos must be the player
		}
		else if (_pos != ropePositions[ropePositions.Count - 1])
		{
			ropePositions.Add(_pos);
			ropePositions.Add(player.position); //Always the last pos must be the player
		}
	}

	private void UpdateRopePositions()
	{
		rope.positionCount = ropePositions.Count;
		rope.SetPositions(ropePositions.ToArray());
	}

	private void LastSegmentGoToPlayerPos() => rope.SetPosition(rope.positionCount - 1, player.position);
}