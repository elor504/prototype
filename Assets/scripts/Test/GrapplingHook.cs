using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GrapplingHook : MonoBehaviour
{
	public LineRenderer lineRenderer;

	public List<Vector2> linePos = new List<Vector2>();
	public Transform mousePosition;





	private void AddPosToRope(Vector3 _pos)
	{
		linePos.Add(_pos);
		linePos.Add(mousePosition.position); //Always the last pos must be the player
	}
}