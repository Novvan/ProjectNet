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
		private Animator _animator;
		private static readonly int IsMirror = Animator.StringToHash("isMirror");

		public Vector2 Direction => _direction;

		private void Awake()
		{
			_animator = GetComponent<Animator>();
		}

		public void SetDirection(Vector2 dir)
		{
			_direction = dir;
			photonView.RPC("UpdateBulletAnimator", RpcTarget.All);
		}

		[PunRPC]
		public void UpdateBulletAnimator()
		{
			_animator.SetBool(IsMirror, _direction.x <= 0);
		}
	}
}
