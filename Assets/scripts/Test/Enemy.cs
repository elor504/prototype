using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
	SpriteRenderer spriteRenderer;
	public float movementSpeed;
	public Transform left, right;
	bool lookingRight;
	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	// Start is called before the first frame update
	void Start()
    {
		lookingRight = true;
		FlipSprite();
	}

    // Update is called once per frame
    void Update()
    {
		FlipMovement();
		enemyMovement();
	}
	void enemyMovement()
	{
		if (lookingRight)
		{
			rb.velocity = new Vector2(movementSpeed, 0);
		}
		else
		{
			rb.velocity = new Vector2(-movementSpeed, 0);
		}
	}
	void FlipMovement()
	{
		if(!lookingRight && this.transform.position.x < left.position.x)
		{
			lookingRight = true;
			FlipSprite();
		}
		else if(lookingRight && this.transform.position.x > right.position.x)
		{
			lookingRight = false;
			FlipSprite();
		}
	}
	void FlipSprite()
	{
		spriteRenderer.flipX = lookingRight;

	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Player")
		{
            GameManager.getInstance.LoseScreen();
		}
	}
}

