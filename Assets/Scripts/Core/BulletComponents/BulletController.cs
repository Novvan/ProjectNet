using System;
using Photon.Pun;
using ProjectNet.Core.Interfaces;
using UnityEngine;

namespace ProjectNet.Core.BulletComponents
{
	public class BulletController : MonoBehaviourPun, IMove
	{
		private Bullet _bullet;
		private float _counter;
		private Rigidbody2D _rb;

		private void Awake()
		{
			if (!PhotonNetwork.IsMasterClient) Destroy(this);
			_bullet = GetComponent<Bullet>();
			_rb = GetComponent<Rigidbody2D>();
		}

		private void Start()
		{
			Move(_bullet.Direction);
		}


		public void Move(Vector2 dir)
		{
			_rb.velocity = dir * _bullet.speed;
			if (_rb.velocity.x < 0) gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
		}

		private void Update()
		{
			_counter += Time.deltaTime;
			if (_counter >= _bullet.lifeSpan)
			{
				PhotonNetwork.Destroy(gameObject);
			}
		}


		private void OnTriggerEnter2D(Collider2D col)
		{
			PhotonNetwork.Destroy(gameObject);
		}
	}
}
