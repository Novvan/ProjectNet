using System;
using Photon.Pun;
using Photon.Realtime;
using ProjectNet.ScriptableObjects.Game;
using UnityEngine;

namespace ProjectNet.Core.Managers
{
	public enum GameState
	{
		Waiting,
		Play,
		Victory,
		Defeat
	}

	public class GameManager : MonoBehaviourPunCallbacks
	{
		public GameSettings gameSettings;
		public int keys;
		public Transform spawnPoint;
		private GameState _gameState;

		public event Action<GameState> OnGameStateChanged;


		public GameState GameState => _gameState;

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

		private void Start()
		{
			SetGameState(GameState.Waiting);
		}

		public override void OnPlayerEnteredRoom(Player newPlayer)
		{
			if (!PhotonNetwork.IsMasterClient) return;
			if (PhotonNetwork.CurrentRoom.PlayerCount - 1 == gameSettings.maxPlayers && _gameState == GameState.Waiting)
				photonView.RPC("SetGameState", RpcTarget.All, GameState.Play);
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
		private void SetGameState(GameState gameState)
		{
			_gameState = gameState;
			OnGameStateChanged?.Invoke(gameState);
			Debug.Log(_gameState);
		}
	}
}
