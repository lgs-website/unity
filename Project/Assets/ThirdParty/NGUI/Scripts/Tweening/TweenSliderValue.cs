using UnityEngine;

[RequireComponent(typeof(UISlider))]
[AddComponentMenu("NGUI/Tween/Tween SliderValue")]
public class TweenSliderValue : UITweener
{
	[Range(0f, 1f)]
	public float from = 1f;
	[Range(0f, 1f)]
	public float to = 1f;

	UISlider mSlider;
	public UISlider cachedSlider { get { if (mSlider == null) mSlider = GetComponent<UISlider>(); return mSlider; } }

	public float value { get { return cachedSlider.value; } set { cachedSlider.value = value; } }

	protected override void OnUpdate(float factor, bool isFinished)
	{
		value = Mathf.Lerp(from, to, factor);
	}

	public override void SetStartToCurrentValue() { from = value; }
	public override void SetEndToCurrentValue() { to = value; }
}