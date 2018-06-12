using UnityEngine;
using System.Collections;


/// <summary>
/// SendMessage can't be received by a disabled object. Use abstract class instead.
/// </summary>
public abstract class UIOverlappedCell : MonoBehaviour
{
	protected UIOverlappedGrid mParentGrid = null;

	protected int mCellIndex = 0;

	public abstract void OnCellCreate(UIOverlappedGrid parent,int nCellIndex);
	public abstract void OnCellSelect(int nCellIndex);
	public abstract void OnCellRefresh(int nCellIndex);

	public abstract void OnCellShow(int nCellIndex);
	public abstract void OnCellHide();
}
