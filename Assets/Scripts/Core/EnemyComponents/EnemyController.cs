using System;
using Photon.Pun;
using ProjectNet.Core.Interfaces;
using UnityEngine;

namespace ProjectNet.Core.EnemyComponents
{
	public class EnemyController : MonoBehaviourPun
	{
		private void Awake()
		{
			if (!PhotonNetwork.IsMasterClient) Destroy(this);
		}

		private void OnTriggerEnter2D(Collider2D col)
		{
			if (col.CompareTag("TriggerRadius")) return;
			if (!col.gameObject.CompareTag("Bullet")) return;
			
			PhotonNetwork.Destroy(gameObject);
		}
	}
}
