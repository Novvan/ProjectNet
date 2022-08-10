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
		private Rigidbody2D _rb;

		#endregion

		private void Awake()
		{
			if (!photonView.IsMine) Destroy(this);
			_playerData = gameObject.GetComponent<PlayerData>();
			_playerView = gameObject.GetComponent<PlayerView>();
			_playerInputActions = new PlayerInputActions();
			_rb = gameObject.GetComponent<Rigidbody2D>();
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
			_rb.velocity = _moveDirection * _playerData.speed;
		}
	}
}
