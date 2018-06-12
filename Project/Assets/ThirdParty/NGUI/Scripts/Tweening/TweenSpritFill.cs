using UnityEngine;

/// <summary>
/// Tween the Sprite Fill Value.
/// </summary>
[RequireComponent(typeof(UISprite))]
[AddComponentMenu("NGUI/Tween/Tween SpriteFill")]
public class TweenSpritFill : UITweener
{

	[Range(0f, 1f)]	public float from = 1f;
	[Range(0f, 1f)]	public float to = 1f;

	UISprite mSprite;

	public UISprite cachedSprite { get { if (mSprite == null) mSprite = GetComponent<UISprite>(); return mSprite; } }

	public float value { get { return cachedSprite.fillAmount; } set { cachedSprite.fillAmount = value; } }

	protected override void OnUpdate(float factor, bool isFinished)
	{
		value = Mathf.Lerp(from, to, factor);
	}

	static public TweenSpritFill Begin(UISprite sprite, float duration, int targetValue)
	{
		TweenSpritFill comp = UITweener.Begin<TweenSpritFill>(sprite.gameObject, duration);
		comp.from = sprite.fillAmount;
		comp.to = targetValue;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
	public override void SetStartToCurrentValue() { from = value; }
	public override void SetEndToCurrentValue() { to = value; }
}