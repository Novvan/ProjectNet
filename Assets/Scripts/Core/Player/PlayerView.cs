using Photon.Pun;
using TMPro;
using UnityEngine;

namespace ProjectNet.Core.Player
{
	public class PlayerView : MonoBehaviourPun
	{
		[SerializeField] private TMP_Text nicknameText;

		private void Awake()
		{
			if (!photonView.IsMine) Destroy(this);
			nicknameText.text = photonView.Owner.NickName;
		}
	}
}
