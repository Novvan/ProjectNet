using System;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectNet.Core.Character
{
	public class PlayerCharacterView : MonoBehaviourPun
	{
		[SerializeField] private Animator _animator;
		
		private string _playerName = "player";
		public TMP_Text nicknameText;
		private Recorder _recorder;
		
		private static readonly int IsWalking = Animator.StringToHash("isWalking");
		private static readonly int IsDead = Animator.StringToHash("isDead");

		private void Awake()
		{
			if (!photonView.IsMine) Destroy(this);
			// _animator = GetComponent<Animator>();
			_recorder = PhotonVoiceNetwork.Instance.PrimaryRecorder;
		}

		private void Update()
		{
			// if (_recorder == null) return; 
			// SetTalking(_recorder.TransmitEnabled);
		}

		public void SetWalk(bool b) 
		{
			_animator.SetBool(IsWalking, b);
		}
		public void SetDead(bool b) 
		{
			_animator.SetBool(IsDead, b);
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
	}
}
