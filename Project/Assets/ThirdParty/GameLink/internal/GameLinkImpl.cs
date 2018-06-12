//using AOT;
//using System;
//using System.IO;
//using UnityEngine;
//using System.Collections;
//using System.Runtime.InteropServices;
//using System.Collections.Generic;

//namespace gamelink
//{
//    enum GLUnity3dCommand
//    {
//        Init,
//        EnableRecognition,
//        Pump,
//        Login,
//        SetProfile,

//        Logout,
//        GetPermanentChannels,
//        JoinChannel,
//        LeaveChannel,
//        SendText,

//        SendCustomText,
//        ClearMessageList,
//        GetMessageList,
//        StartRecord,
//        StopRecord,

//        UploadVoice,
//        DownloadVoice,
//        DownloadVoiceMessage,
//        SendVoice,
//        PlayVoice,

//        PlayVoiceMessage,
//        StopPlay,
//        GetVersion,
//        SetCustomHost,
//        ClearCache,

//        EnableLog,
//        GetPlayingState,
//        GetRecordingState,
//        GetChannelMemberList,
//        GetHistoryMessageList,

//        JoinIntercom,
//        QuitIntercom,
//        StartTalk,
//        StopTalk,
//    }

//    [Serializable]
//    public struct GLMessageList
//    {
//        public GLMessage[] MessageList;
//    };

//    [Serializable]
//    public struct GLVoiceState
//    {
//        public float Volume;
//        public uint Duration;
//    };
		
//    public delegate void NativeDispatchDelegate (int code, String p0, String p1, String p2, String p3, String p4, String p5);

//    public class GameLinkImpl: MonoBehaviour
//    {
//        static bool inited = false;
//        public static bool LogEnabled = false;

//        public static bool Init (String appKey)
//        {
//            if (!inited) {
//                inited = true;

//                LoadAndroidLibrary ();

//                gamelink_setcallback (NativeDispatch);

//                GLEventDispatcher.Reset ();
			
//                GameObject gameObject = new GameObject ("GameLink");
//                GameObject.DontDestroyOnLoad(gameObject);
//                gameObject.AddComponent<GameLinkImpl>();
//            }

//            checkString (ref appKey);

//            string ret = NativeInvoke (GLUnity3dCommand.Init, appKey);

//            return (ret == null);
//        }

//        public static void EnableLog(bool enable)
//        {
//            LogEnabled = enable;
//            NativeInvoke (GLUnity3dCommand.EnableLog, (long)(enable?1:0));
//        }

//        public static void ClearCache ()
//        {
//            NativeInvoke (GLUnity3dCommand.ClearCache);
//        }

//        public static void EnableVoiceRecognition(bool enable)
//        {
//            NativeInvoke (GLUnity3dCommand.EnableRecognition, (long)(enable?1:0));
//        }

//        static void Pump ()
//        {
//            NativeInvoke (GLUnity3dCommand.Pump);
//        }

//        public static void Login (String account)
//        {
//            checkString (ref account);

//            NativeInvoke (GLUnity3dCommand.Login, account);
//        }

//        public static void SetProfile(String nickname, String avatar,  String extraInfo)
//        {
//            checkString (ref nickname);
//            checkString (ref avatar);
//            checkString (ref extraInfo);

//            NativeInvoke (GLUnity3dCommand.SetProfile, nickname, avatar, extraInfo);
//        }

//        public static void Logout ()
//        {
//            NativeInvoke (GLUnity3dCommand.Logout);
//        }

//        public static void GetPermanentChannels(uint pageIndex, uint countPerPage)
//        {
//            NativeInvoke (GLUnity3dCommand.GetPermanentChannels, (long)pageIndex, (long)countPerPage);
//        }
			
//        public static void JoinChannel (String channel)
//        {
//            checkString (ref channel);

//            NativeInvoke (GLUnity3dCommand.JoinChannel, channel);
//        }

//        public static void LeaveChannel (String channel)
//        {
//            checkString (ref channel);

//            NativeInvoke (GLUnity3dCommand.LeaveChannel, channel);
//        }

//        public static void JoinIntercomOfChannel (String channel)
//        {
//            checkString (ref channel);

//            NativeInvoke (GLUnity3dCommand.JoinIntercom, channel);
//        }

//        public static void QuitIntercom ()
//        {
//            NativeInvoke (GLUnity3dCommand.QuitIntercom);
//        }

//        public static void GetChannelMemberList(string channel, uint pageIndex, uint countPerPage)
//        {
//            checkString (ref channel);

//            NativeInvoke (GLUnity3dCommand.GetChannelMemberList, channel, (long)pageIndex, (long)countPerPage);
//        }

//        public static void SendText (GLTargetType type, string account, string text, string extra)
//        {
//            checkString (ref account);
//            checkString (ref text);
//            checkString (ref extra);

//            NativeInvoke (GLUnity3dCommand.SendText, (long)type, account, text, extra);
//        }

//        public static void SendCustomText (GLTargetType type, string account, string text, string extra)
//        {
//            checkString (ref account);
//            checkString (ref text);
//            checkString (ref extra);

//            NativeInvoke (GLUnity3dCommand.SendCustomText, (long)type, account, text, extra);
//        }

//        public static void ClearMessageList(GLTargetType type, string account)
//        {
//            checkString (ref account);

//            NativeInvoke (GLUnity3dCommand.ClearMessageList, (long)type, account);
//        }

//        public static List<GLMessage> GetMessageList(GLTargetType type, string account, uint count)
//        {
//            checkString (ref account);

//            String json = NativeInvoke (GLUnity3dCommand.GetMessageList, (long)type, account, (long)count);
//            GLMessageList list = JsonUtility.FromJson<GLMessageList> (json);

//            return new List<GLMessage> (list.MessageList);
//        }

//        public static void GetHistoryMessageList(GLTargetType type, String account, uint count)
//        {
//            checkString (ref account);

//            NativeInvoke (GLUnity3dCommand.GetHistoryMessageList, (long)type, account, (long)count);
//        }

//        public static void StartRecord()
//        {
//            NativeInvoke (GLUnity3dCommand.StartRecord);
//        }

//        public static void GetRecordingState(ref float volume, ref uint duration)
//        {
//            String json = NativeInvoke(GLUnity3dCommand.GetRecordingState);

//            GLVoiceState state = JsonUtility.FromJson<GLVoiceState>(json);
//            volume = state.Volume;
//            duration = state.Duration;
//        }

//        public static void StopRecord()
//        {
//            NativeInvoke (GLUnity3dCommand.StopRecord);
//        }

//        public static void StartTalk()
//        {
//            NativeInvoke (GLUnity3dCommand.StartTalk);
//        }

//        public static void StopTalk()
//        {
//            NativeInvoke (GLUnity3dCommand.StopTalk);
//        }

//        public static void UploadVoice(String file)
//        {
//            checkString (ref file);

//            NativeInvoke (GLUnity3dCommand.UploadVoice, file);
//        }

//        public static void DownloadVoice(String url)
//        {
//            checkString (ref url);

//            NativeInvoke (GLUnity3dCommand.DownloadVoice, url);
//        }

//        public static void DownloadVoiceMessage(uint messageId)
//        {
//            NativeInvoke (GLUnity3dCommand.DownloadVoiceMessage, (long)messageId);
//        }

//        public static void SendVoice(GLTargetType type, String account, String url, uint duration, String extra, String voiceContent)
//        {
//            checkString (ref account);
//            checkString (ref url);
//            checkString (ref extra);
//            checkString (ref voiceContent);

//            NativeInvoke (GLUnity3dCommand.SendVoice, (long)type, account, url, (long)duration, extra, voiceContent);
//        }

//        public static void PlayVoice(String file)
//        {
//            checkString (ref file);

//            NativeInvoke (GLUnity3dCommand.PlayVoice, file);
//        }

//        public static void PlayVoiceMessage(uint messageId)
//        {
//            NativeInvoke (GLUnity3dCommand.PlayVoiceMessage, (long)messageId);
//        }

//        public static void GetPlayingState(ref float volume, ref uint duration)
//        {
//            String json = NativeInvoke(GLUnity3dCommand.GetPlayingState);

//            GLVoiceState state = JsonUtility.FromJson<GLVoiceState>(json);
//            volume = state.Volume;
//            duration = state.Duration;
//        }

//        public static void StopPlay()
//        {
//            NativeInvoke (GLUnity3dCommand.StopPlay);
//        }

//        public static String GetVersion()
//        {
//            String version = NativeInvoke (GLUnity3dCommand.GetVersion);

//            return version;
//        }

//        public static void SetCustomHost (string host, long port)
//        {
//            checkString (ref host);

//            NativeInvoke (GLUnity3dCommand.SetCustomHost, host, port);
//        }

//        // Update is called once per frame
//        void Update ()
//        {
//            if (inited) {
//                Pump ();
//            }
//        }

//        private void OnApplicationQuit ()
//        {
//            Logout ();
//        }

//        /*
//        protected override void onAppQuit()
//        {
//            Logout ();
//        }
//        */

//        private static void checkString(ref String s)
//        {
//            if (String.IsNullOrEmpty (s)) {
//                s = String.Empty;
//            }
//        }

//        protected static void LoadAndroidLibrary()
//        {
//            #if UNITY_ANDROID
//            AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
//            AndroidJavaObject activity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");
//            AndroidJavaClass classAPI = new AndroidJavaClass("me.gamelink.GameLink");
//            classAPI.CallStatic<AndroidJavaObject>("createInstance",activity);
//            #endif
//        }

//        static String PtrToUTF8String(IntPtr ptr)
//        {
//            if(ptr == IntPtr.Zero){
//                return null;
//            }

//            int len = 0;
//            while (true){
//                byte ch = Marshal.ReadByte(ptr, len);
//                len++;
//                if (ch == 0){
//                    break;
//                }
//            }

//            byte[] test = new byte[len - 1];
//            Marshal.Copy(ptr, test, 0, len - 1);
//            return System.Text.Encoding.UTF8.GetString(test, 0, len - 1);
//        }

//        static String NativeInvoke (GLUnity3dCommand command, params object[] arguments)
//        {
//            List<long> longList = new List<long> ();
//            List<String> stringList = new List<String> ();

//            foreach (object arg in arguments) {
//                if (arg is String) {
//                    stringList.Add ((String)arg);
//                } else {
//                    longList.Add ((long)arg);
//                }
//            }

//            IntPtr result = gamelink_invoke ((int)command, 
//                                longList.ToArray (),
//                                longList.Count,
//                                stringList.ToArray (),
//                                stringList.Count);

//#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
//            return PtrToUTF8String(result);
//#else
//            return Marshal.PtrToStringAuto (result);
//#endif
//        }

//        #if UNITY_IOS
//        [MonoPInvokeCallback(typeof(NativeDispatchDelegate))]
//        #endif
//        static void NativeDispatch (int code, String p0, String p1, String p2, String p3, String p4, String p5)
//        {
//            String[] args = { p0, p1, p2, p3, p4, p5};
//            GLEventDispatcher.Dispatch ((GLEventCode)code, args);
//        }

//        #if UNITY_IOS
//        [DllImport ("__Internal")]
//        #elif UNITY_STANDALONE_OSX ||UNITY_EDITOR_OSX || UNITY_ANDROID
//        [DllImport("GameLink")]
//        #elif UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
//        [DllImport("GameLink.dll", CallingConvention = CallingConvention.Cdecl)]
//        #endif
//        static extern int gamelink_setcallback (NativeDispatchDelegate callback);

//        #if UNITY_IOS
//        [DllImport ("__Internal")]
//        #elif UNITY_STANDALONE_OSX ||UNITY_EDITOR_OSX || UNITY_ANDROID
//        [DllImport("GameLink")]
//        #elif UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
//        [DllImport("GameLink.dll", CallingConvention = CallingConvention.Cdecl)]
//        #endif
//        static extern IntPtr gamelink_invoke (int cmd, long[] llargs, int llcount, String[] strargs, int strcount);
//    }



//#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

//#if UNITY_EDITOR

//    [UnityEditor.InitializeOnLoad]

//#endif
//    class GLWinLibraryHelper

//    {

//        static GLWinLibraryHelper()

//        {

//            var currentPath = Environment.GetEnvironmentVariable("PATH",

//        EnvironmentVariableTarget.Process);


//#if UNITY_EDITOR_32

//            var dllPath = Application.dataPath

//                + Path.DirectorySeparatorChar + "Plugins"

//                + Path.DirectorySeparatorChar + "x86";




//#elif UNITY_EDITOR_64

//    var dllPath = Application.dataPath

//        + Path.DirectorySeparatorChar + "Plugins"

//        + Path.DirectorySeparatorChar + "x64";

//#else // Player

//    var dllPath = Application.dataPath

//        + Path.DirectorySeparatorChar + "Plugins";

//#endif

//            if (currentPath != null && currentPath.Contains(dllPath) == false)

//                Environment.SetEnvironmentVariable("PATH", currentPath + Path.PathSeparator

//                    + dllPath, EnvironmentVariableTarget.Process);

//        }

//    }

//#endif
//}







