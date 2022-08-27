using System;
using Photon.Pun;
using Photon.Realtime;
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
		private InputAction _move;
		private InputAction _fire;
		private Vector2 _moveDirection;
		private bool _isWriting;

		#endregion

		private void Awake()
		{
			if (PhotonNetwork.IsMasterClient) Destroy(this);
			
			var chatManager = FindObjectOfType<ChatManager>();
			if(chatManager)
			{
				//me ahorro crear la funcion
				chatManager.OnSelect += () => _isWriting = true;
				chatManager.OnDeselect += () => _isWriting = false;
			}
			
			_localClient = PhotonNetwork.LocalPlayer;
			_playerInputActions = new PlayerCharacterInputActions();
		}

		private void OnEnable()
		{
			_move = _playerInputActions.Player.Move;
			_move.Enable();
			_fire = _playerInputActions.Player.Fire;
			_fire.Enable();
		}

		private void Start()
		{
			ServerManager.Instance.RPC("RequestConnect", _localClient);
		}
		

		private void Update()
		{
			if (_isWriting) return;
			_moveDirection = _move.ReadValue<Vector2>();
			if (_fire.triggered) Debug.Log("hola");
		}

		private void FixedUpdate()
		{
			ServerManager.Instance.RPC("RequestMove", _localClient, _moveDirection);
		}
	}
}
