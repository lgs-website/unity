using UnityEngine;

/// <summary>
/// Tween the label value by integer.
/// </summary>

[RequireComponent(typeof(UILabel))]
[AddComponentMenu("NGUI/Tween/Tween LabelInteger")]
public class TweenLabelInteger : UITweener
{
	public int from = 100;
	public int to = 100;
	public bool bold = false;

	UILabel mLbl;
	int curValue = 0;
	const string strBoldFormat = "[b]{0}[/b]";
	public UILabel cachedLabel { get { if (mLbl == null) mLbl = GetComponent<UILabel>(); return mLbl; } }

	public int Value {
		set 
		{
			curValue = value;
			if (bold == false)
				cachedLabel.text = value.ToString();
			else
				cachedLabel.text = string.Format(strBoldFormat, value);
		}
		get
		{
			return curValue;
		}
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		Value = Mathf.RoundToInt(from * (1f - factor) + to * factor);
	}

	static public TweenLabelInteger Begin(GameObject go, float duration, int target)
	{
		TweenLabelInteger comp = UITweener.Begin<TweenLabelInteger>(go, duration);
		comp.from = 0;
		comp.to = target;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
}