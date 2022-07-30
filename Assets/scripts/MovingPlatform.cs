using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	public Transform platform;
	public Rigidbody2D platformRB;
	public float movementSpeed;
	public List<Transform> points = new List<Transform>();

	[Header("\nLine Arrow Materials")]
	[SerializeField] private LineRenderer lineRend;
	[SerializeField] private Material matRight;
	[SerializeField] private Material matLeft;

	public int maxPoints => points.Count;

	int currentPoint;
	//public Dictionary<int,Vector2> moveCommand = new Dictionary<int, Vector2>();
	public List<Vector2> movePos = new List<Vector2>();



	Vector2 startPos;

    public bool arrowLinesAreRight = true;

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
				// HERE ********************


				if (arrowLinesAreRight)
				{
					lineRend.material = matLeft;
					arrowLinesAreRight = false;

					//Debug.LogError("IMMA SCHA ZONA");

				}
				else
				{
					lineRend.material = matRight;
					arrowLinesAreRight = true;

				}

				movePos.RemoveAt(0);
				if (movePos.Count <= 0)
                {
				
					if (!GameManager.getInstance.debugSoundModeOn) AudioHandler.GetInstance.PlaySoundGameplayPlatformMovement(false);
				}
			}
		}
	}

	public void MovePlatform()
	{
		movePos.Add(points[currentPoint].position);
		currentPoint++;
		if (!GameManager.getInstance.debugSoundModeOn) AudioHandler.GetInstance.PlaySoundGameplayPlatformMovement(true);
		if (currentPoint > maxPoints -1)
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
