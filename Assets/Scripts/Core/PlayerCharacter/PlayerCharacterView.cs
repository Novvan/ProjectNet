using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectNet.Core.PlayerCharacter
{
	public class PlayerCharacterView : MonoBehaviourPun
	{
		[SerializeField] private TMP_Text nicknameText;
		[SerializeField] private Image micImage;

		private void Awake()
		{
			if (!photonView.IsMine) Destroy(this);
			nicknameText.text = photonView.Owner.NickName;
		}

		public void SetMicImage(bool isActive)
		{
			micImage.enabled = isActive;
		}
	}
}
