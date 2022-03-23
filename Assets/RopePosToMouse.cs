using UnityEngine;

public class RopePosToMouse : MonoBehaviour
{
	private static RopePosToMouse _instance;
	public static RopePosToMouse getInstance => _instance;

	public RopeTest rope;
	public Transform startingPos;
	public Vector2 currentPos;

	[SerializeField] Transform mousePos;

	private void Awake()
	{
		if (_instance == null)
			_instance = this;
		else if (_instance != this)
			Destroy(this.gameObject);


		currentPos = startingPos.position;
		mousePos.position = startingPos.position;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButton(0))
		{

			Vector3 mousePos = Input.mousePosition;
			mousePos.z = Camera.main.nearClipPlane;
			rope.SetRopeActive(true, currentPos);
			this.mousePos.GetComponent<Rigidbody2D>().MovePosition(Camera.main.ScreenToWorldPoint(mousePos));

		}

		if (Input.GetMouseButtonUp(0))
		{
			rope.SetRopeActive(false, currentPos);
			if (rope.hasRuneAttached())
			{
				UpdateCurrentPos(rope.GetLastRunePosition());
				ReturnToCurrentPos();
				rope.ResetRope();
			}
			else
			{
				if (rope.currentRune == null)
					ResetCurrentSpot();
				ReturnToCurrentPos();
				rope.ResetRope();
			}


		}


	}
	void ResetCurrentSpot() => currentPos = startingPos.position;
	void UpdateCurrentPos(Vector2 newPos)
	{
		currentPos = newPos;
		rope.SetPlayerPosition(currentPos);
	}
	void ReturnToCurrentPos() => mousePos.position = currentPos;

}
