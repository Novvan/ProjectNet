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
				Destroy(gameObject);
			}

			DontDestroyOnLoad(gameObject);
		}

		private void Update()
		{
			if (!PhotonNetwork.IsMasterClient) return;
			if (PhotonNetwork.CurrentRoom.PlayerCount == gameSettings.maxPlayers && _gameState == GameState.Waiting)
				SetGameState(GameState.Play);
		}

		private void SetGameState(GameState gameState)
		{
			_gameState = gameState;
			OnGameStateChanged?.Invoke(gameState);
		}
	}
}
