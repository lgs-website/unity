using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UISimpleGrid : MonoBehaviour
{
	public int MaxPerLine
	{
		get
		{
			return m_MaxPerLine;
		}
	}

	public float CellWidth
	{
		get
		{
			return m_CellWidth;
		}
	}

	public float CellHeight
	{
		get
		{
			return m_CellHeight;
		}
	}

	public List<GameObject> GridItems
	{
		get
		{
			return m_GridItems;
		}
	}

	public enum Arrangement
	{
		Horizontal,
		Vertical,
	}
	[SerializeField] Arrangement m_Arrangement = Arrangement.Horizontal;

	[SerializeField] int m_MaxPerLine = 0;

	[SerializeField] float m_CellWidth = 200f;
	[SerializeField] float m_CellHeight = 200f;

	List<GameObject> m_GridItems = new List<GameObject>();

	public void AddItemToGrid(GameObject go)
	{
		AddItemToGrid( go, Vector3.zero );
	}

	public void AddItemToGrid(GameObject go, Vector3 offset)
	{
		int nCount = m_GridItems.Count;
		go.transform.parent = transform;
		go.SetActive(true);
		Vector3 s = go.transform.localScale;
		go.transform.localScale = Vector3.Scale( s, transform.lossyScale );

		int x = nCount;
		int y = 0;
		if ( m_MaxPerLine > 0 )
		{
			x = nCount % m_MaxPerLine;
			y = nCount / m_MaxPerLine;
		}

		go.transform.localPosition = ( ( m_Arrangement == Arrangement.Horizontal ) ?
			new Vector3( m_CellWidth * x, -m_CellHeight * y, 0f ) :
			new Vector3( m_CellWidth * y, -m_CellHeight * x, 0f ) );
		go.transform.localPosition += offset;

		m_GridItems.Add( go );
	}

	public void DestoryItems()
	{
		foreach ( GameObject go in m_GridItems )
		{
			if ( go != null )
			{
				go.transform.parent = null;
				Destroy( go );
			}
		}

		m_GridItems.Clear();
	}
}
