using System;
using Photon.Pun;
using ProjectNet.Core.Interfaces;
using UnityEngine;

namespace ProjectNet.Core.BulletComponents
{
	public class Bullet : MonoBehaviourPun
	{
		public float speed = 15, lifeSpan = 3;

		private Vector2 _direction;
		public Vector2 Direction => _direction;
		
		public void SetDirection(Vector2 dir)
		{
			_direction = dir;
		}
	}
}
