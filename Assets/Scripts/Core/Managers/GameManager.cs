using System;
using Photon.Pun;
using Photon.Realtime;
using ProjectNet.ScriptableObjects.Game;
using UnityEngine;

namespace ProjectNet.Core.Managers
{
	public class GameManager : MonoBehaviourPunCallbacks
	{
		public GameSettings gameSettings;
		public int keys;
		public Transform spawnPoint;
		public GameObject mainDoor;
		
		public static GameManager Instance { get; private set; }

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else
			{
				Destroy(this);
			}
			DontDestroyOnLoad(this);
		}

		public override void OnPlayerEnteredRoom(Player newPlayer)
		{
			if (!PhotonNetwork.IsMasterClient) return;
			if (PhotonNetwork.CurrentRoom.PlayerCount - 1 == gameSettings.maxPlayers)
				photonView.RPC("PlayGame", RpcTarget.All);
		}

		public void AddKey()
		{
			if (PhotonNetwork.IsMasterClient)
				keys++;
		}

		public void UseKey()
		{
			if (PhotonNetwork.IsMasterClient)
				keys--;
		}
		
		[PunRPC]
		public void PlayGame()
		{
			Destroy(mainDoor);
		}
	}
}
