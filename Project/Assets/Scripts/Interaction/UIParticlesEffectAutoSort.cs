using UnityEngine;
using System.Collections.Generic;

public class UIParticlesEffectAutoSort : MonoBehaviour
{
	UIPanel panel;
	List<Material> mats = new List<Material>();
	int beginRenderQ = -1, totalRenderQ = 0;
	void Start()
	{
		panel = gameObject.GetComponentInParent<UIPanel>();
		if(panel == null)
		{
			enabled = false;
		}
		else
		{
			Renderer[] rds = gameObject.GetComponentsInChildren<Renderer>(true);
			for(int i = 0; i < rds.Length; ++i)
			{
				for (int j = 0; j < rds[i].sharedMaterials.Length; ++j )
				{
					if (rds[i].sharedMaterials[j] != null)
						mats.Add(rds[i].sharedMaterials[j]);
				}
			}
			mats.Sort(delegate(Material a, Material b) 
			{
				return a.renderQueue.CompareTo(b.renderQueue);
			});
			panel.uiEffectList.Add(this);
		}
	}
	void OnDestroy()
	{
		if(panel != null)
			panel.uiEffectList.Remove(this);
	}
	public int UpdateRendererQueue(int rq)
	{
		if (panel == null)
			return 0;
		if(mats.Count > 0)
		{
			if(beginRenderQ != rq)
			{
				beginRenderQ = rq;
				int tempRenderQ = -1, addRenderQ = 0;
				for (int i = 0; i < mats.Count; ++i)
				{
					if (mats[i] == null)
						continue;
					if (tempRenderQ != mats[i].renderQueue)
					{
						tempRenderQ = mats[i].renderQueue;
						++addRenderQ;
					}
					mats[i].renderQueue = beginRenderQ + addRenderQ;
				}
				totalRenderQ = addRenderQ + 1;
			}
			return totalRenderQ;
		}
		return 0;
	}
}