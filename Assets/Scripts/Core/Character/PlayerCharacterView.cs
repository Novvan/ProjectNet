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

		private void Awake()
		{
			if (!photonView.IsMine) Destroy(this);
			_animator = GetComponent<Animator>();
			_recorder = PhotonVoiceNetwork.Instance.PrimaryRecorder;
		}

		private void Update()
		{
			// if (_recorder == null) return; 
			// SetTalking(_recorder.TransmitEnabled);
		}
		public void SetMirror(bool b) 
		{
			_animator.SetBool("Mirror", b);
		}
		public void SetWalk(bool b) 
		{
			_animator.SetBool("Walking", b);
		}

		public void SetPlayerNickname(string nickname)
		{
			_playerName = nickname;
			nicknameText.text = _playerName;
			photonView.RPC("UpdateNickname", RpcTarget.OthersBuffered, _playerName);
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
