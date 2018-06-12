
using System;
using UnityEngine;
using System.Collections.Generic;


namespace YunvaVideoTroops
{
	public enum YvListenerEven :uint
	{
		SendRealTimeVoiceMessageErrorNotify,
		ReceiveRealTimeVoiceMessageNofify,
		ReceiveTextMessageNotify,
		ReceiveVoiceMessageNotify,
		ReceiveRichMessageNotify,
		KickOutNotify,
		UserStateNotify,
		MicModeChangeNotify,//麦模式通知
		TroopsChangeNotify,//队伍进出房间通知
		RecorderMeteringPeakPowerNotify,
		PlayMeteringPeakPowerNotify,
        AudioToolsRecorderMeteringPeakPowerNotify,
        AudioToolsPlayMeteringPeakPowerNotify,
		MicStateNotify,//上下麦通知
        onProxyQueryNotification,
        onConnectFail,
        onReconnectSuccess
	}

    public delegate void Callback<T>(T arg1);
    static public class YvEventListenerManager
    {
		private static Dictionary<YvListenerEven, Delegate> mProtocolEventTable = new Dictionary<YvListenerEven, Delegate>();

		public static void AddListener(YvListenerEven protocolEnum, Callback<System.Object> kHandler)
        {
            lock (mProtocolEventTable)
            {
                if (!mProtocolEventTable.ContainsKey(protocolEnum))
                {
                    mProtocolEventTable.Add(protocolEnum, null);
                }

                mProtocolEventTable[protocolEnum] = (Callback<System.Object>)mProtocolEventTable[protocolEnum] + kHandler;
            }
        }

		public static void RemoveListener(YvListenerEven protocolEnum, Callback<System.Object> kHandler)
        {
            lock (mProtocolEventTable)
            {
                if (mProtocolEventTable.ContainsKey(protocolEnum))
                {
                    mProtocolEventTable[protocolEnum] = (Callback<System.Object>)mProtocolEventTable[protocolEnum] - kHandler;

                    if (mProtocolEventTable[protocolEnum] == null)
                    {
                        mProtocolEventTable.Remove(protocolEnum);
                    }
                }
            }
        }

		public static void Invoke(YvListenerEven protocolEnum, System.Object arg1)
        {
            try
            {
                Delegate kDelegate;
                if (mProtocolEventTable.TryGetValue(protocolEnum, out kDelegate))
                {
                    Callback<System.Object> kHandler = (Callback<System.Object>)kDelegate;

                    if (kHandler != null)
                    {
                        kHandler(arg1);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public static void UnInit()
        {
            mProtocolEventTable.Clear();
        }
			
    }


}
