using System;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using ProjectNet.ScriptableObjects.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ProjectNet.Core.Managers
{
	public class MenuManager : MonoBehaviourPunCallbacks
	{
		#region Variables

		[Header("References")]
		[SerializeField] private GameSettings gameSettings;

		[Header("UI")]
		[SerializeField] private Button loginButton;
		[SerializeField] private Button createServerButton;
		[SerializeField] private TMP_InputField screenNameInputField, roomNameInputField;

		#endregion

		private void Start()
		{
			loginButton.interactable = false;
			createServerButton.interactable = false;
			PhotonNetwork.ConnectUsingSettings();

			loginButton.onClick.AddListener(OnLoginButtonClick);
			createServerButton.onClick.AddListener(CreateServer);
		}

		public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();
		public override void OnJoinedLobby() {
			loginButton.interactable = true;
			createServerButton.interactable = true;
		}

		private void OnLoginButtonClick()
		{
			//trim input fields
			screenNameInputField.text = screenNameInputField.text.Trim();
			roomNameInputField.text = roomNameInputField.text.Trim();

			//validate input fields
			if (screenNameInputField.text.Length < 3)
			{
				screenNameInputField.text = "";
				screenNameInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Screen name must be at least 3 characters long";
				return;
			}
			if (roomNameInputField.text.Length < 3)
			{
				roomNameInputField.text = "";
				roomNameInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Room name must be at least 3 characters long";
				return;
			}

			PhotonNetwork.NickName = screenNameInputField.text;

			var maxPlayer = gameSettings.maxPlayers + 1;
			var roomOptions = new RoomOptions{
				MaxPlayers = (byte) maxPlayer,
				IsVisible = true,
				IsOpen = true
			};

			PhotonNetwork.JoinOrCreateRoom(roomNameInputField.text, roomOptions, TypedLobby.Default);
			loginButton.interactable = false;
			createServerButton.interactable = false;
		}

		private void CreateServer()
		{
			screenNameInputField.text = "Server";
			roomNameInputField.text = Random.Range(100, 300).ToString();
			// FindObjectOfType<PhotonVoiceNetwork>().PrimaryRecorder = null;
			OnLoginButtonClick();
		}

		public override void OnJoinedRoom()
		{
			PhotonNetwork.AutomaticallySyncScene = true;
			if (PhotonNetwork.IsMasterClient)
			{
				PhotonNetwork.LoadLevel("Game");
			}
		}

		public override void OnCreateRoomFailed(short returnCode, string message)
		{
			Debug.LogError($"Failed to create room. Error code: {returnCode} Message: {message}");
			loginButton.interactable = true;
			createServerButton.interactable = true;
		}

		public override void OnJoinRoomFailed(short returnCode, string message)
		{
			Debug.LogError($"Failed to join room. Error code: {returnCode} Message: {message}");
			loginButton.interactable = true;
			createServerButton.interactable = true;
		}
	}
}
