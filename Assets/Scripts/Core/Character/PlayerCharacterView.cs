using System;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
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
		private Recorder _recorder;
		public SpriteRenderer _spriteRenderer;

		private void Awake()
		{
			if (!photonView.IsMine) Destroy(this);
			_animator = GetComponent<Animator>();
			_recorder = PhotonVoiceNetwork.Instance.PrimaryRecorder;
			_spriteRenderer = GetComponent<SpriteRenderer>();
		}

		private void Update()
		{
			// if (_recorder == null) return; 
			// SetTalking(_recorder.TransmitEnabled);
		}

		public void SetPlayerNickname(string nickname)
		{
			_playerName = nickname;
			nicknameText.text = _playerName;
			photonView.RPC("UpdateNickname", RpcTarget.OthersBuffered, _playerName);
		}
		public void SetSpriteOrientation(float b) 
		{
			bool bol;
			if(b < 0)bol = true;
			else bol = false;
			photonView.RPC("UpdateFlip", RpcTarget.OthersBuffered, bol);
		}

		public void SetTalking(bool isActive)
		{
			Debug.Log("PlayerView: SetTalking " + isActive);
			photonView.RPC("UpdateTalking", RpcTarget.OthersBuffered, isActive);
		}

		[PunRPC]
		public void UpdateTalking(bool isActive)
		{
			nicknameText.color = isActive ? Color.green : Color.white;
		}

		[PunRPC]
		public void UpdateNickname(string playerName)
		{
			nicknameText.text = playerName;
		}
		[PunRPC]
		public void UpdateFlip(bool b) 
		{
			_spriteRenderer.flipX = b;
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
