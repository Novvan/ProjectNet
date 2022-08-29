using Photon.Pun;
using ProjectNet.Core.Interfaces;
using ProjectNet.Core.BulletComponents;
using ProjectNet.Core.Managers;
using UnityEngine;

namespace ProjectNet.Core.Character
{
	[RequireComponent(typeof(PlayerCharacterView))]
	public class PlayerCharacter : MonoBehaviourPun, IMove
	{
		#region Variables

		public float speed = 2;
		public Transform bulletSpawnPoint;

		private Rigidbody2D _rb;
		private PlayerCharacterView _playerCharacterView;
		private float _lastLookDirection = 1;
		
		#endregion

		private void Awake()
		{
			if (!photonView.IsMine) Destroy(this);
			_rb = GetComponent<Rigidbody2D>();
			_playerCharacterView = gameObject.GetComponent<PlayerCharacterView>();
		}

		public void Move(Vector2 direction)
		{
			_playerCharacterView.SetAnim(direction.magnitude > 0.01 ? PlayerAnimations.Idle : PlayerAnimations.Walk);
			_rb.velocity = direction.normalized * speed;

			if (_rb.velocity.x != 0) _lastLookDirection = direction.normalized.x;
		}

		public void Shoot(Vector2 dir)
		{
			if (dir == Vector2.zero)
			{
				dir.x = _lastLookDirection;
			}

			var obj = PhotonNetwork.Instantiate(GameManager.Instance.gameSettings.bulletPrefab
				.name, bulletSpawnPoint.position, Quaternion.identity);
			if (obj == null)
			{
				Debug.LogError("Bullet prefab not found");
				return;
			}
			var bullet = obj.GetComponent<Bullet>();
			if (bullet == null)
			{
				Debug.LogError("Bullet component not found");
				return;
			}
			bullet.SetDirection(dir);
		}
	}
}
