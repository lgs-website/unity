//using AOT;
//using System;
//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.InteropServices;


namespace gamelink
{ }
//{
//    public class GameLink
//    {
//        public static bool Init(String appKey)
//        {
//            return GameLinkImpl.Init (appKey);
//        }

//        public static void SetCustomHost (string host, long port)
//        {
//            GameLinkImpl.SetCustomHost (host, port);
//        }

//        public static void EnableLog(bool enable)
//        {
//            GameLinkImpl.EnableLog (enable);
//        }

//        public static void ClearCache ()
//        {
//            GameLinkImpl.ClearCache ();
//        }

//        public static void EnableVoiceRecognition(bool enable)
//        {
//            GameLinkImpl.EnableVoiceRecognition (enable);
//        }

//        public static void RemoveAllObservers()
//        {
//            GLEventDispatcher.RemoveAllObservers ();
//        }

//        public static void AddObserver (object observer)
//        {
//            GLEventDispatcher.AddObserver (observer);
//        }

//        public static void RemoveObserver(object observer)
//        {
//            GLEventDispatcher.RemoveObserver (observer);
//        }

//        public static void Login (String account)
//        {
//            GameLinkImpl.Login (account);
//        }

//        public static void SetProfile(String nickname, String avatar,  String extraInfo)
//        {
//            GameLinkImpl.SetProfile (nickname, avatar, extraInfo);
//        }

//        public static void Logout ()
//        {
//            GameLinkImpl.Logout ();
//        }

//        public static void GetPermanentChannels(uint pageIndex, uint countPerPage)
//        {
//            GameLinkImpl.GetPermanentChannels (pageIndex, countPerPage);
//        }

//        public static void JoinChannel (String channel)
//        {
//            GameLinkImpl.JoinChannel (channel);
//        }

//        public static void JoinChannel (GLTarget channel)
//        {
//            GameLinkImpl.JoinChannel (channel.Account);
//        }

//        public static void JoinIntercomOfChannel (GLTarget channel)
//        {
//            GameLinkImpl.JoinIntercomOfChannel (channel.Account);
//        }

//        public static void QuitIntercom ()
//        {
//            GameLinkImpl.QuitIntercom ();
//        }

//        public static void LeaveChannel (String channel)
//        {
//            GameLinkImpl.LeaveChannel (channel);
//        }

//        public static void LeaveChannel (GLTarget channel)
//        {
//            GameLinkImpl.LeaveChannel (channel.Account);
//        }

//        public static void GetChannelMemberList(String channel, uint pageIndex, uint countPerPage)
//        {
//            GameLinkImpl.GetChannelMemberList (channel, pageIndex, countPerPage);
//        }

//        public static void GetChannelMemberList(GLTarget channel, uint pageIndex, uint countPerPage)
//        {
//            GameLinkImpl.GetChannelMemberList (channel.Account, pageIndex, countPerPage);
//        }

//        public static void SendText (GLTarget receiver, String text, String extra)
//        {
//            GameLinkImpl.SendText (receiver.Type, receiver.Account, text, extra);
//        }

//        public static void SendCustomText (GLTarget receiver, String text, String extra)
//        {
//            GameLinkImpl.SendCustomText (receiver.Type, receiver.Account, text, extra);
//        }

//        public static void ClearMessageList(GLTarget target)
//        {
//            GameLinkImpl.ClearMessageList (target.Type, target.Account);
//        }

//        public static List<GLMessage> GetMessageList(GLTarget target, uint count)
//        {
//            return GameLinkImpl.GetMessageList (target.Type, target.Account, count);
//        }

//        public static void GetHistoryMessageList(GLTarget target, uint count)
//        {
//            GameLinkImpl.GetHistoryMessageList (target.Type, target.Account, count);
//        }

//        public static void StartRecord()
//        {
//            GameLinkImpl.StartRecord ();
//        }

//        public static void GetRecordingState(ref float volume, ref uint duration)
//        {
//            GameLinkImpl.GetRecordingState(ref volume,ref duration);
//        }

//        public static void StopRecord()
//        {
//            GameLinkImpl.StopRecord ();
//        }

//        public static void StartTalk()
//        {
//            GameLinkImpl.StartTalk ();
//        }

//        public static void StopTalk()
//        {
//            GameLinkImpl.StopTalk ();
//        }

//        public static void UploadVoice(String file)
//        {
//            GameLinkImpl.UploadVoice (file);
//        }

//        public static void DownloadVoice(String url)
//        {
//            GameLinkImpl.DownloadVoice (url);
//        }

//        public static void DownloadVoice(GLMessage message)
//        {
//            GameLinkImpl.DownloadVoiceMessage (message.Id);
//        }

//        public static void SendVoice(GLTarget receiver, String url, uint duration, String extra, String voiceContent)
//        {
//            GameLinkImpl.SendVoice (receiver.Type, receiver.Account, url, duration, extra, voiceContent);
//        }

//        public static void PlayVoice(String file)
//        {
//            GameLinkImpl.PlayVoice (file);
//        }

//        public static void PlayVoice(GLMessage message)
//        {
//            GameLinkImpl.PlayVoiceMessage (message.Id);
//        }

//        public static void GetPlayingState(ref float volume, ref uint duration)
//        {
//            GameLinkImpl.GetPlayingState(ref volume, ref duration);
//        }

//        public static void StopPlay()
//        {
//            GameLinkImpl.StopPlay ();
//        }

//        public static String GetVersion()
//        {
//            return GameLinkImpl.GetVersion ();
//        }
//    }
//}
