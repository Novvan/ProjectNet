using System;
using Photon.Pun;
using ProjectNet.Core.Interfaces;
using UnityEngine;

namespace ProjectNet.Core.BulletComponents
{
	public class Bullet : MonoBehaviourPun, IMove
	{
		public float speed = 15, lifeSpan = 3;

		private Vector2 _direction;
		private Rigidbody2D _rb;
		private Animator _animator;
		private float _counter;

		private void Awake()
		{
			if (!PhotonNetwork.IsMasterClient) Destroy(this);
			_rb = GetComponent<Rigidbody2D>();
			_animator = GetComponent<Animator>();
		}

		private void Start()
		{
			Move(_direction);
		}

		private void Update()
		{
			_counter += Time.deltaTime;
			if (_counter >= lifeSpan)
			{
				PhotonNetwork.Destroy(gameObject);
			}
		}

		public void SetDirection(Vector2 dir)
		{
			_direction = dir;
			// _animator.SetBool("isMirror", dir.x < 0);
		}

		public void Move(Vector2 dir)
		{
			_rb.velocity = dir * speed;
		}
	}
}
