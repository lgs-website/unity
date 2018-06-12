using UnityEngine;
using System.Collections.Generic;

public class UISpriteFilledAnimation : MonoBehaviour
{
	[SerializeField]
	protected int mFPS = 30;
	[SerializeField]
	protected bool mLoop = true;
	[SerializeField]
	protected int mCount = 10;

	protected UISprite mSprite;
	protected float mDelta = 0f;
	protected int mIndex = 0;
	protected bool mActive = true;

	/// <summary>
	/// Set the animation to be looping or not
	/// </summary>

	public bool loop { get { return mLoop; } set { mLoop = value; } }

	/// <summary>
	/// Returns is the animation is still playing or not
	/// </summary>

	public bool isPlaying { get { return mActive; } }
	protected virtual void Start() {
		if (mSprite == null)
			mSprite = GetComponent<UISprite>();
	}

	protected virtual void Update()
	{
		if (mActive && Application.isPlaying && mFPS > 0)
		{
			mDelta += RealTime.deltaTime;
			float rate = 1f / mFPS;

			if (rate < mDelta)
			{
				mDelta = (rate > 0f) ? mDelta - rate : 0f;

				if (++mIndex > mCount)
				{
					mIndex = 1;
					mActive = mLoop;
				}

				if (mActive)
				{
					mSprite.fillAmount = mIndex * 1f / mCount;
				}
			}
		}
	}

	/// <summary>
	/// Reset the animation to the beginning.
	/// </summary>

	public void Play() { mActive = true; }

	/// <summary>
	/// Pause the animation.
	/// </summary>

	public void Pause() { mActive = false; }
	/// <summary>
	/// Reset the animation to frame 0 and activate it.
	/// </summary>

	public void ResetToBeginning()
	{
		mActive = true;
		mIndex = 1;

		if (mSprite != null )
		{
			mSprite.fillAmount = mIndex * 1f / mCount;
		}
	}
}