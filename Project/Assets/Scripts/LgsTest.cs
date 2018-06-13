using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LgsTest : MonoBehaviour
{
    [SerializeField]
    GameObject oldBG;
    [SerializeField]
    UITexture texBG;

    void OnGUI()
    {
        if (GUI.Button(new Rect(20, 20, 100, 50), "CaptureCamera"))
            CaptureCamera();

        if (GUI.Button(new Rect(20, 130, 100, 50), "GaussianBlur"))
            StartCoroutine(CaptureTexture());
    }

    Texture tmpBgTex = null;
    IEnumerator CaptureTexture()
    {
        RapidBlurEffect.RenderStart();
        yield return new WaitForEndOfFrame();
        CaptureCamera();
        RapidBlurEffect.RenderEnd();
        oldBG.SetActive(false);
    }

    void CaptureCamera()
    {
        tmpBgTex = CommonFunc.CaptureCamera(NGUITools.FindCameraForLayer(this.gameObject.layer), new Rect(0, 0, Screen.width, Screen.height));
        texBG.mainTexture = tmpBgTex;
    }

    void Destroy()
    {
        if (null != tmpBgTex)
        {
            Destroy(tmpBgTex);
            tmpBgTex = null;
        }
    }
}
