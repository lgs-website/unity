using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace YunvaVideoTroops {
	public class IOSInterface {

		#region 实时语音部份
		[DllImport("__Internal")]
		public static extern void YvChatSDKInit(string appId, bool isTest, string observer);
		[DllImport("__Internal")]
		public static extern void YvChatSDKInitWithenvironment(int environment,string appId,string observer);

		[DllImport("__Internal")]
		public static extern void loginWithSeq(string seq);
	
		[DllImport("__Internal")]
		public static extern void loginBindingWithTT(string tt,string seq);

		[DllImport("__Internal")]
		public static extern void logout();

		[DllImport("__Internal")]
		public static extern void releaseResource();

		[DllImport("__Internal")]
		public static extern void chatMic(bool onOff, string expand);
		[DllImport("__Internal")]
		public static extern  void  ChatMicWithTimeLimit (int timeLimit, string expand);

		[DllImport("__Internal")]
		public static extern void setPausePlayRealAudio(bool isPause);

		[DllImport("__Internal")]
		public static extern bool isPausePlayRealAudio();

		[DllImport("__Internal")]
		public static extern void micModeSettingReq(int modeType);

		[DllImport("__Internal")]
		public static extern void YvSetLogLevel(int logLevel);

		#endregion

		#region 发送消息接口部份
		/**发送文本信息*/
		[DllImport("__Internal")]
		public static extern void sendTextMessage (string text, string expand);

//		/**发送语音留言*/
		[DllImport("__Internal")]
		public static extern void sendVoiceMessage(string filePath,int voiceDuration,string expand);
		[DllImport("__Internal")]
		public static extern void sendVoiceMessageWithVoiceUrl (string voiceUrl, int voiceDuration, string expand);

//		/**发送文字+语音留言*/
		[DllImport("__Internal")]
		public static extern void sendRichMessage (string text, string filePath, int voiceDuration, string expand);
//
//		[DllImport("__Internal")]
//		public static extern void sendRichMessageWithVoiceUrl (string text, string voiceUrl, int voiceDuration, string expand);

//		/**设置是否自动播放收到的语音留言消息,默认为NO*/
		[DllImport("__Internal")]
		public static extern void setIsAutoPlayVoiceMessage (bool isAutoPlayVoiceMessage);

		[DllImport("__Internal")]
		public static extern bool isAutoPlayVoiceMessage ();
		#endregion

		#region
//		//语音文件上传接口
		[DllImport("__Internal")]
		public static extern void uploadVoiceFile(string voiceFilePath,int fileRetainTimeType,string expand);


	

		//语音识别+语音文件上传
		[DllImport("__Internal")]
		public static extern void httpVoiceRecognizeReqAndUploadVoiceFile (int recognizeLanguage,
		                                                                     int outputTextLanguageType, string voivoiceFilePath, int voiceDuration, int fileRetainTimeType,
		                                                                     string expand);
        //语音识别url
        [DllImport("__Internal")]
        public static extern void speechDiscernByUrl(int recognizeLanguage,
                                                     int outputTextLanguageType, 
                                                     string UrlFilePath, 
                                                     string expand);

		/*! 语音下载
 		@method
 		@brief 在【wifi】情况下下载语音文件到缓存中(不是wifi不下载)
 		注:AudioTools类的播放voiceUrl语音接口会先从缓存中查询是否有已下载的缓存文件播放，如果没下载再去下载播放。
 		@param voiceUrl 文件下载url
 		*/
		[DllImport("__Internal")]
		public static extern void downloadVoiceFileToCacheWhenWifi (string voiceUrl);

//		/**
//     	*  获取历史消息
//     	*  @param pageIndex             查询的是第几页(从0开始)
//    	*  @param pageSize              每页的消息个数
//    	*/
		[DllImport("__Internal")]
		public static extern void queryHistoryMsgReqWithPageIndex(int pageIndex, int pageSize);

		/**
    	 *  获取房间用户列表
     	*/
		[DllImport("__Internal")]
		public static extern void ChatGetTroopsListReq();



		#region 录音工具
		/*!
    	 @method
     	@brief 初始化
     	@param observer 回调接收对象
     	*/
		[DllImport("__Internal")]
		public static extern void audioTools_Init (string observer);

//		/*!
//     	@method
//    	@brief 开始语音录制
//     	@param minMillseconds 识别录音最短时间(录音少于该时间不做处理)
//     	@param maxMillseconds 录音最长时间(超过该时间会自动停止录制)
//     	*/
		[DllImport("__Internal")]
		public static extern void audioTools_startRecord(int minMillseconds,int maxMillseconds);

//		/*!
//    	 @method
//     	@brief 停止语音录制
//     	*/
		[DllImport("__Internal")]
		public static extern void audioTools_stopRecord();

//		/*!
//     	@method
//     	@brief 查询是否正在录制
//     	*/
		[DllImport("__Internal")]
		public static extern bool audioTools_isRecording();

//		/*!
//     	@method
//     	@brief 播放语音文件
//     	@param 语音文件绝对路径
//     	*/
		[DllImport("__Internal")]
		public static extern int audioTools_playAudio(string filePath);

//		/*!
//     	@method
//     	@brief 在线播放语音
//     	@param 语音文件下载url
//     	*/
		[DllImport("__Internal")]
		public static extern int audioTools_playOnlineAudio(string fileUrl);

//		/*!
//     	@method
//     	@brief 主动停止语音播放
//     	*/
		[DllImport("__Internal")]
		public static extern void audioTools_stopPlayAudio();

//		/*!
//     	@method
//     	@brief 查询当前是否正在播放
//     	*/
		[DllImport("__Internal")]
		public static extern bool audioTools_isPlaying();

//		/*!
//     	@method
//    	 @brief 删除文件
//     
//     	@param filePath -文件路径
//     	*/
		[DllImport("__Internal")]
		public static extern  bool audioTools_deleteFile(string filePath);

        //		/*!
        //     	@method
        //    	 @brief 是否开启音量回调
        //     
        //     	@param enable -true 开启  false 关闭
        //     	*/
        [DllImport("__Internal")]
        public static extern bool audioTools_setMeteringEnabled(bool filePath);

//		[DllImport("__Internal")]
//		public static extern string createRecordAudioFilePath ();

        [DllImport("__Internal")]
        public static extern void synthesizeSentence(string text, string per);
		#endregion
		#endregion
       }
}