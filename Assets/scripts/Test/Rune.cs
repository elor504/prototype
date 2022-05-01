using UnityEngine;

public class Rune : grip
{
	public Color useableColor;
	public Color disabledColor;
	SpriteRenderer sprite;

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
		useableCountLeft = useableCountStart;
		SetRuneGFX();
	}


	public virtual void UseRune()
	{
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
		if (isTurnedOff)
			sprite.color = disabledColor;
		else
			sprite.color = useableColor;
	}
}
