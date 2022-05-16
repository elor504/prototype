using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	public Transform platform;
	public Rigidbody2D platformRB;
	public float movementSpeed;
	public List<Transform> points = new List<Transform>();
	public int maxPoints => points.Count;

	int currentPoint;
	//public Dictionary<int,Vector2> moveCommand = new Dictionary<int, Vector2>();
	public List<Vector2> movePos = new List<Vector2>();



	Vector2 startPos;
	private void Awake()
	{
		startPos = platform.position;
		currentPoint = 1;
	}
	//private void Update()
	//{
	//	if(Input.GetKeyDown(KeyCode.D))
	//	{
	//		MovePlatform();
	//	}


		
	//}
	private void FixedUpdate()
	{
		if (movePos.Count > 0)
		{
			if (Vector2.Distance(platform.position, movePos[0]) > 0.1f)
			{
				Vector2 dir = (movePos[0] - (Vector2)platform.position).normalized;
				platformRB.MovePosition(platformRB.position + dir * Time.deltaTime * movementSpeed);
			}
			else
			{
				movePos.RemoveAt(0);
			}
		}
	}

	public void MovePlatform()
	{
		movePos.Add(points[currentPoint].position);
		currentPoint++;
		if(currentPoint > maxPoints -1)
		{
			currentPoint = 0;
		}

	}

	Vector2 GetPositionByPointsIndex()
	{
		return points[currentPoint].position;
	}
	public void ResetMovingPlatform()
	{
		movePos.Clear();
		currentPoint = 1;
		platform.position = startPos;
	}





}
