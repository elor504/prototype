using UnityEngine;

public class Rune : grip
{
	public bool isFragile;
	bool canBeUsed;
	public bool getCanBeUsed => canBeUsed;
	public Color useableColor;
	public Color disabledColor;
	SpriteRenderer sprite;


	[Header("Ignore, old settings")]
	[SerializeField]
	private int useableCountStart;

	[SerializeField]
	private int useableCountLeft;

	public bool isTurnedOff => useableCountLeft <= 0;


	private void Awake()
	{
		sprite = GetComponentInChildren<SpriteRenderer>();
		InitRune();
	}

	public virtual void InitRune()
	{
		canBeUsed = true;
		useableCountLeft = useableCountStart;
		SetRuneGFX();
	}


	public virtual void UseRune()
	{
		if (isFragile)
		{
			canBeUsed = false;
		}
		useableCountLeft--;
		SetRuneGFX();
	}

	//in case in the future they want to charge a rune somehow
	public void RechargeRune()
	{
		useableCountLeft++;
		SetRuneGFX();
	}


	void SetRuneGFX()
	{
		if (!canBeUsed)
			sprite.color = disabledColor;
		else
			sprite.color = useableColor;
	}
}
