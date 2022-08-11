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
		private Vector2 _moveDirection;

		#endregion

		private void Awake()
		{
			if (PhotonNetwork.IsMasterClient) Destroy(this);
			
			_localClient = PhotonNetwork.LocalPlayer;
			_playerInputActions = new PlayerCharacterInputActions();
		}

		private void OnEnable()
		{
			_move = _playerInputActions.Player.Move;
			_move.Enable();
		}

		private void Start()
		{
			ServerManager.Instance.RPC("RequestConnect", _localClient);
		}
		

		private void Update()
		{
			_moveDirection = _move.ReadValue<Vector2>();
		}

		private void FixedUpdate()
		{
			ServerManager.Instance.RPC("RequestMove", _localClient, _moveDirection);
		}
	}
}
