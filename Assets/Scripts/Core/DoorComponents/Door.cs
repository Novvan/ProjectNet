using Photon.Pun;
using UnityEngine;

namespace ProjectNet.Core.DoorComponents
{
	public class Door : MonoBehaviourPun
	{
		public Color openColor;
		public Collider2D doorCollider;
		private bool _doorOpened = false;
		
		public bool IsOpened => _doorOpened;
		
		public void OpenDoor()
		{
			photonView.RPC("ROpenDoor", RpcTarget.All);
		}

		[PunRPC]
		public void ROpenDoor()
		{
			GetComponent<SpriteRenderer>().color = openColor;
			doorCollider.enabled = false;
			_doorOpened = true;
		}
	}
}
