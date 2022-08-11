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

		public Action OnChangedName;

		public void SetPlayerName(string playerName)
		{
			_playerName = playerName;
			OnChangedName?.Invoke();
		}

		private void Awake()
		{
			if (!photonView.IsMine) Destroy(this);
			OnChangedName += () => nicknameText.text = _playerName;
		}

		public void SetTalking(bool isActive)
		{
			nicknameText.color = isActive ? Color.green : Color.white;
		}
	}
}
