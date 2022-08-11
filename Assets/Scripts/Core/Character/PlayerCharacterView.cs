using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectNet.Core.Character
{
	public class PlayerCharacterView : MonoBehaviourPun
	{
		[SerializeField] private TMP_Text nicknameText;

		private void Awake()
		{
			if (!photonView.IsMine) Destroy(this);
			nicknameText.text = photonView.Owner.NickName;
		}

		public void SetTalking(bool isActive)
		{
			nicknameText.color = isActive ? Color.green : Color.white;
		}
	}
}
