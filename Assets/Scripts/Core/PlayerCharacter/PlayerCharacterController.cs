using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectNet.Core.PlayerCharacter
{
	[RequireComponent(typeof(PlayerCharacter), typeof(PlayerCharacterView))]
	public class PlayerCharacterController : MonoBehaviourPun
	{
		#region Variables

		private PlayerCharacter _playerCharacter;
		private PlayerCharacterView _playerCharacterView;
		
		private PlayerCharacterInputActions _playerInputActions;
		private InputAction _move;
		private Vector2 _moveDirection;

		#endregion

		private void Awake()
		{
			if (!photonView.IsMine) Destroy(this);
			_playerCharacter = gameObject.GetComponent<PlayerCharacter>();
			_playerCharacterView = gameObject.GetComponent<PlayerCharacterView>();
			_playerInputActions = new PlayerCharacterInputActions();
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
			_playerCharacter.Move(_moveDirection);
		}
	}
}
