using UnityEngine;

public class RopePosToMouse : MonoBehaviour
{
	private static RopePosToMouse _instance;
	public static RopePosToMouse getInstance => _instance;

	public RopeTest rope;

	public Vector2 startingPos;
	public Vector2 currentPos;

	[SerializeField] Transform playerPos;

	private void Awake()
	{
		if (_instance == null)
			_instance = this;
		else if (_instance != this)
			Destroy(this.gameObject);


		currentPos = startingPos;
		playerPos.position = startingPos;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			rope.SetRopeActive(true);
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = Camera.main.nearClipPlane;
			playerPos.GetComponent<Rigidbody2D>().MovePosition(Camera.main.ScreenToWorldPoint(mousePos));
			//playerPos.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
		}

		if (Input.GetMouseButtonUp(0))
		{
			rope.SetRopeActive(false);
			if (rope.hasRuneAttached())
			{
				UpdateCurrentPos(rope.GetLastRunePosition());
				ReturnToCurrentPos();
				rope.ResetRope();
			}
			else
			{
				ResetCurrentSpot();
				ReturnToCurrentPos();
				rope.ResetRope();
			}

			
		}


	}
	void ResetCurrentSpot() => currentPos = startingPos;
	void UpdateCurrentPos(Vector2 newPos) => currentPos = newPos;
	void ReturnToCurrentPos() => playerPos.position = currentPos;

}
