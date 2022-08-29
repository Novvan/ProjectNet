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
		private bool _isDead = false;

		#endregion

		private void Awake()
		{
			if (PhotonNetwork.IsMasterClient) Destroy(this);

			var chatManager = FindObjectOfType<ChatManager>();
			if (chatManager)
			{
				chatManager.OnSelect += () => _isWriting = true;
				chatManager.OnDeselect += () => _isWriting = false;
			}

			_localClient = PhotonNetwork.LocalPlayer;
			_playerInputActions = new PlayerCharacterInputActions();

			_recorder = PhotonVoiceNetwork.Instance.PrimaryRecorder;
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
			ServerManager.Instance.RequestRPC("RequestConnect", _localClient);
		}


		private void Update()
		{

			if (_isWriting) return;
			if (_recorder == null) return;
			_recorder.TransmitEnabled = _talk.IsPressed();

			if (GameManager.Instance.GameState != GameState.Play)
			{
				Debug.Log("GameState != Play");
				return;
			}
			_isDead = ServerManager.Instance.GetPlayerCharacter(_localClient).isDead;
			if (_isDead)
			{
				Debug.Log("Player is dead");
				return;
			}
			
			_moveDirection = _move.ReadValue<Vector2>();
			if (_fire.triggered)
			{
				var x = _moveDirection.x;
				ServerManager.Instance.RequestRPC("RequestShoot", _localClient, new Vector2(x, 0));
			}
		}

		private void FixedUpdate()
		{
			if (GameManager.Instance.GameState != GameState.Play)
			{
				Debug.Log("LATE GameState != Play");
				return;
			}
			if (_isDead)
			{
				Debug.Log("LATE Player is dead");
				return;
			}
			ServerManager.Instance.RequestRPC("RequestMove", _localClient, _moveDirection);
		}
	}
}
