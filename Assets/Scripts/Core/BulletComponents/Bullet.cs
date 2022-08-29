using System;
using Photon.Pun;
using ProjectNet.Core.Interfaces;
using ProjectNet.Core.Managers;
using UnityEngine;

namespace ProjectNet.Core.BulletComponents
{
	public class Bullet : MonoBehaviourPun, IMove
	{
		public Vector2 direction;
		public float speed;
		private Rigidbody2D _rb;

		private void Awake()
		{
			if (!PhotonNetwork.IsMasterClient) Destroy(this);
			_rb = GetComponent<Rigidbody2D>();
		}

		private void Start()
		{
			if(direction.x <= 0) gameObject.transform.localScale = new Vector3(-1, 1, 1);
			
			Move(direction);
		}

		public void Move(Vector2 dir)
		{
			_rb.velocity = dir * speed;
		}
	}
}
