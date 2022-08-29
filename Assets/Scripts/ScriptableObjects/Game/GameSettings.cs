using UnityEngine;

namespace ProjectNet.ScriptableObjects.Game
{
	[CreateAssetMenu(fileName = "GameSettings", menuName = "ProjectNet/ScriptableObjects/Game/GameSettings")]
	public class GameSettings : ScriptableObject
	{
		public byte maxPlayers = 2;
		public GameObject playerPrefab, bulletPrefab;
	}
}
