using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectNet.Core.Character
{
	public class PlayerCharacterView : MonoBehaviourPun
	{
		private string _playerName = "player";
		public TMP_Text nicknameText;

		private void Awake()
		{
			if (!photonView.IsMine) Destroy(this);
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
	}
}
