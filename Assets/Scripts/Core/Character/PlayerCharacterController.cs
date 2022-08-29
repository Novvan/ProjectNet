using System;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using ProjectNet.Core.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectNet.Core.Character
{
	public class PlayerCharacterController : MonoBehaviourPun
	{
		#region Variables

		private Player _localClient;

		private PlayerCharacterInputActions _playerInputActions;
		private InputAction _move, _fire, _talk;
		private Vector2 _moveDirection;
		private bool _isWriting;
		private Recorder _recorder;
		// private PlayerCharacterView _view;

		#endregion

		private void Awake()
		{
			if (PhotonNetwork.IsMasterClient) Destroy(this);

			var chatManager = FindObjectOfType<ChatManager>();
			if (chatManager)
			{
				//me ahorro crear la funcion
				chatManager.OnSelect += () => _isWriting = true;
				chatManager.OnDeselect += () => _isWriting = false;
			}

			_localClient = PhotonNetwork.LocalPlayer;
			_playerInputActions = new PlayerCharacterInputActions();

			_recorder = PhotonVoiceNetwork.Instance.PrimaryRecorder;
			// _view = GetComponent<PlayerCharacterView>();
		}

		private void OnEnable()
		{
			_move = _playerInputActions.Player.Move;
			_move.Enable();
			_fire = _playerInputActions.Player.Fire;
			_fire.Enable();
			_talk = _playerInputActions.Player.Talk;
			_talk.Enable();
		}

		private void Start()
		{
			ServerManager.Instance.RPC("RequestConnect", _localClient);
		}


		private void Update()
		{
			if (_isWriting) return;
			_moveDirection = _move.ReadValue<Vector2>();
			if (_fire.triggered)
			{
				var x = _moveDirection.x;
				ServerManager.Instance.RPC("RequestShoot", _localClient, new Vector2(x, 0));
			}
			if (_recorder == null) return;
			_recorder.TransmitEnabled = _talk.IsPressed();
		}

		private void FixedUpdate()
		{
			ServerManager.Instance.RPC("RequestMove", _localClient, _moveDirection);
		}
	}
}
