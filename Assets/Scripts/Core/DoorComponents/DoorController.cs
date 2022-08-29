using System;
using Photon.Pun;
using ProjectNet.Core.Managers;
using UnityEngine;

namespace ProjectNet.Core.DoorComponents
{
	public class DoorController : MonoBehaviourPun
	{
		private Door _door;

		private void Awake()
		{
			if (!PhotonNetwork.IsMasterClient) Destroy(this);
		}

		private void Start()
		{
			_door = GetComponent<Door>();
		}

		private void OnTriggerEnter2D(Collider2D col)
		{
			if (_door.IsOpened) return;
			if (col.gameObject.CompareTag("Player"))
			{
				ServerManager.Instance.RequestRPC("RequestOpenDoor", this.gameObject);
			}
		}
	}
}
