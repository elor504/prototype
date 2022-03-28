using UnityEngine;

public class RopePosToMouse : MonoBehaviour
{
	private static RopePosToMouse _instance;
	public static RopePosToMouse getInstance => _instance;

	public float mouseSpeed = 1f;
	public RopeRenderer rope;
	public Transform startingPos;
	public Vector2 currentPos;
	public LayerMask platformMask;
	[SerializeField] Transform playerPos;
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
		//if (Input.GetMouseButton(0))
		//{

		//	Vector3 mousePos = Input.mousePosition;
		//	mousePos.z = Camera.main.nearClipPlane;

		//	Vector2 target = Camera.main.ScreenToWorldPoint(mousePos);
		//	RaycastHit2D hit = Physics2D.Linecast(rope.GetBeforeLastPosition(), target);
		//	//if (hit.collider != null)
		//	//{
		//	//	this.mousePos.position = hit.point;
		//	//}
		//	//else
		//	//{
		//		this.mousePos.position = target;
		//	//}
		//	rope.SetRopeActive(true, playerPos.position);
		//}

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

	private void FixedUpdate()
	{
		if (Input.GetMouseButton(0))
		{

			Vector3 mousePos = Input.mousePosition;
			mousePos.z = Camera.main.nearClipPlane;

			Vector2 target = Camera.main.ScreenToWorldPoint(mousePos);
			RaycastHit2D hit;
			if (Physics2D.Linecast(rope.GetBeforeLastPosition(), target, platformMask))
			{
				hit = Physics2D.Linecast(rope.GetBeforeLastPosition(), target, platformMask);
				this.mousePos.GetComponent<Rigidbody2D>().MovePosition(hit.point);
			}
			else
			{
				this.mousePos.GetComponent<Rigidbody2D>().MovePosition(target);
			}


			rope.SetRopeActive(true, playerPos.position);
		}
	}

	public void ResetCurrentSpot() => currentPos = startingPos.position;
	public void UpdateCurrentPos(Vector2 newPos)
	{
		currentPos = newPos;
		rope.SetPlayerPosition(currentPos);
	}
	public void ReturnToCurrentPos() => mousePos.position = currentPos;
	public void SetMousePos(Vector2 _pos) => mousePos.position = _pos;
}
