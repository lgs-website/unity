using UnityEngine;
using System.Collections.Generic;


public class UIOverlappedGrid : MonoBehaviour
{
	public CellMode Mode
	{
		get
		{
			return m_CellMode;
		}
	}

	public bool Sliding
	{
		get
		{
			return mSlide;
		}
	}
	
	public bool Pressed
	{
		get
		{
			return mPressed;
		}
	}

	/// <summary>
	/// Overlapped cells' position.
	/// </summary>
	[SerializeField] Vector3 m_OverlapCenter = Vector3.zero;

	/// <summary>
	/// Drag scale in axes, to fix the draggable direction.
	/// </summary>
	public enum DragMode
	{
		Horizontal,
		Vertical,
	}
	[SerializeField] DragMode m_DragMode = DragMode.Horizontal;

	/// <summary>
	/// The cell reused mode. The cell in list will be reused when Dynamic.
	/// </summary>
	public enum CellMode
	{
		Static,
		Dynamic,
	}
	[SerializeField] CellMode m_CellMode = CellMode.Static;

	/// <summary>
	/// Scroll show setting.
	/// </summary>
	public enum ScrollShow
	{
		None,
		Always,
		Dragging,
	}
	[SerializeField] ScrollShow m_ScrollShow = ScrollShow.None;

	/// <summary>
	/// Scroll mode setting.
	/// </summary>
	public enum ScrollMode
	{
		Horizontal,
		Vertical,
	}
	[SerializeField] ScrollMode m_ScrollMode = ScrollMode.Horizontal;

	/// <summary>
	/// Scroll start position.
	/// </summary>
	[SerializeField] Transform m_ScrollPos = null;

	/// <summary>
	/// Scroll local scale in moving direction.
	/// </summary>
	[SerializeField] float m_ScrollLen = 0f;

	/// <summary>
	/// Scroll sprite.
	/// </summary>
	[SerializeField] UISprite m_ScrollSprite = null;

	/// <summary>
	/// Distance from the overlappes center to bounds.
	/// </summary>
	[SerializeField] float m_BoundsDis = 200f;

	/// <summary>
	/// Extra depth the lower cell has.
	/// </summary>
	[SerializeField] float m_ExtraDepth = 0.1f;

	/// <summary>
	/// Distance must be met for switching.
	/// </summary>
	[SerializeField] float m_SwitchDis = 50f;

	/// <summary>
	/// Distance must be met for switching.
	/// </summary>
	[SerializeField] float m_Speed = 20f;

	/// <summary>
	/// How much momentum gets applied when the press is released after dragging.
	/// </summary>
	[SerializeField] float m_Inertia = 0.3f;

	/// <summary>
	/// Whether zoom effect when cell on show.
	/// </summary>
	[SerializeField] bool m_Zoom = false;
	[SerializeField] float m_ZoomOnShow = 0.1f;

	/// <summary>
	/// Whether color effect when cell on show.
	/// </summary>
	[SerializeField] bool m_Alpha = false;
	[SerializeField] float m_AlphaOnShow = 0.1f;

	/// <summary>
	/// Prefab to be instantiate in cell.
	/// </summary>
	[SerializeField] GameObject m_CellPrefab = null;

	/// <summary>
	/// Whether the grid has been initialized.
	/// </summary>
	bool mInitialized = false;

	/// <summary>
	/// The current cell index in cell list, is an analog value when cell mode is Dynamic.
	/// </summary>
	int mCurrentIndex = 0;
	LinkedListNode<GameObject> mCurrentCell = null;

	/// <summary>
	/// The current cell index in cell list, is an analog value when cell mode is Dynamic.
	/// </summary>
	int mPreparedIndex = 0;
	LinkedListNode<GameObject> mPreparedCell = null;

	/// <summary>
	/// The cell list count ought to be, is an analog value when cell mode is Dynamic.
	/// </summary>
	int mCellCount = 0;

	/// <summary>
	/// Whether the cell index and count need a sync.
	/// </summary>
	bool mNeedSync = true;

	LinkedList<GameObject> mCellList = new LinkedList<GameObject>();

	Plane mPlane;

	int mTouches = 0;

	Vector3 mSelectCenter = Vector3.zero;

	Vector3 mDragScale = Vector3.zero;

	Vector3 mInertiaOffset = Vector3.zero;
	
	bool mPressed = false;
	bool mSlide = false;
	Vector3 mSlidePos = Vector3.zero;
	Vector3 mSlideSpeed = Vector3.zero;

	int mScrollPart = 0;
	int mScrollTotal = 0;
	Vector3 mScrollOffset = Vector3.zero;
	Vector3 mScrollScale = Vector3.zero;

	Vector3 mLastHintPos = Vector3.zero;

    private Dictionary<UIWidget, float> m_AlphaWidgetDic = new Dictionary<UIWidget, float>();
    private Dictionary<GameObject, Vector3> m_ZoomCellDic = new Dictionary<GameObject, Vector3>();

	enum SwitchDirection
	{
		None,
		Negative,
		Positive,
	}

	void Awake()
	{
		mSelectCenter = m_OverlapCenter;
		mSelectCenter.z += m_ExtraDepth;

		mDragScale = ( m_DragMode == DragMode.Horizontal ? Vector3.right : Vector3.up );

		mSlidePos = mSelectCenter;
		mSlideSpeed = ( m_DragMode == DragMode.Horizontal ? Vector3.right : Vector3.down ) * m_Speed;

		// calculate scroll world offset
		Vector3 offset = Vector3.zero;
		if ( m_ScrollMode == ScrollMode.Horizontal )
		{
			offset.x = m_ScrollLen;
		}
		else
		{
			offset.y = m_ScrollLen;
		}

		mScrollOffset = transform.TransformPoint( offset ) - transform.TransformPoint( Vector3.zero );

		// calculate scroll local scale
		Vector3 scale = offset;
		Transform scrollParent = m_ScrollSprite.transform.parent;
		if ( scrollParent != transform )
		{
			scale = scrollParent.InverseTransformPoint( mScrollOffset ) - scrollParent.InverseTransformPoint( Vector3.zero );
		}

		if ( m_ScrollMode == ScrollMode.Horizontal )
		{
			scale.y = m_ScrollSprite.transform.localScale.y;
			scale.z = m_ScrollSprite.transform.localScale.z;
		}
		else
		{
			scale.x = m_ScrollSprite.transform.localScale.x;
			scale.z = m_ScrollSprite.transform.localScale.z;
		}

		mScrollScale = scale;
	}

	void Start()
	{
		ZoomSrcoll( mScrollTotal );
		PositionScroll( mScrollPart, mScrollTotal );

		m_ScrollSprite.alpha = 0f;
	}

	void Update()
	{
		if ( mSlide && mCurrentCell != null && mCurrentCell.Value != null )
		{
			Vector3 offset = mSlidePos - mCurrentCell.Value.transform.localPosition;

			if ( !Mathf.Approximately( offset.magnitude, 0f ) )
			{
				if ( mSlideSpeed.magnitude < offset.magnitude )
				{
					SwitchDirection direction = OffsetDirection( offset );
					if ( direction == SwitchDirection.Positive )
					{
						mCurrentCell.Value.transform.localPosition += mSlideSpeed;
					}
					else if ( direction == SwitchDirection.Negative )
					{
						mCurrentCell.Value.transform.localPosition -= mSlideSpeed;
					}
				}
				else
				{
					mCurrentCell.Value.transform.localPosition = mSlidePos;
				}

				if ( mPreparedCell != null && mPreparedCell.Value != null )
				{
					ZoomCell( mPreparedCell.Value, mCurrentCell.Value.transform.localPosition );
					AlphaCell( mPreparedCell.Value, mCurrentCell.Value.transform.localPosition );
				}
			}
			else
			{
                ResetCell();
				if ( mPreparedCell != null && mPreparedCell.Value != null )
				{
					SwitchDirection direction = TargetDirection( mSlidePos );
					if ( direction != SwitchDirection.None )
					{
						HideCell( mCurrentCell.Value );

						mCurrentCell = mPreparedCell;
						mCurrentIndex = mPreparedIndex;
						SelectCell( mCurrentCell.Value, mCurrentIndex );

						ResetPrepareCell( SwitchDirection.None );
					}
					else
					{
						HideCell( mPreparedCell.Value );
						ResetPrepareCell( SwitchDirection.None );
					}
				}

				mSlide = false;
				mSlidePos = mSelectCenter;
			}
		}

		UpdateScroll();

		// attenuate the inertia
		NGUIMath.SpringDampen( ref mInertiaOffset, 9f, 0.025f );
	}

	void OnCellFree()
	{
		if ( mCurrentCell != null && mCurrentCell.Value != null )
		{
			Vector3 srcPos = mCurrentCell.Value.transform.localPosition;
			Vector3 desPos = srcPos + mInertiaOffset;

			RestrictInBounds( ref desPos );
			Vector3 desOffset = desPos - mSelectCenter;

			if ( desOffset.magnitude >= m_SwitchDis )	// need to switch cell
			{
				SwitchDirection srcDirection = TargetDirection( srcPos );
				SwitchDirection desDirection = TargetDirection( desPos );

				if ( srcDirection == desDirection )
				{
					mSlidePos = SwitchPosition( desDirection );
				}
				else
				{
					if ( srcDirection == SwitchDirection.None && CanSwitchDirection( desDirection ) )
					{
						mSlidePos = SwitchPosition( desDirection );
					}
					else
					{
						mSlidePos = SwitchPosition( SwitchDirection.None );
					}
				}
			}
			else
			{
				mSlidePos = SwitchPosition( SwitchDirection.None );
			}

			mSlide = true;
		}

		mInertiaOffset = Vector3.zero;
	}

	void OnCellDrag(Vector3 offset)
	{
		if ( mCurrentCell != null && mCurrentCell.Value != null )
		{
			if ( !Mathf.Approximately( offset.magnitude, 0f ) )
			{
				Vector3 srcPos = mCurrentCell.Value.transform.localPosition;
				Vector3 desPos = srcPos + offset;
				RestrictInBounds( ref desPos );

				SwitchDirection srcDirection = TargetDirection( srcPos );
				SwitchDirection desDirection = TargetDirection( desPos );

				if ( srcDirection != desDirection )
				{
					// handle old prepared cell
					if ( mPreparedCell != null && mPreparedCell.Value != null )
					{
						HideCell( mPreparedCell.Value );
						ResetPrepareCell( SwitchDirection.None );
					}

					// set new prepared cell
					if ( desDirection != SwitchDirection.None )
					{
						ResetPrepareCell( desDirection );

						if ( mPreparedCell != null && mPreparedCell.Value != null )
						{
							// fufeng to do: get rotation according to the direction and the beyond offset, add to current rotation
							//Vector3 beyondOffset = desPos - mSelectCenter;
							//Quaternion rot = mCurrentCell.Value.transform.rotation;
							//rot.y += 0.1f;
							//TweenRotation.Begin( mCurrentCell.Value, 0.5f, rot );

							ShowCell( mPreparedCell.Value, mPreparedIndex );
						}
						else
						{
							// fufeng to do: get rotation according to the direction and the beyond offset, add to current rotation
							//Vector3 beyondOffset = desPos - mSelectCenter;
							//Quaternion rot = mCurrentCell.Value.transform.rotation;
							//rot.y += 0.1f;
							//TweenRotation.Begin( mCurrentCell.Value, 0.5f, rot );

							desDirection = SwitchDirection.None;
							desPos = SwitchPosition( SwitchDirection.None );
						}
					}
				}

				offset = desPos - srcPos;
				mCurrentCell.Value.transform.localPosition = desPos;

				if ( mPreparedCell != null && mPreparedCell.Value != null )
				{
					ZoomCell( mPreparedCell.Value, desPos );
					AlphaCell( mPreparedCell.Value, desPos );
				}

				mInertiaOffset += ( offset * m_Inertia );
			}
            else
            {
                ResetCell();
            }
		}
		else
		{
			mInertiaOffset = Vector3.zero;
		}
	}

	void RestrictInBounds(ref Vector3 pos)
	{
		if ( m_DragMode == DragMode.Horizontal )
		{
			float maxH = mSelectCenter.x + m_BoundsDis;
			float minH = mSelectCenter.x - m_BoundsDis;

			if ( pos.x > maxH )
			{
				pos.x = maxH;
			}
			else if ( pos.x < minH )
			{
				pos.x = minH;
			}
		}
		else
		{
			float maxV = mSelectCenter.y + m_BoundsDis;
			float minV = mSelectCenter.y - m_BoundsDis;

			if ( pos.y > maxV )
			{
				pos.y = maxV;
			}
			else if ( pos.y < minV )
			{
				pos.y = minV;
			}
		}
	}

	SwitchDirection TargetDirection(Vector3 pos)
	{
		if ( m_DragMode == DragMode.Horizontal )
		{
			if ( pos.x > mSelectCenter.x )
			{
				return SwitchDirection.Positive;
			}
			else if ( pos.x < mSelectCenter.x )
			{
				return SwitchDirection.Negative;
			}
		}
		else
		{
			if ( pos.y > mSelectCenter.y )
			{
				return SwitchDirection.Negative;
			}
			else if ( pos.y < mSelectCenter.y )
			{
				return SwitchDirection.Positive;
			}
		}

		return SwitchDirection.None;
	}

	SwitchDirection OffsetDirection(Vector3 offset)
	{
		if ( !Mathf.Approximately( offset.magnitude, 0f ) )
		{
			if ( m_DragMode == DragMode.Horizontal )
			{
				if ( offset.x > 0f )
				{
					return SwitchDirection.Positive;
				}
				else if ( offset.x < 0f )
				{
					return SwitchDirection.Negative;
				}
			}
			else
			{
				if ( offset.y > 0f )
				{
					return SwitchDirection.Negative;
				}
				else if ( offset.y < 0f )
				{
					return SwitchDirection.Positive;
				}
			}
		}

		return SwitchDirection.None;
	}

	Vector3 SwitchPosition(SwitchDirection direction)
	{
		Vector3 pos = mSelectCenter;

		if ( m_DragMode == DragMode.Horizontal )
		{
			if ( direction == SwitchDirection.Positive )
			{
				pos.x += m_BoundsDis;
			}
			else if ( direction == SwitchDirection.Negative )
			{
				pos.x -= m_BoundsDis;
			}
		}
		else
		{
			if ( direction == SwitchDirection.Positive )
			{
				pos.y -= m_BoundsDis;
			}
			else if ( direction == SwitchDirection.Negative )
			{
				pos.y += m_BoundsDis;
			}
		}

		return pos;
	}

	void ZoomCell(GameObject obj,Vector3 pos)
	{
		if ( obj != null && m_Zoom && !Mathf.Approximately( m_BoundsDis, 0f ) )
		{
            if(!m_ZoomCellDic.ContainsKey(obj))
            {
                m_ZoomCellDic.Add(obj, obj.transform.localScale);
            }

			Vector3 scale = Vector3.one;
			Vector3 cellOffset = pos - mSelectCenter;

			float factor = 1 - m_ZoomOnShow;
			factor *= ( cellOffset.magnitude / m_BoundsDis );
			factor += m_ZoomOnShow;

			scale.x *= factor;
			scale.y *= factor;

			obj.transform.localScale = scale;
		}
	}

	void AlphaCell(GameObject obj,Vector3 pos)
	{
		if ( obj != null && m_Alpha )
		{
			if ( !Mathf.Approximately( m_BoundsDis, 0f ) )
			{
				Vector3 cellOffset = pos - mSelectCenter;

				float factor = 1 - m_AlphaOnShow;
				factor *= ( cellOffset.magnitude / m_BoundsDis );
				factor += m_AlphaOnShow;

				UIWidget[] widgets = mPreparedCell.Value.GetComponentsInChildren<UIWidget>();
				for ( int i = 0; i < widgets.Length; ++i )
				{
                    UIWidget widget = widgets[i];
                    Color color = widget.color;
                    if (!m_AlphaWidgetDic.ContainsKey(widget))
                    {
                        m_AlphaWidgetDic.Add(widget, color.a);
                    }

					color.a = factor;
                    widget.color = color;
				}
			}
		}
	}

	void ZoomSrcoll(int scrollCount)
	{
		if ( scrollCount <= 0 ) scrollCount = 1;

		Vector3 scale = mScrollScale;
		if ( m_ScrollMode == ScrollMode.Horizontal )
		{
			scale.x = mScrollScale.x / scrollCount;
		}
		else
		{
			scale.y = mScrollScale.y / scrollCount;
		}

		m_ScrollSprite.transform.localScale = scale;
	}

	void PositionScroll(int scrollPart, int scrollCount)
	{
		if ( scrollCount <= 0 ) scrollCount = 1;
		if ( scrollPart < 0 ) scrollPart = 0;
		if ( scrollPart > scrollCount ) scrollPart = scrollCount -1;

		if ( m_ScrollMode == ScrollMode.Horizontal )
		{
			m_ScrollSprite.transform.position = m_ScrollPos.position + mScrollOffset * scrollPart / scrollCount;
		}
		else
		{
			m_ScrollSprite.transform.position = m_ScrollPos.position - mScrollOffset * scrollPart / scrollCount;
		}
	}

	void UpdateScroll()
	{
		bool bShow = false;

		if ( m_ScrollShow == ScrollShow.Always )
		{
			bShow = true;
		}
		else if ( m_ScrollShow == ScrollShow.Dragging )
		{
			if ( mSlide || mTouches > 0 )
			{
				bShow = true;
			}
		}

		if ( bShow )
		{
			if ( mScrollPart != mCurrentIndex || mScrollTotal != mCellCount )
			{
				mScrollPart = mCurrentIndex;
				mScrollTotal = mCellCount;

				ZoomSrcoll( mScrollTotal );
				PositionScroll( mScrollPart, mScrollTotal );
			}

			if ( !Mathf.Approximately( m_ScrollSprite.alpha, 1f ) )
			{
				float alpha = m_ScrollSprite.alpha;
				alpha += Time.deltaTime * 2f;
				alpha = Mathf.Clamp01( alpha );

				m_ScrollSprite.alpha = alpha;
			}
		}
		else
		{
			if ( !Mathf.Approximately( m_ScrollSprite.alpha, 0f ) )
			{
				float alpha = m_ScrollSprite.alpha;
				alpha -= Time.deltaTime * 2f;
				alpha = Mathf.Clamp01( alpha );

				m_ScrollSprite.alpha = alpha;
			}
		}
	}

	void ResetPrepareCell(SwitchDirection direction)
	{
		mPreparedCell = null;
		mPreparedIndex = -1;

		if ( CanSwitchDirection( direction ) )
		{
			if ( direction == SwitchDirection.Negative )
			{
				if ( mCurrentCell.Next != null )
				{
					mPreparedCell = mCurrentCell.Next;
					mPreparedIndex = mCurrentIndex + 1;
				}
				else
				{
					if ( m_CellMode == CellMode.Dynamic )
					{
						mPreparedCell = mCellList.First;
						mPreparedIndex = mCurrentIndex + 1;
					}
				}
			}
			else if ( direction == SwitchDirection.Positive )
			{
				if ( mCurrentCell.Previous != null )
				{
					mPreparedCell = mCurrentCell.Previous;
					mPreparedIndex = mCurrentIndex - 1;
				}
				else
				{
					if ( m_CellMode == CellMode.Dynamic )
					{
						mPreparedCell = mCellList.Last;
						mPreparedIndex = mCurrentIndex - 1;
					}
				}
			}
		}
	}

	bool CanSwitchDirection(SwitchDirection direction)
	{
		if ( direction == SwitchDirection.Negative )
		{
			if ( mNeedSync || mCurrentIndex < 0 || mCurrentIndex + 1 >= mCellCount )
			{
				return false;
			}
		}
		else if ( direction == SwitchDirection.Positive )
		{
			if ( mNeedSync || mCurrentIndex <= 0 )
			{
				return false;
			}
		}

		return true;
	}

	void ShowCell(GameObject obj, int index)
	{
		if ( obj != null )
		{
			obj.SetActive( true );

			UIOverlappedCell cell = obj.GetComponent<UIOverlappedCell>();
			if ( cell != null )
			{
				cell.OnCellShow( index );
			}
		}
	}

	void SelectCell(GameObject obj, int index)
	{
		if ( obj != null )
		{
			obj.transform.localPosition = mSelectCenter;

			UIOverlappedCell cell = obj.GetComponent<UIOverlappedCell>();
			if ( cell != null )
			{
				cell.OnCellSelect( index );
			}
		}
	}

	void RefreshCell(GameObject obj, int index)
	{
		if ( obj != null )
		{
			UIOverlappedCell cell = obj.GetComponent<UIOverlappedCell>();
			if ( cell != null )
			{
				cell.OnCellRefresh( index );
			}
		}
	}

	void HideCell(GameObject obj)
	{
		if ( obj != null )
		{
			obj.transform.localPosition = m_OverlapCenter;

			UIOverlappedCell cell = obj.GetComponent<UIOverlappedCell>();
			if ( cell != null )
			{
				cell.OnCellHide();
			}

			obj.SetActive( false );
		}
	}

	void AddCell()
	{
		if ( m_CellPrefab != null )
		{
			GameObject obj = (GameObject)Instantiate( m_CellPrefab, m_CellPrefab.transform.position, m_CellPrefab.transform.rotation );
			obj.transform.parent = transform;
			obj.SetActive(true);
			Vector3 scale = obj.transform.localScale;
			obj.transform.localScale = Vector3.Scale( scale, transform.lossyScale );
			obj.transform.localPosition = m_OverlapCenter;

			UIOverlappedCell cell = obj.GetComponent<UIOverlappedCell>();
			if ( cell != null )
			{
				cell.OnCellCreate( this, mCellList.Count );

				mCellList.AddLast( obj );
				++mCellCount;

				obj.SetActive( false );
			}
			else
			{
				Destroy( obj );
			}
		}
	}

	void StaticInit(int nCellCount, bool needInit)
	{
		mInitialized = true;
		mNeedSync = false;

		if (needInit)
		{
			if (nCellCount > 0)
			{
				for (int i = 0; i < nCellCount; ++i)
				{
					AddCell();
				}
			}
		}

		if ( mCellList.Count > 0 )
		{
			mCurrentIndex = 0;
			mCurrentCell = mCellList.First;

			if ( mCurrentCell != null && mCurrentCell.Value != null )
			{
				ShowCell( mCurrentCell.Value, mCurrentIndex );
				SelectCell( mCurrentCell.Value, mCurrentIndex  );
			}
		}
		else
		{
			Debug.LogWarning("UIOverlappedGrid StaticInit error.CellList can not be empty");
		}
	}

	void DynamicInit(int nCellCount, bool needInit)
	{
		mInitialized = true;
		mNeedSync = true;

		if (needInit)
		{
			if (nCellCount > 1)
			{
				for (int i = 0; i < nCellCount; ++i)
				{
					AddCell();
				}
			}
		}

		if ( mCellList.Count > 0 )
		{
			mCurrentIndex = 0;
			mCurrentCell = mCellList.First;

			if ( mCurrentCell != null && mCurrentCell.Value != null )
			{
				ShowCell( mCurrentCell.Value, mCurrentIndex );
				SelectCell( mCurrentCell.Value, mCurrentIndex  );
			}
		}
		else
		{
			Debug.LogWarning("UIOverlappedGrid DynamicInit error.CellList can not be empty");
		}
	}

	void ClearGrid(bool needClear)
	{
		mInitialized = false;
		mNeedSync = true;
		
		mCurrentIndex = 0;
		mPreparedIndex = 0;
		mCurrentCell = null;
		mPreparedCell = null;

		mSlide = false;
		mSlidePos = mSelectCenter;

        mTouches = 0;

		if (needClear)
		{
			mCellCount = 0;
			while (mCellList.Count > 0)
			{
				LinkedListNode<GameObject> cell = mCellList.Last;
				if (cell != null && cell.Value != null)
				{
					Destroy(cell.Value);
				}

				mCellList.RemoveLast();
			}
		}
		else
		{
			foreach (GameObject cell in mCellList)
			{
				if(cell != null)
				{
                    HideCell(cell);
				}
			}
		}

        ResetCell();
	}

    void ResetCell()
    {
        foreach (UIWidget widget in m_AlphaWidgetDic.Keys)
        {
            Color color = widget.color;
            color.a = m_AlphaWidgetDic[widget];
            widget.color = color;
        }

        foreach (GameObject go in m_ZoomCellDic.Keys)
        {
            go.transform.localScale = m_ZoomCellDic[go];
        }

        m_AlphaWidgetDic.Clear();
        m_ZoomCellDic.Clear();
    }

	public void SyncCell(int nCellIndex,int nCellCount)
	{
		if ( m_CellMode == CellMode.Dynamic )
		{
			mCurrentIndex = nCellIndex;
			mCellCount = nCellCount;

			mNeedSync = false;
		}
	}

	public void InitGrid(int nCellCount)
	{
		if ( mInitialized )
		{
			ClearGrid(true);
		}

		if ( m_CellMode == CellMode.Static )
		{
			StaticInit(nCellCount, true);
		}
		else
		{
			DynamicInit(nCellCount, true);
		}
	}

	/// <summary>
	/// This function must be used with InitGrid() function.
	/// </summary>
	public void ReInitGrid()
	{
		if ( mInitialized )
		{
			ClearGrid(false);
		}

		if (m_CellMode == CellMode.Static)
		{
			StaticInit(0, false);
		}
		else
		{
			DynamicInit(0, false);
		}
	}

	public void ShowGrid()
	{
		if ( mInitialized )
		{
			foreach ( GameObject cellValue in mCellList )
			{
				if ( mCurrentCell != null && mCurrentCell.Value == cellValue )
				{
					ShowCell( cellValue, mCurrentIndex );
				}
				else
				{
					HideCell( cellValue );
				}
			}
		}
	}

	public void RefreshCell()
	{
		mTouches = 0;
		if ( mInitialized && mCurrentCell != null && mCurrentCell.Value != null && !mSlide )
		{
			RefreshCell( mCurrentCell.Value, mCurrentIndex );
		}
	}

	public void Press(bool pressed)
	{
		if ( enabled && gameObject.activeSelf )
		{
			mPressed = pressed;
			
			mTouches += ( pressed ? 1 : -1 );
			if(mTouches < 0)
			{
				mTouches = 0;
			}

			if ( pressed )
			{
				// remove momentum on press
				mInertiaOffset = Vector3.zero;

				// remove slide
				mSlide = false;
				mSlidePos = mSelectCenter;

				// remember the hit position
				mLastHintPos = UICamera.lastHit.point;

				// create the plane to drag along
				mPlane = new Plane( transform.rotation * Vector3.back, mLastHintPos );
			}
			else
			{
				if ( mTouches == 0)
				{
					OnCellFree();
				}
			}
		}
	}

	public void Drag(Vector2 delta)
	{
		if ( enabled && gameObject.activeSelf )
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;

			Ray ray = UICamera.currentCamera.ScreenPointToRay( UICamera.currentTouch.pos );
			float distance = 0f;

			if ( mPlane.Raycast( ray, out distance ) )
			{
				Vector3 curPos = ray.GetPoint( distance );
				Vector3 curOffset = curPos - mLastHintPos;
				mLastHintPos = curPos;

				curOffset = transform.InverseTransformDirection( curOffset );
				curOffset.Scale( mDragScale );
				curOffset = transform.TransformDirection( curOffset );

				// convert offset to be in local.
				if ( !Mathf.Approximately( curOffset.magnitude, 0f ) )
				{
					curOffset = transform.InverseTransformPoint( curOffset );
					curOffset -= transform.InverseTransformPoint( Vector3.zero );

					OnCellDrag( curOffset );
				}
			}
		}
	}
	
	public void DragNext()
	{
		ResetPrepareCell(SwitchDirection.Negative);
		
		if ( mPreparedCell != null && mPreparedCell.Value != null )
		{
			ShowCell( mPreparedCell.Value, mPreparedIndex );
		}
	}
	
	public void DragPrevious()
	{
		ResetPrepareCell(SwitchDirection.Positive);
		
		if ( mPreparedCell != null && mPreparedCell.Value != null )
		{
			ShowCell( mPreparedCell.Value, mPreparedIndex );
		}
	}
}
