using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using ProjectNet.Core.Character;
using UnityEngine;

namespace ProjectNet.Core.Managers
{
	public class ServerManager : MonoBehaviourPunCallbacks
	{
		private Dictionary<Player, PlayerCharacter> _playerCharacters = new Dictionary<Player, PlayerCharacter>();
		private Player _server;

		public static ServerManager Instance { get; private set; }

		public Player GetServer => _server;

		public bool IsServer => PhotonNetwork.IsMasterClient;

		private void Awake()
		{
			_server = PhotonNetwork.MasterClient;
			if (Instance == null)
			{
				Instance = this;
			}
			else
			{
				Destroy(gameObject);
			}
		}

		[PunRPC]
		public void RequestConnect(Player client)
		{
			CreatePlayer(client);
		}

		private void CreatePlayer(Player client)
		{
			var obj = PhotonNetwork.Instantiate(GameManager.Instance.gameSettings.playerPrefab.name, Vector3.zero, Quaternion.identity);
			var character = obj.GetComponent<PlayerCharacter>();
			if (character != null)
			{
				_playerCharacters[client] = character;
			}
		}

		[PunRPC]
		public void RPC(string rpcName, params object[] p)
		{
			photonView.RPC(rpcName, _server, p);
		}

		[PunRPC]
		public void RequestMove(Player client, Vector2 dir)
		{
			if (_playerCharacters.ContainsKey(client))
			{
				_playerCharacters[client].Move(dir);
			}
		}

		public override void OnPlayerLeftRoom(Player otherPlayer)
		{
			if (!IsServer) return;
			if (_playerCharacters.ContainsKey(otherPlayer))
			{
				PhotonNetwork.Destroy(_playerCharacters[otherPlayer].gameObject);
			}
		}
	}
}
