using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonFunc
{

    public static Texture2D CaptureCamera(Camera camera, Rect rect)
    {
        // 创建一个RenderTexture对象  
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 24);
        // 临时设置相关相机的targetTexture为rt, 并手动渲染该相机
        RenderTexture originRtt = null;
        //string removeName = "PlayerCamera";
        foreach (Camera ca in Camera.allCameras)
        {
            //if (ca.name.Equals(removeName))
            if (ca.targetTexture != null)
            {
                continue;
            }
            originRtt = ca.targetTexture;
            ca.targetTexture = rt;
            ca.RenderDontRestore();
            ca.targetTexture = originRtt;
        }
        //camera.targetTexture = rt;
        //camera.RenderDontRestore();
        //camera.targetTexture = null;
        // 激活这个rt, 并从中中读取像素。  
        RenderTexture.active = rt;  //一定要激活，不然读取不到像素的
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(rect, 0, 0);// 这个时候，它是从RenderTexture.active中读取像素 
        screenShot.Apply();
        GameObject.Destroy(rt);
        // 重置相关参数，以使camera继续在屏幕上显示  
        //camera.targetTexture = null;
        RenderTexture.active = null;
        return screenShot;
    }
}
