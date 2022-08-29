using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using ProjectNet.Core.CameraScripts;
using ProjectNet.Core.Character;
using ProjectNet.Core.DoorComponents;
using ProjectNet.Core.EnemyComponents;
using UnityEngine;

namespace ProjectNet.Core.Managers
{
	public class ServerManager : MonoBehaviourPunCallbacks
	{
		[SerializeField] private CameraController cameraController;

		private Dictionary<Player, PlayerCharacter> _playerCharacters = new Dictionary<Player, PlayerCharacter>();
		private Dictionary<PlayerCharacter, Player> _characterPlayers = new Dictionary<PlayerCharacter, Player>();
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
			var character = _playerCharacters[client];
			var id = character.photonView.ViewID;
			photonView.RPC("SetCamera", client, id, cameraController.Offset);
			character.GetComponent<PlayerCharacterView>().SetPlayerNickname(client.NickName);
		}

		private void CreatePlayer(Player client)
		{
			var obj = PhotonNetwork.Instantiate(GameManager.Instance.gameSettings.playerPrefab.name, Vector3.zero, Quaternion.identity);
			var character = obj.GetComponent<PlayerCharacter>();
			if (character == null) return;
			_playerCharacters[client] = character;
			_characterPlayers[character] = client;
		}

		public Player GetPlayer(PlayerCharacter playerCharacter)
		{
			return _characterPlayers[playerCharacter];
		}
		public PlayerCharacter GetPlayerCharacter(Player player)
		{
			return _playerCharacters[player];
		}

		[PunRPC]
		public void RequestRPC(string rpcName, params object[] p)
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

		[PunRPC]
		public void RequestDamage(Player client)
		{
			if(_playerCharacters.ContainsKey(client))
			{
				_playerCharacters[client].TakeDamage();
			}
			
			// foreach (var client in _playerCharacters)
			// {
			// 	if (client.Value.photonView.ViewID == viewId)
			// 	{
			// 		client.Value.TakeDamage();
			// 	}
			// }
		}


		[PunRPC]
		public void RequestDeath(Player client)
		{
			if (_playerCharacters.ContainsKey(client))
			{
				_playerCharacters[client].isDead = true;
			}
			
			// foreach (var client in _playerCharacters)
			// {
			// 	if (client.Value.photonView.ViewID == viewId)
			// 	{
			// 		client.Value.TakeDamage();
			// 	}
			// }
		}

		[PunRPC]
		public void RequestEnemyMove(GameObject enemy, Vector2 dir)
		{
			enemy.GetComponent<Enemy>().Move(dir);
		}

		[PunRPC]
		public void RequestResetEnemyMove(GameObject enemy)
		{
			enemy.GetComponent<Enemy>().ResetMove();
		}

		[PunRPC]
		public void RequestShoot(Player client, Vector2 dir)
		{
			if (_playerCharacters.ContainsKey(client))
			{
				_playerCharacters[client].Shoot(dir);
			}
		}

		[PunRPC]
		public void RequestAddKey()
		{
			GameManager.Instance.keys++;
		}

		[PunRPC]
		public void RequestOpenDoor(GameObject door)
		{
			if (GameManager.Instance.keys <= 0) return;
			GameManager.Instance.keys--;
			var doorComponent = door.GetComponent<Door>();
			if (doorComponent != null)
			{
				doorComponent.OpenDoor();
			}
		}

		[PunRPC]
		public void RequestOpenAllDoors()
		{
			foreach (var door in GameObject.FindGameObjectsWithTag("Door"))
			{
				var doorComponent = door.GetComponent<Door>();
				if (doorComponent != null)
				{
					doorComponent.OpenDoor();
				}
			}
		}

		[PunRPC]
		public void SetCamera(int id, Vector3 offset)
		{
			var view = PhotonView.Find(id);
			if (view == null) return;
			var characterTransform = view.gameObject.transform;
			cameraController.SetTarget(characterTransform, offset);
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
