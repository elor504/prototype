using UnityEngine;

public class Rune : grip
{
	public bool isFragile;
	bool canBeUsed;
	public bool getCanBeUsed => canBeUsed;
	public Color useableColor;
	public Color disabledColor;

	[Header("VFX")]
	public GameObject IdleVFX;
	public GameObject AttachedVFX;
	public GameObject gfxMask;

	SpriteRenderer sprite;


	[Header("Ignore, old settings")]
	[SerializeField]
	private int useableCountStart;

	[SerializeField]
	private int useableCountLeft;

	public bool isTurnedOff => useableCountLeft <= 0;


	private void Awake()
	{
		useableCountLeft = 1;
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
		//if (isFragile)
		//{
		//	canBeUsed = false;
		//}
		//useableCountLeft--;
		//if(useableCountLeft <= 0){
		//	canBeUsed = false;
		//}

		IdleVFX.SetActive(true);
		AttachedVFX.SetActive(false);

		SetRuneGFX();
	}

	//in case in the future they want to charge a rune somehow
	public void RechargeRune()
	{
		useableCountLeft++;
		SetRuneGFX();
	}

	public void RuneAttachedVFX()
	{
		IdleVFX.SetActive(false);
		AttachedVFX.SetActive(true);

	}

	public void SetRuneGFX()
	{
		IdleVFX.SetActive(true);
		AttachedVFX.SetActive(false);

		if (sprite == null)
			return;
		if (!canBeUsed)
			gfxMask.SetActive(false);
		else
			gfxMask.SetActive(true);

	}
}
