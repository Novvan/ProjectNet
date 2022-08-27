using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectNet.Core.Character
{
	public enum PlayerAnimations
	{
		Idle,
		Walk,
	}

	public class PlayerCharacterView : MonoBehaviourPun
	{
		private string _playerName = "player";
		public TMP_Text nicknameText;
		private Animator _animator;

		private void Awake()
		{
			if (!photonView.IsMine) Destroy(this);
			_animator = GetComponent<Animator>();
		}

		public void SetPlayerNickname(string nickname)
		{
			_playerName = nickname;
			nicknameText.text = _playerName;
			photonView.RPC("UpdateNickname", RpcTarget.OthersBuffered, _playerName);
		}

		[PunRPC]
		public void UpdateNickname(string playerName)
		{
			nicknameText.text = playerName;
		}


		public void SetTalking(bool isActive)
		{
			nicknameText.color = isActive ? Color.green : Color.white;
		}

		[PunRPC]
		public void SetAnim(PlayerAnimations anim)
		{
			//TODO: ANIMACIONES DE MOVIMIENTO
			// switch (anim)
			// {
			// 	case PlayerAnimations.Idle:
			// 		_animator.Play("Player_idle");
			// 		break;
			// 	case PlayerAnimations.Walk:
			// 		_animator.Play("Player_walk");
			// 		break;
			// 	default:
			// 		throw new ArgumentOutOfRangeException(nameof(anim), anim, null);
			// }
		}
	}
}
