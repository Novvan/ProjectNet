using Photon.Pun;
using ProjectNet.Core.Interfaces;
using UnityEngine;

namespace ProjectNet.Core.BulletComponents
{
	public class Bullet : MonoBehaviourPun, IMove
	{
		public float speed;
		private Vector2 _direction;
		private Rigidbody2D _rb;

		private void Awake()
		{
			if (!PhotonNetwork.IsMasterClient) Destroy(this);
			_rb = GetComponent<Rigidbody2D>();
		}

		private void Start()
		{
			if(_direction.x <= 0) gameObject.transform.localScale = new Vector3(-1, 1, 1);
			
			Move(_direction);
		}
		
		public void SetDirection(Vector2 dir)
		{
			_direction = dir;
		}

		public void Move(Vector2 dir)
		{
			_rb.velocity = dir * speed;
		}
	}
}
