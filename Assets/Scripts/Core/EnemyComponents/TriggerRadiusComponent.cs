using System;
using Photon.Pun;
using ProjectNet.Core.Managers;
using UnityEngine;

namespace ProjectNet.Core.EnemyComponents
{
	public class TriggerRadiusComponent : MonoBehaviourPun
	{
		public GameObject graphics;

		private GameObject _parent, _player;
		private Animator _animator;
		private static readonly int IsWalking = Animator.StringToHash("isWalking");

		private void Awake()
		{
			if (!PhotonNetwork.IsMasterClient) Destroy(this);
			_parent = transform.parent.gameObject;
			_animator = graphics.GetComponent<Animator>();
		}

		private void OnTriggerEnter2D(Collider2D col)
		{
			if (!col.gameObject.CompareTag("Player")) return;
			_player = col.gameObject;
		}

		private void Update()
		{
			if (GameManager.Instance.GameState != GameState.Play) return;
			
			if (_parent == null) return;
			if (_player == null) return;

			var dir = (Vector2) (_player.transform.position - _parent.transform.position);
			ServerManager.Instance.RequestRPC("RequestEnemyMove", _parent, dir);
			_animator.SetBool(IsWalking, true);

			graphics.transform.rotation = Quaternion.Euler(0, dir.x < 0 ? 180 : 0, 0);
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			_player = null;
			ServerManager.Instance.RequestRPC("RequestResetEnemyMove", _parent);
			_animator.SetBool(IsWalking, false);
		}
	}
}
