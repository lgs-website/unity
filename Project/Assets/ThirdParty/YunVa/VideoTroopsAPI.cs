using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Threading;
using UnityEngine;
using YunvaVideoTroops;

namespace YunvaVideoTroops
{
    public enum PathType
    {
        filePath,
        urlPath,
    }
	public class VideoTroopsAPI:YvMonoSingleton<VideoTroopsAPI>
	{
		
		#if UNITY_ANDROID
		private static AndroidInterface androidInterface;

//				static VideoTroopsAPI() {
//					androidInterface = new AndroidInterface();
//				}
		#endif

		public override void Init()
		{

			base.Init ();
			DontDestroyOnLoad (this);
			#if UNITY_ANDROID
			androidInterface = new AndroidInterface();
			#endif


		}
	
		//SDK初始化
		private System.Action<string> InitResp;
		public  void YvChatSDKInit(string appId, bool isTest,System.Action<string> response = null)
		{
            YunvaLogPrint.YvDebugLog("YvChatSDKInit", string.Format("appId:{0},isTest:{1},gameObjectName:{2}", appId, isTest, this.gameObject.name));
			InitResp = response;
			#if UNITY_IOS
			IOSInterface.YvChatSDKInit(appId, isTest, this.gameObject.name);
			#elif UNITY_ANDROID
            androidInterface.YvChatSDKInit(appId, isTest, this.gameObject.name);
			#endif
		}

		public void YvChatSDKInit(int environment,string appId,System.Action<string> response = null)
		{
            YunvaLogPrint.YvDebugLog("YvChatSDKInit", string.Format("environment:{0},appId:{1},gameObjectName:{2}", environment, appId, this.gameObject.name));
			InitResp = response;
			#if UNITY_IOS
			IOSInterface.YvChatSDKInitWithenvironment (environment, appId, this.gameObject.name);
			#elif UNITY_ANDROID
            androidInterface.YvChatSDKInit(environment, appId, this.gameObject.name);
			#endif
		}

		//登录（不推荐使用此接口）
		private System.Action<String> LoginResp;
		public void LoginWithSeq(string seq,System.Action<string> response = null)
		{
            YunvaLogPrint.YvDebugLog("LoginWithSeq", string.Format("seq:{0}", seq));
			LoginResp = response;
			#if UNITY_IOS
			IOSInterface.loginWithSeq(seq);
			#elif UNITY_ANDROID
			androidInterface.loginWithSeq(seq);
			#endif
		}
			
		//登录（CP接入推荐使用此接口）
		public void LoginBindingWithTT(string tt, string seq,System.Action<string> response = null)
		{
            YunvaLogPrint.YvDebugLog("LoginBindingWithTT", string.Format("tt:{0},seq:{1}", tt, seq));
			LoginResp = response;
			#if UNITY_IOS
			IOSInterface.loginBindingWithTT(tt, seq);
			#elif UNITY_ANDROID
			androidInterface.loginBindingWithTT(tt, seq);
			#endif
		}

		//登出
		private System.Action<String> LogoutResp;
		public void Logout(System.Action<String> response = null)
		{
            YunvaLogPrint.YvDebugLog("Logout", "");
			LogoutResp = response;
			#if UNITY_IOS
			IOSInterface.logout();
			#elif UNITY_ANDROID
			androidInterface.logout();
			#endif
		}

		//上，下麦操作
		private System.Action<String> ChatMicResp;
		public void ChatMic(bool onOff, string expand,System.Action<String> response = null)
		{
            YunvaLogPrint.YvDebugLog("ChatMic", string.Format("onOff:{0},expand:{1}", onOff, expand));
			ChatMicResp = response;
			#if UNITY_IOS
			IOSInterface.chatMic(onOff,expand);
			#elif UNITY_ANDROID
			androidInterface.chatMic(onOff,expand);
			#endif
		}

		public void ChatMicWithTimeLimit(int timeLimit,string expand,System.Action<String> response = null)
		{
            YunvaLogPrint.YvDebugLog("ChatMicWithTimeLimit", string.Format("timeLimit:{0},expand:{1}", timeLimit, expand));
			ChatMicResp = response;
			#if UNITY_IOS
			IOSInterface.ChatMicWithTimeLimit(timeLimit,expand);
			#elif UNITY_ANDROID
			#endif
		}

		//暂停播放实时语音
		public void SetPausePlayRealAudio(bool isPause)
		{
            YunvaLogPrint.YvDebugLog("SetPausePlayRealAudio", string.Format("isPause:{0}", isPause));
			#if UNITY_IOS
			IOSInterface.setPausePlayRealAudio(isPause);
			#elif UNITY_ANDROID
			androidInterface.setPausePlayRealAudio(isPause);
			#endif
		}

		//返回是否正在播入实时语音
		public bool IsPausePlayRealAudio()
		{
            YunvaLogPrint.YvDebugLog("IsPausePlayRealAudio", "");
			#if UNITY_IOS
			return IOSInterface.isPausePlayRealAudio();
			#elif UNITY_ANDROID
			return androidInterface.isPausePlayRealAudio();
			#endif
			return true;

		}

		//设置麦模式：0自由模式，1抢麦模式，2指挥模式
		private System.Action<String> micModeSettingReqResp;
		public void MicModeSettingReq(int modeType , System.Action<String> response = null)
		{
            YunvaLogPrint.YvDebugLog("MicModeSettingReq", string.Format("modeType:{0}", modeType));
			micModeSettingReqResp = response;
			#if UNITY_IOS
			IOSInterface.micModeSettingReq(modeType);
			#elif UNITY_ANDROID
			androidInterface.micModeSettingReq(modeType);
			#endif
		}

		//设置语音流类型
		public void SetAudioPlayStreamType(int streamType)
		{
            YunvaLogPrint.YvDebugLog("SetAudioPlayStreamType", string.Format("streamType:{0}", streamType));
			#if UNITY_IOS

			#elif UNITY_ANDROID
			androidInterface.setAudioPlayStreamType(streamType);
			#endif
		}

		//发送文本消息
		private System.Action<String> SendTextMessageResp;
		public void SendTextMessage(string text , string expand,System.Action<string> response = null)
		{
            YunvaLogPrint.YvDebugLog("SendTextMessage", string.Format("text:{0},expand:{1}", text, expand));
			SendTextMessageResp = response;
			#if UNITY_IOS
			IOSInterface.sendTextMessage (text, expand);
			#elif UNITY_ANDROID
			androidInterface.sendTextMessage (text, expand);
			#endif
		}

//		//发送声音消息
		private System.Action<string> SendVoiceMessageResp;
		public void SendVoiceMessage(string voiceFilePath,int voiceDuration,string expand, System.Action<string> response = null)
		{
            YunvaLogPrint.YvDebugLog("SendVoiceMessage", string.Format("voiceFilePath:{0},voiceDuration:{1},expand:{2}", voiceFilePath, voiceDuration, expand));
			SendVoiceMessageResp = response;
			#if UNITY_IOS
			IOSInterface.sendVoiceMessage (voiceFilePath, voiceDuration, expand);
			#elif UNITY_ANDROID
			androidInterface.sendVoiceMessage (voiceFilePath, voiceDuration, expand);
			#endif
		}

		private System.Action<string> SendRichMessageResp;
		public void SendRichMessage(string text,string voiceFilePath,int voiceDuration,string expand, System.Action<string> response = null)
		{
            YunvaLogPrint.YvDebugLog("SendRichMessage", string.Format("text:{0},voiceFilePath:{1},voiceDuration:{2},expand:{3}", text, voiceFilePath, voiceDuration, expand));
			SendRichMessageResp = response;
			#if UNITY_IOS
			IOSInterface.sendRichMessage (text, voiceFilePath, voiceDuration, expand);
			#elif UNITY_ANDROID
			androidInterface.sendRichMessageWithTextMsg(text, voiceFilePath, voiceDuration, expand);
			#endif
		}

//		//上传语音文件
		private System.Action<string> UploadVoiceFileResp;
		public void UploadVoiceFile(string voiceFilePath,int fileRetainTimeType,string expand,System.Action<string> response = null)
		{
            YunvaLogPrint.YvDebugLog("UploadVoiceFile", string.Format("voiceFilePath:{0},fileRetainTimeType:{1},expand:{2}", voiceFilePath, fileRetainTimeType, expand));
			UploadVoiceFileResp = response;
			#if UNITY_IOS
			IOSInterface.uploadVoiceFile (voiceFilePath, fileRetainTimeType, expand);
			#elif UNITY_ANDROID
			androidInterface.UploadVoiceFile(voiceFilePath, fileRetainTimeType, expand);
			#endif
		}

////		//查看聊天记录
//        private System.Action<string> QueryHistoryMsgResp;
//        public void QueryHistoryMsgReqWithPageIndex(int pageIndex, int pageSize,System.Action<string> response)
//        {
//            YunvaLogPrint.YvDebugLog("QueryHistoryMsgReqWithPageIndex", string.Format("pageIndex:{0},pageSize:{1}", pageIndex, pageSize));
//            QueryHistoryMsgResp = response;
//            #if UNITY_IOS
//            IOSInterface.queryHistoryMsgReqWithPageIndex (pageIndex, pageSize);
//            #elif UNITY_ANDROID
//            androidInterface.QueryHistoryMsgReqWithPageIndex (pageIndex, pageSize);
//            #endif
//        }

		//获取房间用户列表
		private System.Action<string> ChatGetTroopsListResp;
		public void ChatGetTroopsListReq(System.Action<string> response)
		{
            YunvaLogPrint.YvDebugLog("ChatGetTroopsListReq", "");
			ChatGetTroopsListResp = response;
			#if UNITY_IOS
			IOSInterface.ChatGetTroopsListReq ();
			#elif UNITY_ANDROID
			androidInterface.ChatGetTroopsListReq();
			#endif
		}

		//设置音量（android)端有效果 值是从1-10范围 正常取5
		public void SetVoiceVolume(int volume)
		{
            YunvaLogPrint.YvDebugLog("SetVoiceVolume", string.Format("volume:{0}", volume));
			#if UNITY_ANDROID
			androidInterface.SetVoiceVolume (volume);
			#endif
		}

        //可关闭或开启媒体音量的限制开关,默认开启  true：限制，false：不限制
        public void setBackgroundVoiceLimitEnable(bool enable)
        {
            YunvaLogPrint.YvDebugLog("setBackgroundVoiceLimitEnable", string.Format("enable:{0}", enable));
            #if UNITY_ANDROID
            androidInterface.setBackgroundVoiceLimitEnable(enable);
            #endif
        }

        public void setDefaultVolumeEnable(bool enable)
        {
            YunvaLogPrint.YvDebugLog("setDefaultVolumeEnable", string.Format("enable:{0}", enable));
            #if UNITY_ANDROID
            androidInterface.setDefaultVolumeEnable(enable);
            #endif
        }

        //播放音量类型0为通话音量（默认此音量），1为媒体音量
        public void setAudioManagerStreamType(int type)
        {
            YunvaLogPrint.YvDebugLog("setAudioManagerStreamType", string.Format("type:{0}", type));
            #if UNITY_ANDROID
            setDefaultVolumeEnable(false);
            androidInterface.setAudioManagerStreamType(type);
            #endif
        }

        /**
        *  获取当前上下麦状态
        */
        public bool getMicUpOrDown()
        {
            YunvaLogPrint.YvDebugLog("getMicUpOrDown", string.Format(""));
            #if UNITY_ANDROID
                return androidInterface.getMicUpOrDown();
            #endif
                return false;
        }

//		//声音工具初始化
		public void AudioToolsInit()
		{
            YunvaLogPrint.YvDebugLog("AudioToolsInit", string.Format("gameObjectName:{0}", this.gameObject.name));
			#if UNITY_IOS
			IOSInterface.audioTools_Init (this.gameObject.name);
			#elif UNITY_ANDROID
			androidInterface.AudioToolsInit (this.gameObject.name);
			#endif
		}

//		//开始录音
		private System.Action<string> audioToolsStopRecordResp;
		public void AudioToolsStartRecord(string storeDataFilePath,int minMillSeconds=1,int maxMillSeconds=1,System.Action<string> response = null)
		{
            YunvaLogPrint.YvDebugLog("AudioToolsStartRecord", string.Format("storeDataFilePath:{0},minMillSeconds:{1},maxMillSeconds:{2}", storeDataFilePath, minMillSeconds, maxMillSeconds));
			audioToolsStopRecordResp = response;
			#if UNITY_IOS
			IOSInterface.audioTools_startRecord (minMillSeconds, maxMillSeconds);
			#elif UNITY_ANDROID
            androidInterface.AudioToolsStartRecord(storeDataFilePath);
            //androidInterface.AudioToolsStartRecord(storeDataFilePath,3);
			#endif
		}
//
//		//停止录音
		public void AudioToolsStopRecord()
		{
            YunvaLogPrint.YvDebugLog("AudioToolsStopRecord", "");
			#if UNITY_IOS
			IOSInterface.audioTools_stopRecord ();
			#elif UNITY_ANDROID
			androidInterface.AudioToolsStopRecord ();
			#endif
		}
//
//		//判断是否正在录音
		public bool AudioToolsIsRecording()
		{
            YunvaLogPrint.YvDebugLog("AudioToolsIsRecording", "");
			#if UNITY_IOS
			return IOSInterface.audioTools_isRecording ();
			#elif UNITY_ANDROID
			return androidInterface.AudioToolsIsRecording ();
			#else
			return false;
			#endif
		}

//		//播放本地录音文件
		private System.Action<string> audioToolsPlayAudioFinishResp;
		public int  AudioToolsPlayAudio(string filePath ,System.Action<string> response = null)
		{
            YunvaLogPrint.YvDebugLog("AudioToolsPlayAudio", string.Format("filePath:{0}", filePath));
			audioToolsPlayAudioFinishResp = response;
			#if UNITY_IOS
			return IOSInterface.audioTools_playAudio (filePath);
			#elif UNITY_ANDROID
			return androidInterface.AudioToolsPlayAudio (filePath);
			#else
			return int.MaxValue;
			#endif
		}

//		//在线播放录音文件
		public int AudioToolsPlayOnlineAudio(string fileUrl ,System.Action<string> response = null)
		{
            YunvaLogPrint.YvDebugLog("AudioToolsPlayOnlineAudio", string.Format("fileUrl:{0}", fileUrl));
			audioToolsPlayAudioFinishResp = response;
			#if UNITY_IOS
			return IOSInterface.audioTools_playOnlineAudio (fileUrl);
			#elif UNITY_ANDROID
			return androidInterface.audioToolsPlayOnlineAudio (fileUrl);
			#else
			return int.MaxValue;
			#endif
		}
//
//		//停止播放录音文件
		public void AudioToolsStopPlayAudio()
		{
            YunvaLogPrint.YvDebugLog("AudioToolsStopPlayAudio", "");
			#if UNITY_IOS
			IOSInterface.audioTools_stopPlayAudio ();
			#elif UNITY_ANDROID
			androidInterface.AudioToolsStopPlayAudio ();
			#endif
		}

//		//录音文件是否正在播放
		public bool AudioToolsisPlaying()
		{
            YunvaLogPrint.YvDebugLog("AudioToolsisPlaying", "");
			#if UNITY_IOS
			return IOSInterface.audioTools_isPlaying ();
			#elif UNITY_ANDROID
			return androidInterface.audioToolsisPlaying ();
			#else
			return false;
			#endif
		}


		/*语音识别
		 * @recognizeLanguage 			0普通话
		 * @outputTextLanguageType		0简体中文
		 * @voiceFilePath				文件路径
		 * @voiceDuration				时长
		 * @resp						回调
		*/
		private System.Action<string> VoiceRecognizeReqAction;
		public void VoiceRecognizeReq(int recognizeLanguage,int outputTextLanguageType,string voiceFilePath,int voiceDuration,String expand,System.Action<string> resp)
		{
            YunvaLogPrint.YvDebugLog("VoiceRecognizeReq", string.Format("recognizeLanguage:{0},outputTextLanguageType:{1},voiceFilePath:{2},voiceDuration:{3},expand:{4}", recognizeLanguage, outputTextLanguageType, voiceFilePath, voiceDuration, expand));
			VoiceRecognizeReqAction = resp;
			#if UNITY_IOS
			IOSInterface.httpVoiceRecognizeReqAndUploadVoiceFile(recognizeLanguage,outputTextLanguageType,voiceFilePath,voiceDuration,5,expand);
			#elif UNITY_ANDROID
			androidInterface.VoiceRecognizeReq(recognizeLanguage,outputTextLanguageType,voiceFilePath,voiceDuration,expand);
			#endif
		}

        public void speechDiscernByUrl(int recognizeLanguage, int outputTextLanguageType, string voiceUrlFilePath, int voiceDuration, String expand, System.Action<string> resp)
        {
            YunvaLogPrint.YvDebugLog("speechDiscernByUrl", string.Format("recognizeLanguage:{0},outputTextLanguageType:{1},voiceUrlFilePath:{2},voiceDuration:{3},expand:{4}", recognizeLanguage, outputTextLanguageType, voiceUrlFilePath, voiceDuration, expand));
            VoiceRecognizeReqAction = resp;
            #if UNITY_IOS
                IOSInterface.speechDiscernByUrl(recognizeLanguage, outputTextLanguageType, voiceUrlFilePath, expand);
            #elif UNITY_ANDROID
                androidInterface.speechDiscernByUrl(recognizeLanguage, outputTextLanguageType, voiceUrlFilePath, voiceDuration, expand);
            #endif
        }

//		//删除录音文件
		public bool AudioToolsDeleteFile(string filePath)
		{
            YunvaLogPrint.YvDebugLog("AudioToolsDeleteFile", string.Format("filePath:{0}", filePath));
			#if UNITY_IOS
			return IOSInterface.audioTools_deleteFile (filePath);
			#elif UNITY_ANDROID
			return androidInterface.audioToolsDeleteFile (filePath);
			#else
			return false;
			#endif
		}

        //文字转语音
        private System.Action<string> synthesizeSentenceAction;
        public void synthesizeSentence(string text, string per, System.Action<string> response = null)
        {
            synthesizeSentenceAction = response;
            YunvaLogPrint.YvDebugLog("synthesizeSentence", string.Format("voiceUrl:{0},per:{1}", text, per));
            #if UNITY_IOS
	            IOSInterface.synthesizeSentence(text,per);
            #elif UNITY_ANDROID
                androidInterface.TextSendind(text, per);
            #endif
        }

		/**设置日志级别:0--关闭日志（不设置为默认该级别）  1--error  2--debug 3--warn  4--info  5--trace*/
		public void SetLogLevel(int level)
		{
            YunvaLogPrint.YvDebugLog("SetLogLevel", string.Format("level:{0}", level));
			#if UNITY_IOS
			IOSInterface.YvSetLogLevel (level);
			#elif UNITY_ANDROID
			androidInterface.SetLogLevel(level);

			#endif
		}

        public void AudioToolsSetMeteringEnabled(bool enable)
        {
            YunvaLogPrint.YvDebugLog("AudioToolsSetMeteringEnabled", string.Format("enable:{0}", enable));
            #if UNITY_IOS
            IOSInterface.audioTools_setMeteringEnabled (enable);
            #elif UNITY_ANDROID

            #endif
        }

		#region  CallBack
		public void onSDKInitDidFinish (string msg) 
		{
            YunvaLogPrint.YvDebugLog("onSDKInitDidFinish", string.Format("JsonMsg:{0}", msg));
            AudioToolsInit();
            #if UNITY_IOS
                AudioToolsSetMeteringEnabled(true);
            #elif UNITY_ANDROID
                setBackgroundVoiceLimitEnable(false);
            #endif
         
			if (InitResp != null) {
				InitResp (msg);
				InitResp = null;
			}
		}

		public void onAuthResp (string msg) 
		{
            YunvaLogPrint.YvDebugLog("onAuthResp", string.Format("JsonMsg:{0}", msg));
		}

		public void onLoginResp (string msg) 
		{
            //SetVoiceVolume(10);
            YunvaLogPrint.YvDebugLog("onLoginResp", string.Format("JsonMsg:{0}", msg));
			if (LoginResp != null) {
				LoginResp (msg);
				LoginResp = null;
			}
		}

		public void onLogoutResp (string msg) 
		{
            YunvaLogPrint.YvDebugLog("onLogoutResp", string.Format("JsonMsg:{0}", msg));
			if (LogoutResp != null) {
				LogoutResp (msg);
				LogoutResp = null;
			}
		}

		public void onChatMicResp (string msg) 
		{
            YunvaLogPrint.YvDebugLog("onChatMicResp", string.Format("JsonMsg:{0}", msg));
			if (ChatMicResp != null) {
				ChatMicResp (msg);
				ChatMicResp = null;
			}
		}

		//发送实时语音失败回调
		public void onSendRealTimeVoiceMessageError (string msg) 
		{
            YunvaLogPrint.YvDebugLog("onSendRealTimeVoiceMessageError", string.Format("JsonMsg:{0}", msg));
			YvEventListenerManager.Invoke (YvListenerEven.SendRealTimeVoiceMessageErrorNotify, (object)msg);
		}

		//收到实时语音
		public void onRealTimeVoiceMessageNotify (string msg) 
		{
            //YunvaLogPrint.YvDebugLog("onRealTimeVoiceMessageNotify", string.Format("JsonMsg:{0}", msg));
			YvEventListenerManager.Invoke (YvListenerEven.ReceiveRealTimeVoiceMessageNofify, (object)msg);
		}

		//被踢出队伍通知
		public void onKickOutNotify (string msg) 
		{
            YunvaLogPrint.YvDebugLog("onKickOutNotify", string.Format("JsonMsg:{0}", msg));
			YvEventListenerManager.Invoke (YvListenerEven.KickOutNotify, msg);
		}

		//用户状态通知
		public void onUserStateNotify (string msg) 
		{
            YunvaLogPrint.YvDebugLog("onUserStateNotify", string.Format("JsonMsg:{0}", msg));
			YvEventListenerManager.Invoke (YvListenerEven.UserStateNotify, msg);
		}

		//房间里用户变更通知
		//msg: {"yunvaId":(int)yunvaId, "seq":(string)seq, "userId":(string)userId, "changeType":(int)changeType}
		public void TroopsChangeNotify(string msg)
		{
            YunvaLogPrint.YvDebugLog("TroopsChangeNotify", string.Format("JsonMsg:{0}", msg));
			YvEventListenerManager.Invoke (YvListenerEven.TroopsChangeNotify, msg);
		}

		public void onMicModeSettingResp (string msg) 
		{
            YunvaLogPrint.YvDebugLog("onMicModeSettingResp", string.Format("JsonMsg:{0}", msg));
			if ( micModeSettingReqResp != null) {
				micModeSettingReqResp (msg);
				micModeSettingReqResp = null;
			}
		}

		//队伍模式更换通知
		public void onMicModeChangeNotify (string msg) 
		{
            YunvaLogPrint.YvDebugLog("onMicModeChangeNotify", string.Format("JsonMsg:{0}", msg));
			YvEventListenerManager.Invoke (YvListenerEven.MicModeChangeNotify, msg);
		}

        //上下麦状态通知
		//msg:{"yunvaId":(int)yunvaId, "seq":(string)seq, "userId":(string)userId, "actionType":(string)actionType}
		public void MicStateNotify(string msg)
		{
            YunvaLogPrint.YvDebugLog("MicStateNotify", string.Format("JsonMsg:{0}", msg));
			YvEventListenerManager.Invoke (YvListenerEven.MicStateNotify, msg);
		}

		//发送文本回调 msg: {"result":(int)result, "msg":(string)msg, "expand":
		public void onSendTextMessageResp(string msg)
		{
            YunvaLogPrint.YvDebugLog("onSendTextMessageResp", string.Format("JsonMsg:{0}", msg));
			if (SendTextMessageResp != null) {
				SendTextMessageResp (msg);
				SendTextMessageResp = null;
			}
		}

		//收到文本消息通知 msg: {"yunvaId":(int)yunvaId, "chatRoomId":(string)chatRoomId, "text":(string)text, "time":(UInt64)time, "expand":(string)expand}
		public void onTextMessageNotify(string msg)
		{
            YunvaLogPrint.YvDebugLog("onTextMessageNotify", string.Format("JsonMsg:{0}", msg));
			YvEventListenerManager.Invoke (YvListenerEven.ReceiveTextMessageNotify, msg);
		}

		/*发送语音回调 msg: {"result":(int)result, "msg":(string)msg,  "voiceUrl":(string)voiceUrl, 
		"voiceDuration":(UInt64)voiceDuration, "filePath":(string)filePath,  "expand":(string)expand}; */
		public void onSendVoiceMessageResp(string msg)
		{
            YunvaLogPrint.YvDebugLog("onSendVoiceMessageResp", string.Format("JsonMsg:{0}", msg));
			if (SendVoiceMessageResp != null) {
				SendVoiceMessageResp (msg);
				SendVoiceMessageResp = null;
			}
		}

		/*收到语音消息通知 msg: {"yunvaId":(int)yunvaId, "chatRoomId":(string)chatRoomId, "voiceUrl":(string)voiceUrl,  
		"voiceTime":(UInt64)voiceTime,    "time":(UInt64)time, "expand":(string)expand} */
		public void onVoiceMessageNotify(string msg)
		{
            YunvaLogPrint.YvDebugLog("onVoiceMessageNotify", string.Format("JsonMsg:{0}", msg));
			YvEventListenerManager.Invoke (YvListenerEven.ReceiveVoiceMessageNotify, msg);
		}

		//发送富消息(语音+文本)回调msg: {"result":(int)result, "msg":(string)msg, "text":(string)text,  
		//"voiceUrl":(string)voiceUrl, "voiceDuration":(UInt64)voiceDuration, "filePath":(string)filePath,  "expand":(string)expand};
		public void	onSendRichMessageResp(string msg)
		{
            YunvaLogPrint.YvDebugLog("onSendRichMessageResp", string.Format("JsonMsg:{0}", msg));
			if (SendRichMessageResp != null) {
				SendRichMessageResp (msg);
				SendRichMessageResp = null;
				Debug.Log ("发送富消息(语音+文本)回调" + msg);
			}
		}

		//收到富消息(语音+文本)通知 msg: {"yunvaId":(int)yunvaId, "chatRoomId":(string)chatRoomId, "text":(string)text, 
		//"voiceUrl":(string)voiceUrl,   "voiceTime":(UInt64)voiceTime,    "time":(UInt64)time, "expand":(string)expand}
		public void onRichMessageNotify(string msg)
		{
            YunvaLogPrint.YvDebugLog("onRichMessageNotify", string.Format("JsonMsg:{0}", msg));
			YvEventListenerManager.Invoke (YvListenerEven.ReceiveRichMessageNotify, msg);
		}


		//上传语音文件回调 msg: {"result":(int)result, "msg":(String)msg, "voiceUrl":(String)voiceUrl,  "expand":(string)expand}
		public void onUploadVoiceResp(string msg)
		{
            YunvaLogPrint.YvDebugLog("onUploadVoiceResp", string.Format("JsonMsg:{0}", msg));
			if (UploadVoiceFileResp != null) {
				UploadVoiceFileResp (msg);
				UploadVoiceFileResp = null;
			}
		}

		//语音识别+语音文件上传 回调 msg: {"result":(int)result, "msg":(String)msg, "voiceUrl":(String)voiceUrl, "text":(String)text,
		//"expand":(string)expand}
		public void onHttpVoiceRecognizeReqAndUploadVoiceFile(string msg)
		{
            YunvaLogPrint.YvDebugLog("onHttpVoiceRecognizeReqAndUploadVoiceFile", string.Format("JsonMsg:{0}", msg));
			if (VoiceRecognizeReqAction != null) 
			{
				VoiceRecognizeReqAction (msg);
				VoiceRecognizeReqAction = null;
			}
		}

		//获取房间用户列表回调msg: {"result":(int)result, "msg":(string)msg, "userList":(string)userList}
		public void GetTroopsListResp(string msg)
		{
            YunvaLogPrint.YvDebugLog("GetTroopsListResp", string.Format("JsonMsg:{0}", msg));
			if (ChatGetTroopsListResp != null) {
				ChatGetTroopsListResp (msg);
				ChatGetTroopsListResp = null;
			}
		}

		//当开启录音的计量检测,返回实时录音的峰值和平均值msg:{"peakPower":(float)peakPower,"avgPower":(float)avgPower}
		public void RecorderMeteringPeakPowerNotify(string msg)
		{
            //YunvaLogPrint.YvDebugLog("RecorderMeteringPeakPowerNotify", string.Format("JsonMsg:{0}", msg));
			YvEventListenerManager.Invoke (YvListenerEven.RecorderMeteringPeakPowerNotify, msg);
		}

		//当开启播放的计量检测,返回实时录音的峰值和平均值msg:{"peakPower":(float)peakPower,"avgPower":(float)avgPower}
		public void PlayMeteringPeakPowerNotify(string msg)
		{
            //YunvaLogPrint.YvDebugLog("PlayMeteringPeakPowerNotify", string.Format("JsonMsg:{0}", msg));
			YvEventListenerManager.Invoke (YvListenerEven.PlayMeteringPeakPowerNotify, msg);
		}

        //返回录音的峰值和平均值msg:{"peakPower":(float)peakPower,"avgPower":(float)avgPower}
        public void AudioToolsRecorderMeteringPeakPowerNotify(string msg)
        {
            //YunvaLogPrint.YvDebugLog("AudioToolsRecorderMeteringPeakPowerNotify", string.Format("JsonMsg:{0}", msg));
            YvEventListenerManager.Invoke(YvListenerEven.AudioToolsRecorderMeteringPeakPowerNotify, msg);
        }

        //返回播放的峰值和平均值msg:{"peakPower":(float)peakPower,"avgPower":(float)avgPower},Android无此回调
        public void AudioToolsPlayMeteringPeakPowerNotify(string msg)
        {
            //YunvaLogPrint.YvDebugLog("AudioToolsPlayMeteringPeakPowerNotify", string.Format("JsonMsg:{0}", msg));
            YvEventListenerManager.Invoke(YvListenerEven.AudioToolsPlayMeteringPeakPowerNotify, msg);
        }

		//录制结束回调  msg: {"filePath":(string)filePath, "voiceDuration":(int)voiceDuration}
		public void onRecordCompleteNotify(string msg)
		{
            YunvaLogPrint.YvDebugLog("onRecordCompleteNotify", string.Format("JsonMsg:{0}", msg));
			if (audioToolsStopRecordResp != null) {
				audioToolsStopRecordResp (msg);
			}
		}

		//播放结束通知 msg: {"result":(int)result, @"playSequenceId":
		public void onPlayAudioCompleteNotify(string msg)
		{
            YunvaLogPrint.YvDebugLog("onPlayAudioCompleteNotify", string.Format("JsonMsg:{0}", msg));
			if (audioToolsPlayAudioFinishResp != null) {
				audioToolsPlayAudioFinishResp (msg);
			}
		}

        public void onIsPausePlayRealAudioResp(string msg)
		{
            //YunvaLogPrint.YvDebugLog("onIsPausePlayRealAudioResp", string.Format("JsonMsg:{0}", msg));
		}

        public void SynthesizeSentenceEndResp(string msg)
        {
	        YunvaLogPrint.YvDebugLog("SynthesizeSentenceEndResp", string.Format("JsonMsg:{0}", msg));
	        if (synthesizeSentenceAction != null) {
	        synthesizeSentenceAction (msg);
	        synthesizeSentenceAction = null;
	        }
        }

        public void onConnectFail(string msg)
        {
            YunvaLogPrint.YvDebugLog("onConnectFail", string.Format("JsonMsg:{0}", msg));
            YvEventListenerManager.Invoke(YvListenerEven.onConnectFail, msg);
        }

        public void onReconnectSuccess(string msg)
        {
            YunvaLogPrint.YvDebugLog("onReconnectSuccess", string.Format("JsonMsg:{0}", msg));
            YvEventListenerManager.Invoke(YvListenerEven.onReconnectSuccess, msg);
        }
		#endregion
	}
}


