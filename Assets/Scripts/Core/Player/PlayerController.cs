using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectNet.Core.Player
{
	[RequireComponent(typeof(PlayerData), typeof(PlayerView))]
	public class PlayerController : MonoBehaviourPun
	{
		#region Variables

		private PlayerData _playerData;
		private PlayerView _playerView;
		
		private PlayerInputActions _playerInputActions;
		private InputAction _move;
		private Vector2 _moveDirection;

		#endregion

		private void Awake()
		{
			if (!photonView.IsMine) Destroy(this);
			_playerData = gameObject.GetComponent<PlayerData>();
			_playerView = gameObject.GetComponent<PlayerView>();
			_playerInputActions = new PlayerInputActions();
		}

		private void OnEnable()
		{
			_move = _playerInputActions.Player.Move;
			_move.Enable();
		}

		private void Update()
		{
			if (!photonView.IsMine) return;
			_moveDirection = _move.ReadValue<Vector2>();
		}

		private void FixedUpdate()
		{
			if (!photonView.IsMine) return;
			_playerData.Move(_moveDirection);
		}
	}
}
