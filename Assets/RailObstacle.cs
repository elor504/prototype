using System.Collections.Generic;
using UnityEngine;

public class RailObstacle : MonoBehaviour
{
	public float movementSpeed;
	public Obstacle obstacle;
	Rigidbody2D obstacleRB;
	public List<Transform> points;
	int maxIndex => points.Count - 1;
	public int currentIndex;

	Vector2 startPos;


	private void Awake()
	{
		currentIndex = 0;
		startPos = obstacle.transform.position;
		obstacleRB = obstacle.GetComponent<Rigidbody2D>();
	}
	// Update is called once per frame
	void Update()
	{
		MoveObstacle();

	}


	void MoveObstacle()
	{
		if (Vector2.Distance(obstacle.transform.position, points[currentIndex].position) > 0.1f)
		{
			Vector2 dir = ((Vector2)points[currentIndex].position - (Vector2)obstacle.transform.position).normalized;
			obstacleRB.MovePosition(obstacleRB.position + dir * Time.deltaTime * movementSpeed);
		}
		else
		{
			currentIndex++;
			if (currentIndex > maxIndex)
			{
				currentIndex = 0;
			}
		}
	}


	public void ResetRail()
	{
		currentIndex = 0;
		obstacle.transform.position = startPos;
	}
}
