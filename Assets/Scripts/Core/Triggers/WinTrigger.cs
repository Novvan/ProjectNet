using System;
using Photon.Pun;
using Photon.Realtime;
using ProjectNet.Core.Character;
using ProjectNet.Core.Managers;
using UnityEngine;

namespace ProjectNet.Core.Triggers
{
	public class WinTrigger : MonoBehaviourPun
	{
		public GameObject winCanvas;
		private ServerManager _serverManager;

		private void Start()
		{
			_serverManager = ServerManager.Instance;
		}

		private void OnTriggerEnter2D(Collider2D coll)
		{
			if (!PhotonNetwork.IsMasterClient) return;

			var character = coll.GetComponent<PlayerCharacter>();
			if (character == null) return;

			var playerClient = _serverManager.GetPlayer(character);
			if (playerClient != null)
			{
				photonView.RPC("Win", RpcTarget.OthersBuffered);
			}
		}

		[PunRPC]
		public void Win()
		{
			if (PhotonNetwork.IsMasterClient) return;
			winCanvas.SetActive(true);
		}
	}
}
