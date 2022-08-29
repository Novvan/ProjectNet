using System;
using Photon.Pun;
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
			if (!PhotonNetwork.IsMasterClient) Destroy(this);
			
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

		private void Update()
		{
			if (PhotonNetwork.CurrentRoom.PlayerCount - 1 == gameSettings.maxPlayers && _gameState == GameState.Waiting)
				SetGameState(GameState.Play);
		}

		private void SetGameState(GameState gameState)
		{
			_gameState = gameState;
			OnGameStateChanged?.Invoke(gameState);
			Debug.Log(_gameState);
		}
	}
}
