using UnityEngine;

public class PickUpInteraction : ObjectInteraction
{
	public SpriteRenderer pickupSprite;
	public DoorInteraction door;

	public override void Awake()
	{
		base.Awake();
		InitPickUp();
	}
	public void InitPickUp()
	{
		isInteracted = false;
		pickupSprite.enabled = true;
	}
	public override void OnInteraction()
	{
		isInteracted = true;
		pickupSprite.enabled = false;
		door.OnGettingKey();
	}

	public override void OnTriggerEnter2D(Collider2D collision)
	{
		base.OnTriggerEnter2D(collision);
	}

}
