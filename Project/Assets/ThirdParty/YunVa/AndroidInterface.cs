//#if UNITY_ANDROID
using UnityEngine;
using System.Collections;
using YunvaVideoTroops;

namespace YunvaVideoTroops
{
	public class AndroidInterface : MonoBehaviour
	{
		private   AndroidJavaObject _plugin = null;
		private   AndroidJavaObject _audioTools = null;

//	    static AndroidInterface()
//	    {
//	        using (AndroidJavaClass jpushClass = new AndroidJavaClass("com.yaya.realtime.voice.u3d.api.YvChatManageUnity3dExtension"))
//	        {
//	            _plugin = jpushClass.CallStatic<AndroidJavaObject>("getInstance");
//	
//	        }
//	
//			using (AndroidJavaClass tools = new AndroidJavaClass ("com.yaya.realtime.voice.u3d.api.YvAudioToolsUnity3dExtension")) {
//				_audioTools = tools.CallStatic<AndroidJavaObject> ("getInstance");
//			}
//						if (_plugin == null) {
//							Debug.Log ("UnityPlugins is NULL!!!!!!!!!");
//						} else {
//							Debug.Log ("UnityPlugins is NotNULL!!!!!!!!!");
//						}
//
//	    }

		public AndroidInterface()
		{
			using (AndroidJavaClass jpushClass = new AndroidJavaClass("com.yaya.realtime.voice.u3d.api.YvChatManageUnity3dExtension"))
			{
				_plugin = jpushClass.CallStatic<AndroidJavaObject>("Instance");

			}

			using (AndroidJavaClass tools = new AndroidJavaClass ("com.yaya.realtime.voice.u3d.api.YvAudioToolsUnity3dExtension")) {
				_audioTools = tools.CallStatic<AndroidJavaObject> ("getInstance");
				if (_audioTools == null) {
					Debug.Log ("_audioTools is NULL !");
				}

			}

		}


	    //初始化接口
	    public void YvChatSDKInit(string appId, bool isTest, string gameObjectName)
	    {
			if (_plugin == null) return;	
			
	    //    _plugin.Call("YvChatSDKInit", new object[] { appId, isTest, gameObjectName });
			_plugin.Call("YvChatSDKInit",  appId, isTest, gameObjectName);
	    }

		public void YvChatSDKInit(int environment,string appId,string gameObjectName)
		{
			if (_plugin == null) return;
			_plugin.Call("YvChatSDKInit", appId, environment, gameObjectName);
		}

	    //自动登录队伍
	    public void loginWithSeq(string seq)
	    {
			if (_plugin == null) return;
	        _plugin.Call("loginWithSeq", seq);
	    }

	    //第三方绑定登录队伍
	    public void loginBindingWithTT(string tt, string seq)
	    {
			if (_plugin == null) return;
	        _plugin.Call("loginBindingWithTT", tt, seq);
	    }

	    //登出队伍
	    public void logout()
	    {
			if (_plugin == null) return;
	        _plugin.Call("logout");
	    }

	    //实时语音上麦，下麦
	    public void chatMic(bool onOff, string expand)
	    {
			if (_plugin == null) return;
	        _plugin.Call("chatMic", onOff, expand);
	    }
	    /**
	 *  设置是否暂停播放实时语音聊天
	 *
	 *  @param isPause       true:暂停实时播放  false:恢复实时播放
	 */
	    public void setPausePlayRealAudio(bool isPause)
	    {
			if (_plugin == null) return;
	        _plugin.Call("setPausePlayRealAudio", isPause);
	    }

	    /**
	     *  查询是否暂停播放实时语音聊天
	     */
	    public bool isPausePlayRealAudio()
	    {
			if (_plugin == null) return false;
	        //_plugin.Call ("isPausePlayRealAudio");	
	        return _plugin.Call<bool>("isPausePlayRealAudio");
	    }

	    /**
	 *  设置麦模式，mode:0自由模式，1抢麦模式，2指挥模式
	 *
	 *  @param modeType      0自由模式，1抢麦模式，2指挥模式
	 */
	    public void micModeSettingReq(int modeType)
	    {
			if (_plugin == null) return;
	        _plugin.Call("micModeSettingReq", modeType);
	    }

	    /**
	 *  设置播放实时语音声源（系统声音/媒体声音）
	 *
	 *  @param streamType      1系统声音，3媒体声音
	 */
	    public void setAudioPlayStreamType(int streamType)
	    {
			if (_plugin == null) return;
	        _plugin.Call("setAudioPlayStreamType", streamType);
	    }

		public void sendTextMessage(string text, string expand)
		{
			if (_plugin == null) return;
			_plugin.Call("sendTextMessage",text,expand);
		}

		public void sendVoiceMessage(string voiceFilePath, int voiceDuration,string expand)
		{
			if (_plugin == null) return;
			_plugin.Call ("sendVoiceMessage", voiceFilePath, voiceDuration, expand);
		}

		public void sendRichMessageWithTextMsg(string text,string voiceFilePath,int voiceDuration,string expand)
		{
			if (_plugin == null) return;
			_plugin.Call ("sendRichMessage", text, voiceFilePath, voiceDuration, expand);
		}

		public void UploadVoiceFile(string voiceFilePath,int fileRetainTimeType,string expand)
		{
			if (_plugin == null) return;
			_plugin.Call ("uploadVoiceFile", voiceFilePath, fileRetainTimeType, expand);
		}

		public void QueryHistoryMsgReqWithPageIndex(int pageIndex,int pageSize)
		{
			if (_plugin == null) return;
			_plugin.Call ("queryHistoryMsgReqWithPageIndex", pageIndex, pageSize);
		}

		public void VoiceRecognizeReq(int recognizeLanguage, int outputTextlanguageType,string voiceFilePath,int voiceDuration,string expand)
		{
			if (_plugin == null)
				return;
            _plugin.Call("startRecognize", recognizeLanguage, outputTextlanguageType, voiceFilePath, expand);
		}

        public void speechDiscernByUrl(int recognizeLanguage, int outputTextlanguageType, string voiceFilePath, int voiceDuration, string expand)
        {
            if (_plugin == null)
                return;
            _plugin.Call("speechDiscernByUrl", recognizeLanguage, outputTextlanguageType, voiceFilePath, expand);
        }

		public void SetVoiceVolume(int volume)
		{
			if (_plugin == null)
				return;
			_plugin.Call ("SetVoiceVolume", volume);
		}

        public void setBackgroundVoiceLimitEnable(bool enable)
        {
            if (_plugin == null)
                return;
            _plugin.Call("setBackgroundVoiceLimitEnable", enable);
        }

        public void setDefaultVolumeEnable(bool enable)
        {
            if (_plugin == null)
                return;
            _plugin.Call("setDefaultVolumeEnable", enable);
        }

        //播放音量类型0为通话音量，1为媒体音量
        public void setAudioManagerStreamType(int type)
        {
            if (_plugin == null)
                return;
            _plugin.Call("setAudioManagerStreamType", type);
        }

		///**设置日志级别:0--关闭日志（不设置为默认该级别）  1--error  2--debug 3--warn  4--info  5--trace*/
		public void SetLogLevel(int level)
		{
			if (_plugin == null)
				return;
			_plugin.Call ("SetLogLevel", level);
		}


		public void ChatGetTroopsListReq()
		{
			if (_plugin == null)
				return;
			_plugin.Call ("ChatGetTroopsListReq");
		}

        /**
        *  获取当前上下麦状态
        */
        public bool getMicUpOrDown()
        {
            if (_plugin == null) return false;	
            return _plugin.Call<bool>("getMicUpOrDown");
        }

        //public void startWeb()
        //{
        //    if (_plugin == null)
        //        return;
        //    _plugin.Call("startWeb");
        //}

        //public void setqueryNotification()
        //{
        //    if (_plugin == null)
        //        return;
        //    _plugin.Call("setqueryNotification");
        //}
        
		public void AudioToolsInit(string gameObjectName)
		{
			if (_audioTools == null) return;
			Debug.Log ("初始化：AudioToolsInit");
			_audioTools.Call ("audioTools_Init", gameObjectName);
		}

		public void AudioToolsStartRecord(string storeDataFilePath,int minMillSeconds,int maxMillSeconds)
		{
			if (_audioTools == null) return;
			_audioTools.Call ("audioTools_startRecord", storeDataFilePath, minMillSeconds, maxMillSeconds);
		}

        public bool AudioToolsStartRecord(string storeDataFilePath)
        {
            if (_audioTools == null) return false;
            return _audioTools.Call<bool>("audioTools_startRecord", storeDataFilePath);
        }
        public bool AudioToolsStartRecord(string storeDataFilePath, int speech)
        {
            if (_audioTools == null) return false;
            return _audioTools.Call<bool>("audioTools_startRecord", storeDataFilePath, speech);
        }
		public void AudioToolsStopRecord()
		{
			if (_audioTools == null) return;
			_audioTools.Call ("audioTools_stopRecord");
		}

		public bool AudioToolsIsRecording()
		{
			if (_audioTools == null) return false;
			return _audioTools.Call<bool> ("audioTools_isRecording");
		}

		public int  AudioToolsPlayAudio(string filePath)
		{
			if (_audioTools == null) return 0;
			return _audioTools.Call<int>("audioTools_playAudio",filePath);
		}

		public int audioToolsPlayOnlineAudio(string fileUrl)
		{
			if (_audioTools == null) return 0;
			return _audioTools.Call<int> ("audioTools_playOnlineAudio", fileUrl);
		}

		public void AudioToolsStopPlayAudio()
		{
			if (_audioTools == null) return;
			_audioTools.Call ("audioTools_stopPlayAudio");
		}

		public bool audioToolsisPlaying()
		{
			if (_audioTools == null) return false;
			return _audioTools.Call<bool> ("audioTools_isPlaying");
		}

		public bool audioToolsDeleteFile(string filePath)
		{
			if (_audioTools == null) return false;
			return _audioTools.Call<bool> ("audioTools_deleteFile");
		}
        //文字转语音
        public void TextSendind(string text, string per)
        {
            if (_audioTools == null) return;
            _audioTools.Call("TextSendind", text, per);
        }

        //public void SpeedUpInit(string appid)
        //{
        //    if (_audioTools == null) return;
        //    _audioTools.Call("SpeedUpInit", appid);
        //}
        
        //public void SpeedUpStartWeb()
        //{
        //    if (_audioTools == null) return;
        //    _audioTools.Call ("SpeedUpStartWeb");
        //}
        //public void GeneralFlowInit(string appid)
        //{
        //    if (_audioTools == null) return;
        //    _audioTools.Call("GeneralFlowInit", appid);
        //}

        //public void GeneralFlowStartWeb()
        //{
        //    if (_audioTools == null) return;
        //    _audioTools.Call("GeneralFlowStartWeb");
        //}
	}
}
//#endif