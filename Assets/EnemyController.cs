using UnityEngine;

public class EnemyController : MonoBehaviour
{
	public float movementSpeed;

	[Header("Ground sensor")]
	public LayerMask groundMask;
	public float groundRayLength;
	public Transform groundCheckTrans;
	private float groundCheckXPos;
	bool flip;

	private Rigidbody2D rb;
	private bool isLookingRight;
	private SpriteRenderer spriteRenderer;

	// Start is called before the first frame update
	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		groundCheckXPos = groundCheckTrans.localPosition.x;
	}

	// Update is called once per frame
	void Update()
	{
		MoveEnemy();


		if (!GroundCheck())
		{
			Flip();
		}
	}

	void MoveEnemy()
	{
		if (isLookingRight)
		{
			rb.velocity = new Vector2(movementSpeed * Time.deltaTime, rb.velocity.y);
		}
		else
		{
			rb.velocity = new Vector2(-movementSpeed * Time.deltaTime, rb.velocity.y);
		}
	}

	public bool GroundCheck()
	{
		return Physics2D.Raycast(this.groundCheckTrans.position, new Vector2(0, -1), groundRayLength, groundMask);
	}

	void Flip()
	{
		isLookingRight = !isLookingRight;
		spriteRenderer.flipX = isLookingRight;
		if (isLookingRight)
		{
			groundCheckTrans.localPosition =  new Vector2(groundCheckXPos * -1, groundCheckTrans.localPosition.y);
		}
		else
		{
			groundCheckTrans.localPosition = new Vector2(groundCheckXPos, groundCheckTrans.localPosition.y);
		}
	}


	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawRay(this.groundCheckTrans.position, new Vector2(0, -1) * groundRayLength);
	}



}
