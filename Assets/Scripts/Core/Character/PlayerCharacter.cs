using System;
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
		public Transform graphics, canvas;
		public bool isDead = false;

		private Rigidbody2D _rb;
		private PlayerCharacterView _playerCharacterView;
		private float _lastLookDirection = 1;
		private int _lives = 3;

		#endregion

		private void Awake()
		{
			if (!photonView.IsMine) Destroy(this);
			_rb = GetComponent<Rigidbody2D>();
			_playerCharacterView = gameObject.GetComponent<PlayerCharacterView>();
		}

		public void Move(Vector2 direction)
		{
			if (isDead) return;
			_rb.velocity = direction.normalized * speed;

			if (_rb.velocity != Vector2.zero)
			{
				_playerCharacterView.SetWalk(true);

				if (direction.normalized.x != 0)
					_lastLookDirection = direction.normalized.x;

				graphics.rotation = Quaternion.Euler(0, _lastLookDirection < 0 ? 180 : 0, 0);
			}
			else _playerCharacterView.SetWalk(false);
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

		private void OnTriggerEnter2D(Collider2D col)
		{
			if (col.gameObject.CompareTag("Enemy"))
			{
				ServerManager.Instance.RequestRPC("RequestDamage", ServerManager.Instance.GetPlayer(this));
			}
		}


		public void TakeDamage()
		{
			_lives--;
			if (_lives <= 0)
			{
				_playerCharacterView.SetDead(true);
				_rb.velocity = Vector2.zero;
				ServerManager.Instance.RequestRPC("RequestDeath", ServerManager.Instance.GetPlayer(this));
			}
			this.transform.position = GameManager.Instance.spawnPoint.position;
		}
	}
}
