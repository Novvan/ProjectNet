using Photon.Pun;
using UnityEngine;

namespace ProjectNet.Core.Player
{
	public class PlayerData : MonoBehaviourPun
	{
		public float speed = 2;

		private void Awake()
		{
			if (!photonView.IsMine) Destroy(this);
		}
	}
}
