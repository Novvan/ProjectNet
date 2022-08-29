using System;
using Photon.Pun;
using ProjectNet.Core.Interfaces;
using UnityEngine;

namespace ProjectNet.Core.Pickups
{
	[RequireComponent(typeof(BoxCollider2D), typeof(PhotonView))]
	public class PickUp : MonoBehaviourPun, IPickUp
	{
		private void Awake()
		{
			if (!PhotonNetwork.IsMasterClient) Destroy(this);
		}

		private void OnTriggerEnter2D(Collider2D col)
		{
			if (col.gameObject.CompareTag("Player"))
				OnPickUp(col.gameObject);
		}

		public virtual void OnPickUp(GameObject whoPickedUp)
		{
			Debug.Log(this.name + " picked up");
		}
	}
}
