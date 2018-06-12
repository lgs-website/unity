//using System;
//using UnityEngine;
//using System.Collections.Generic;
//using System.Reflection;

//namespace gamelink
//{
//    public enum GLEventCode
//    {
//        OnLogin,
//        OnLogout,
//        OnJoinChannel,
//        OnLeaveChannel,
//        OnGetPermanentChannels,

//        OnSendMessage,
//        OnReceiveMessage,
//        OnStartRecord,
//        OnRecording,
//        OnStopRecord,

//        OnUploadVoice,
//        OnDownloadVoice,
//        OnDownloadVoiceMessage,
//        OnPlayStart,
//        OnPlaying,

//        OnPlayStop,
//        OnPlayMessageStart,
//        OnPlayingMessage,
//        OnPlayMessageStop,
//        OnGetChannelMemberList,

//        OnGetHistoryMessageList,
//        OnJoinIntercomOfChannel,
//        OnQuitIntercom,
//        OnStartTalk,
//        OnStopTalk,
//    }

//    [Serializable]
//    public struct GLChannelList
//    {
//        public GLTarget[] ChannelList;
//    };

//    [Serializable]
//    public struct GLChannelMemberList
//    {
//        public String[] MemberList;
//    };

//    public class GLEventDispatcher
//    {
//        protected static Dictionary<object ,Dictionary<GLEventCode, MethodInfo>> observerDictionary;

//        public static void Reset ()
//        {
//            if (observerDictionary == null) {
//                observerDictionary = new Dictionary<object, Dictionary<GLEventCode, MethodInfo>> ();		
//            }

//            observerDictionary.Clear ();
//        }

//        protected static void addMethod(object observer, GLEventCode code, params Type[] paramTypes)
//        {
//            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
//            MethodInfo[] methods = observer.GetType ().GetMethods (flags);
//            GLEventCode codeForComparingName = code;

//            //for overriding callback methods
//            if (code == GLEventCode.OnPlayMessageStart) {
//                codeForComparingName = GLEventCode.OnPlayStart;
//            } else if (code == GLEventCode.OnPlayingMessage) {
//                codeForComparingName = GLEventCode.OnPlaying;
//            } else if (code == GLEventCode.OnPlayMessageStop) {
//                codeForComparingName = GLEventCode.OnPlayStop;
//            } else if (code == GLEventCode.OnDownloadVoiceMessage) {
//                codeForComparingName = GLEventCode.OnDownloadVoice;
//            }

//            foreach(MethodInfo method in methods){
//                //compare method name
//                if(!method.Name.Equals(codeForComparingName.ToString())){
//                    continue;
//                }

//                //compare method parameter count
//                ParameterInfo[] paramInfos = method.GetParameters ();
//                if (paramInfos.Length != paramTypes.Length) {
//                    continue;
//                }

//                //compare method parameter type
//                bool matched = true;
//                for (int i = 0; i < paramInfos.Length; i++) {
//                    if (paramInfos [i].ParameterType != paramTypes [i]) {
//                        matched = false;
//                        break;
//                    }
//                }

//                if (!matched) {
//                    continue;
//                }

//                Dictionary<GLEventCode, MethodInfo> methodDictionary = observerDictionary[observer];
//                methodDictionary.Add (code,	method);
//            }
//        }
			
//        public static void AddObserver (object observer)
//        {
//            if (observer == null || observerDictionary == null || observerDictionary.ContainsKey(observer)) {
//                return;
//            }

//            observerDictionary.Add (observer, new Dictionary<GLEventCode, MethodInfo> ());

//            addMethod (observer, GLEventCode.OnLogin, typeof(GLTarget), typeof(GLError));
//            addMethod (observer, GLEventCode.OnLogout, typeof(GLError));
//            addMethod (observer, GLEventCode.OnJoinChannel, typeof(GLTarget), typeof(GLError));
//            addMethod (observer, GLEventCode.OnLeaveChannel, typeof(GLTarget), typeof(GLError));
//            addMethod (observer, GLEventCode.OnGetPermanentChannels, typeof(uint), typeof(List<GLTarget>), typeof(GLError));

//            addMethod (observer, GLEventCode.OnSendMessage, typeof(GLMessage), typeof(GLError));
//            addMethod (observer, GLEventCode.OnReceiveMessage, typeof(GLMessage));
//            addMethod (observer, GLEventCode.OnStartRecord, typeof(GLError));
//            addMethod (observer, GLEventCode.OnRecording, typeof(float), typeof(uint));
//            addMethod (observer, GLEventCode.OnStopRecord, typeof(String), typeof(String), typeof(uint), typeof(GLError));

//            addMethod (observer, GLEventCode.OnUploadVoice, typeof(String), typeof(String), typeof(GLError));
//            addMethod (observer, GLEventCode.OnDownloadVoice, typeof(String), typeof(String), typeof(GLError));
//            addMethod (observer, GLEventCode.OnDownloadVoiceMessage, typeof(GLMessage), typeof(GLError));
//            addMethod (observer, GLEventCode.OnPlayMessageStart, typeof(GLMessage), typeof(GLError));
//            addMethod (observer, GLEventCode.OnPlayingMessage, typeof(GLMessage), typeof(uint));

//            addMethod (observer, GLEventCode.OnPlayMessageStop, typeof(GLMessage));
//            addMethod (observer, GLEventCode.OnPlayStart, typeof(String), typeof(GLError));
//            addMethod (observer, GLEventCode.OnPlaying, typeof(String), typeof(uint));
//            addMethod (observer, GLEventCode.OnPlayStop, typeof(String));
//            addMethod (observer, GLEventCode.OnGetChannelMemberList, typeof(GLTarget), typeof(uint), typeof(uint), typeof(List<String>), typeof(GLError));
		
//            addMethod (observer, GLEventCode.OnGetHistoryMessageList, typeof(GLTarget), typeof(uint), typeof(GLError));
//            addMethod (observer, GLEventCode.OnJoinIntercomOfChannel, typeof(GLTarget), typeof(GLError));
//            addMethod (observer, GLEventCode.OnQuitIntercom, typeof(GLError));
//            addMethod (observer, GLEventCode.OnStartTalk, typeof(GLTarget), typeof(GLError));
//            addMethod (observer, GLEventCode.OnStopTalk, typeof(GLTarget), typeof(GLError));
//        }

//        public static void RemoveObserver(object observer)
//        {
//            if (observer == null || observerDictionary == null || !observerDictionary.ContainsKey (observer)) {
//                return;
//            }

//            observerDictionary.Remove (observer);
//        }

//        public static void RemoveAllObservers()
//        {
//            if (observerDictionary != null) {
//                observerDictionary.Clear ();
//            }
//        }

//        protected static void DoDispatch(GLEventCode code, params object[] parameters)
//        {
//            foreach (object observer in observerDictionary.Keys) {
//                Dictionary<GLEventCode, MethodInfo> methodDictionary = observerDictionary[observer];
//                if (methodDictionary.ContainsKey (code)) {
//                    MethodInfo method = methodDictionary[code];
//                    method.Invoke (observer, parameters);
//                }
//            }
//        }

//        public static void Dispatch(GLEventCode code, string[] parameters)
//        {
//            if (GameLinkImpl.LogEnabled) {
//                Debug.Log ("dispatch(" + code + "):");

//                for (int i = 0; i < parameters.Length; i++) {
//                    if (parameters [i] != null && parameters [i].Length > 0) {
//                        Debug.Log ("param[" + (i + 1) + "]:" + parameters [i]);
//                    }
//                }
//            }
				
//            switch (code) {
//            case GLEventCode.OnLogin:
//                {
//                    GLTarget user = JsonUtility.FromJson<GLTarget> (parameters [0]);
//                    GLError error = JsonUtility.FromJson<GLError> (parameters [1]);
//                    if (error.Code == GLErrorCode.NoError) {
//                        error = null;
//                    }

//                    DoDispatch (code, user, error);
//                }
//                break;

//            case GLEventCode.OnLogout:
//                {
//                    GLError error = JsonUtility.FromJson<GLError> (parameters [0]);
//                    if (error.Code == GLErrorCode.NoError) {
//                        error = null;
//                    }

//                    DoDispatch (code, error);
//                }
//                break;

//            case GLEventCode.OnJoinChannel:
//            case GLEventCode.OnLeaveChannel:
//                {
//                    GLTarget channel = JsonUtility.FromJson<GLTarget> (parameters [0]);
//                    GLError error = JsonUtility.FromJson<GLError> (parameters [1]);
//                    if (error.Code == GLErrorCode.NoError) {
//                        error = null;
//                    }

//                    DoDispatch (code, channel, error);
//                }
//                break;

//            case GLEventCode.OnGetPermanentChannels:
//                {
//                    uint pageIndex = uint.Parse (parameters [0]);
//                    GLChannelList list = JsonUtility.FromJson<GLChannelList> (parameters [1]);
//                    List<GLTarget> channels = new List<GLTarget> (list.ChannelList);
//                    GLError error = JsonUtility.FromJson<GLError> (parameters [2]);
//                    if (error.Code == GLErrorCode.NoError) {
//                        error = null;
//                    }

//                    DoDispatch (code, pageIndex, channels, error);
//                }
//                break;
			
//            case GLEventCode.OnSendMessage:
//                {
//                    GLMessage message = JsonUtility.FromJson<GLMessage> (parameters [0]);
//                    GLError error = JsonUtility.FromJson<GLError> (parameters [1]);
//                    if (error.Code == GLErrorCode.NoError) {
//                        error = null;
//                    }

//                    DoDispatch (code, message, error);
//                }
//                break;

//            case GLEventCode.OnReceiveMessage:
//                {
//                    GLMessage message = JsonUtility.FromJson<GLMessage> (parameters [0]);
//                    DoDispatch (code, message);
//                }
//                break;

//            case GLEventCode.OnStartRecord:
//                {
//                    GLError error = JsonUtility.FromJson<GLError> (parameters [0]);
//                    if (error.Code == GLErrorCode.NoError) {
//                        error = null;
//                    }

//                    DoDispatch (code, error);
//                }
//                break;

//            case GLEventCode.OnRecording:
//                {
//                    float volume = float.Parse (parameters [0]);
//                    uint duration = uint.Parse (parameters [1]);

//                    DoDispatch (code, volume, duration);
//                }
//                break;

//            case GLEventCode.OnStopRecord:
//                {
//                    String file = parameters[0];
//                    String voiceContent = parameters[1];
//                    uint duration = uint.Parse (parameters [2]);
//                    GLError error = JsonUtility.FromJson<GLError> (parameters [3]);
//                    if (error.Code == GLErrorCode.NoError) {
//                        error = null;
//                    }

//                    DoDispatch (code, file, voiceContent, duration, error);
//                }
//                break;

//            case GLEventCode.OnUploadVoice:
//                {
//                    String file = parameters[0];
//                    String url = parameters[1];
//                    GLError error = JsonUtility.FromJson<GLError> (parameters [2]);
//                    if (error.Code == GLErrorCode.NoError) {
//                        error = null;
//                    }

//                    DoDispatch (code, file, url, error);
//                }
//                break;

//            case GLEventCode.OnDownloadVoice:
//                {
//                    String url = parameters[0];
//                    String file = parameters[1];
//                    GLError error = JsonUtility.FromJson<GLError> (parameters [2]);
//                    if (error.Code == GLErrorCode.NoError) {
//                        error = null;
//                    }

//                    DoDispatch (code, url, file, error);
//                }
//                break;

//            case GLEventCode.OnDownloadVoiceMessage:
//                {
//                    GLMessage message = JsonUtility.FromJson<GLMessage> (parameters [0]);
//                    GLError error = JsonUtility.FromJson<GLError> (parameters [1]);
//                    if (error.Code == GLErrorCode.NoError) {
//                        error = null;
//                    }

//                    DoDispatch (code, message, error);
//                }
//                break;

//            case GLEventCode.OnPlayMessageStart:
//                {
//                    GLMessage message = JsonUtility.FromJson<GLMessage> (parameters [0]);
//                    GLError error = JsonUtility.FromJson<GLError> (parameters [1]);
//                    if (error.Code == GLErrorCode.NoError) {
//                        error = null;
//                    }

//                    DoDispatch (code, message, error);
//                }
//                break;

//            case GLEventCode.OnPlayingMessage:
//                {
//                    GLMessage message = JsonUtility.FromJson<GLMessage> (parameters [0]);
//                    uint duration = uint.Parse (parameters [1]);

//                    DoDispatch (code, message, duration);
//                }
//                break;

//            case GLEventCode.OnPlayMessageStop:
//                {
//                    GLMessage message = JsonUtility.FromJson<GLMessage> (parameters [0]);
//                    DoDispatch (code, message);
//                }
//                break;

//            case GLEventCode.OnPlayStart:
//                {
//                    String file = parameters [0];
//                    GLError error = JsonUtility.FromJson<GLError> (parameters [1]);
//                    if (error.Code == GLErrorCode.NoError) {
//                        error = null;
//                    }

//                    DoDispatch (code, file, error);
//                }
//                break;

//            case GLEventCode.OnPlaying:
//                {
//                    String file = parameters [0];
//                    uint duration = uint.Parse (parameters [1]);

//                    DoDispatch (code, file, duration);
//                }
//                break;

//            case GLEventCode.OnPlayStop:
//                {
//                    String file = parameters [0];

//                    DoDispatch (code, file);
//                }
//                break;

//            case GLEventCode.OnGetChannelMemberList:
//                {
//                    GLTarget channel = JsonUtility.FromJson<GLTarget> (parameters [0]);
//                    uint pageIndex = uint.Parse (parameters [1]);
//                    uint total = uint.Parse (parameters [2]);
//                    GLChannelMemberList list = JsonUtility.FromJson<GLChannelMemberList> (parameters [3]);
//                    List<String> members = new List<String> (list.MemberList);
//                    GLError error = JsonUtility.FromJson<GLError> (parameters [4]);
//                    if (error.Code == GLErrorCode.NoError) {
//                        error = null;
//                    }

//                    DoDispatch (code, channel, pageIndex, total, members, error);
//                }
//                break;

//            case GLEventCode.OnGetHistoryMessageList:
//                {
//                    GLTarget target = JsonUtility.FromJson<GLTarget> (parameters [0]);
//                    uint count = uint.Parse (parameters [1]);
//                    GLError error = JsonUtility.FromJson<GLError> (parameters [2]);
//                    if (error.Code == GLErrorCode.NoError) {
//                        error = null;
//                    }

//                    DoDispatch (code, target, count, error);
//                }
//                break;

//            case GLEventCode.OnJoinIntercomOfChannel:
//                {
//                    GLTarget channel = JsonUtility.FromJson<GLTarget> (parameters [0]);
//                    GLError error = JsonUtility.FromJson<GLError> (parameters [1]);
//                    if (error.Code == GLErrorCode.NoError) {
//                        error = null;
//                    }

//                    DoDispatch (code, channel, error);
//                }
//                break;

//            case GLEventCode.OnQuitIntercom:
//                {
//                    GLError error = JsonUtility.FromJson<GLError> (parameters [0]);
//                    if (error.Code == GLErrorCode.NoError) {
//                        error = null;
//                    }

//                    DoDispatch (code, error);
//                }
//                break;

//            case GLEventCode.OnStartTalk:
//                {
//                    GLTarget user = JsonUtility.FromJson<GLTarget> (parameters [0]);
//                    GLError error = JsonUtility.FromJson<GLError> (parameters [1]);
//                    if (error.Code == GLErrorCode.NoError) {
//                        error = null;
//                    }

//                    DoDispatch (code, user, error);
//                }
//                break;

//            case GLEventCode.OnStopTalk:
//                {
//                    GLTarget user = JsonUtility.FromJson<GLTarget> (parameters [0]);
//                    GLError error = JsonUtility.FromJson<GLError> (parameters [1]);
//                    if (error.Code == GLErrorCode.NoError) {
//                        error = null;
//                    }

//                    DoDispatch (code, user, error);
//                }
//                break;

//            default:
//                break;
//            }
//        }
//    }
//}

