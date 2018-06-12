using UnityEngine;


[ExecuteInEditMode]
public class UIOverlappedContents : MonoBehaviour
{
	[SerializeField] UIOverlappedGrid m_OverlappedGrid;

	void Start()
	{
		if ( m_OverlappedGrid == null )
		{
			m_OverlappedGrid = NGUITools.FindInParents<UIOverlappedGrid>( gameObject );
		}
	}

	/// <summary>
	/// Press the object.
	/// </summary>

	void OnPress(bool pressed)
	{
		if ( enabled && gameObject.activeSelf && m_OverlappedGrid != null )
		{
			m_OverlappedGrid.Press( pressed );
		}
	}

	/// <summary>
	/// Drag the object along the plane.
	/// </summary>

	void OnDrag(Vector2 delta)
	{
		if ( enabled && gameObject.activeSelf && m_OverlappedGrid != null )
		{
			m_OverlappedGrid.Drag( delta );
		}
	}
}
