using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjectNet.Core.Managers
{
	public class ChatManager : MonoBehaviour, IChatClientListener
	{
		public Action OnSelect = delegate { };
		public Action OnDeselect = delegate { };

		public TMP_Text content;
		public TMP_InputField inputField;
		public ScrollRect scrollRect;
		public int maxLines;

		private ChatClient _chatClient;
		private string[] _channels;
		private string[] _chats;
		private int _currentChat;
		private float _scrollLimit = 0.2f;
		private Dictionary<string, int> _chatDic = new Dictionary<string, int>();

		private void Start()
		{
			if (!PhotonNetwork.IsConnected) return;
			_channels = new[]{"World", PhotonNetwork.CurrentRoom.Name};
			_chats = new string[2];

			_chatDic["World"] = 0;
			_chatDic[PhotonNetwork.CurrentRoom.Name] = 1;

			_chatClient = new ChatClient(this);

			_chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion,
				new AuthenticationValues(PhotonNetwork.LocalPlayer.NickName));
		}

		private void Update()
		{
			_chatClient.Service();
		}

		private void UpdateChatUI()
		{
			content.text = _chats[_currentChat];

			if (content.textInfo.lineCount > maxLines)
			{
				StartCoroutine(WaitToDeleteLines());
			}
			if (scrollRect.verticalNormalizedPosition < _scrollLimit)
			{
				StartCoroutine(WaitToScroll());
			}
		}

		private IEnumerator WaitToScroll()
		{
			yield return new WaitForEndOfFrame();
			scrollRect.verticalNormalizedPosition = 0;
		}

		private IEnumerator WaitToDeleteLines()
		{
			yield return new WaitForEndOfFrame();
			for (var i = 0; i < content.textInfo.lineCount - maxLines; i++)
			{
				var index = _chats[_currentChat].IndexOf("\n", StringComparison.Ordinal);

				_chats[_currentChat] = _chats[_currentChat].Substring(index + 1);
			}
			content.text = _chats[_currentChat];
		}

		public void SendChat()
		{
			if (string.IsNullOrEmpty(inputField.text) || string.IsNullOrWhiteSpace(inputField.text)) return;

			var words = inputField.text.Split(' ');
			
			switch (words[0])
			{
				case "/openDoors":
					ServerManager.Instance.RequestRPC("RequestOpenAllDoors");
					inputField.text = "";
					EventSystem.current.SetSelectedGameObject(null);
					EventSystem.current.SetSelectedGameObject(inputField.gameObject);
					return;
				case "/w" when words.Length > 2:
					_chatClient.SendPrivateMessage(words[1], string.Join(" ", words, 2, words.Length - 2));
					break;
				default:
					_chatClient.PublishMessage(_channels[_currentChat], inputField.text);
					break;
			}

			inputField.text = "";

			EventSystem.current.SetSelectedGameObject(null);
			EventSystem.current.SetSelectedGameObject(inputField.gameObject);
		}

		public void SelectedChat()
		{
			OnSelect();
		}

		public void DeselectedChat()
		{
			OnDeselect();
		}

		void IChatClientListener.DebugReturn(DebugLevel level, string message)
		{
		}

		void IChatClientListener.OnChatStateChange(ChatState state)
		{
		}

		void IChatClientListener.OnConnected()
		{
			print("Connected to Chat");
			_chatClient.Subscribe(_channels);
		}

		public void DebugReturn(DebugLevel level, string message)
		{
			throw new System.NotImplementedException();
		}

		void IChatClientListener.OnDisconnected()
		{
			print("Disconnected from Chat");
		}

		void IChatClientListener.OnGetMessages(string channelName, string[] senders, object[] messages)
		{
			for (var i = 0; i < senders.Length; i++)
			{
				var color = senders[i] == PhotonNetwork.NickName ? "<color=blue>" : "<color=yellow>";
				var indexChat = _chatDic[channelName];
				_chats[indexChat] += color + senders[i] + ": " + "</color>" + messages[i] + "\n";
			}
			UpdateChatUI();
		}

		void IChatClientListener.OnPrivateMessage(string sender, object message, string channelName)
		{
			for (var i = 0; i < _chats.Length; i++)
			{
				_chats[i] += "<color=purple>" + sender + ": " + "</color>" + message + "\n";
			}
			UpdateChatUI();
		}

		void IChatClientListener.OnStatusUpdate(string user, int status, bool gotMessage, object message)
		{
		}

		void IChatClientListener.OnSubscribed(string[] channels, bool[] results)
		{
			foreach (var t in channels)
			{
				_chats[0] += "<color=green>" + "Connected to channel: " + t + "</color>" + "\n";
			}
			UpdateChatUI();
		}

		void IChatClientListener.OnUnsubscribed(string[] channels)
		{
		}

		void IChatClientListener.OnUserSubscribed(string channel, string user)
		{
		}

		void IChatClientListener.OnUserUnsubscribed(string channel, string user)
		{
		}
	}
}
