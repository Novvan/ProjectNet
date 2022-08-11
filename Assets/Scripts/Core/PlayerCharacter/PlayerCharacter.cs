using Photon.Pun;
using UnityEngine;

namespace ProjectNet.Core.PlayerCharacter
{
	public class PlayerCharacter : MonoBehaviourPun
	{
		public float speed = 2;
		private Rigidbody2D _rb;

		private void Awake()
		{
			if (!photonView.IsMine) Destroy(this);
			_rb = GetComponent<Rigidbody2D>();
		}

		public void Move(Vector2 direction)
		{
			_rb.velocity = direction * speed;
		}
	}
}
