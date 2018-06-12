using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace YunvaVideoTroops
{

	public class YunvaLogPrint
	{
		private static int s_YvlogLevel = (int)YvlogLevel.LOG_LEVEL_OFF;

	    public static void YvLog_setLogLevel(int loglevel)
		{
			s_YvlogLevel = loglevel;
		}

		public static void YvDebugLog (string yv_function,string yv_msg)
		{
			YvLogPrintf (yv_function, yv_msg, (int)YvlogLevel.LOG_LEVEL_DEBUG);
		}

		public static void YvInfoLog(string yv_function,string yv_msg)
		{
			YvLogPrintf (yv_function, yv_msg, (int)YvlogLevel.LOG_LEVEL_INFO);
		}
		
		public static void YvErrorLog(string yv_function,string yv_msg)
		{
			YvLogPrintf (yv_function, yv_msg, (int)YvlogLevel.LOG_LEVEL_ERROR);
		}

		private static void YvLogPrintf(string yv_function,string yv_msg,int loglevel=(int)YvlogLevel.LOG_LEVEL_DEBUG)
		{
			if (s_YvlogLevel == (int)YvlogLevel.LOG_LEVEL_OFF)
			{
				return;
			}
			if (loglevel <= s_YvlogLevel) 
			{
				if(loglevel==(int)YvlogLevel.LOG_LEVEL_DEBUG)
				{
					Debug.Log (string.Format ("###YunVaU3DLogPrintf [Debug]###{0},logMsg:{1}", yv_function, yv_msg));
				}
				if(loglevel==(int)YvlogLevel.LOG_LEVEL_INFO)
				{
                    Debug.Log(string.Format("###YunVaU3DLogPrintf [Info]###{0},logMsg:{1}", yv_function, yv_msg));
				}
				if(loglevel==(int)YvlogLevel.LOG_LEVEL_ERROR)
				{
                    Debug.Log(string.Format("###YunVaU3DLogPrintf [Error]###{0},logMsg:{1}", yv_function, yv_msg));
				}
			}
		}
	}

    public enum YvlogLevel
    {
        LOG_LEVEL_OFF = 0,  //0：关闭日志
        LOG_LEVEL_DEBUG = 1, //1：Debug默认该级别
        LOG_LEVEL_INFO = 2,   //2:info
        LOG_LEVEL_ERROR = 3   //3:error
    }
}
