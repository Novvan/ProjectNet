using System;
using ProjectNet.Core.Interfaces;
using UnityEngine;

namespace ProjectNet.Core.EnemyComponents
{
	public class Enemy : MonoBehaviour, IMove
	{
		public float speed;
		private Rigidbody2D _rb;

		private void Awake()
		{
			_rb = GetComponent<Rigidbody2D>();
		}


		public void Move(Vector2 dir)
		{
			// Debug.Log("Enemy move to " + dir);
			_rb.velocity = dir * speed;
		}

		public void ResetMove()
		{
			_rb.velocity = Vector2.zero;
		}
	}
}
