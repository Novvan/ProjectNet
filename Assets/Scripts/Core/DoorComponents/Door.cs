using Photon.Pun;
using UnityEngine;

namespace ProjectNet.Core.DoorComponents
{
	public class Door : MonoBehaviourPun
	{
		public Color openColor;
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
			foreach (var component in GetComponents<Collider2D>())
			{
				component.enabled = false;
			}
			_doorOpened = true;
		}
	}
}
