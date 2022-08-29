using System;
using Photon.Pun;
using ProjectNet.Core.Managers;
using UnityEngine;

namespace ProjectNet.Core.Pickups
{
	public class Key : PickUp
	{
		private bool _done = false;

		public override void OnPickUp(GameObject whoPickedUp)
		{
			base.OnPickUp(whoPickedUp);
			ServerManager.Instance.RPC("RequestAddKey");
			PhotonNetwork.Destroy(gameObject);
		}
	}
}
