using Photon.Pun;
using Photon.Realtime;
using ProjectNet.Core.Managers;
using UnityEngine;

namespace ProjectNet.Core.Character
{
    public class FullAuthorityController : MonoBehaviour
    {
        [SerializeField] ServerManager server;
        Player _localClient;
        private void Awake()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Destroy(this);
            }
            else
            {
                _localClient = PhotonNetwork.LocalPlayer;
            }
        }
        void Start()
        {
            server.RPC("RequestConnect", _localClient);
        }

        void Update()
        {
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");
            Vector2 dir = new Vector2(h, v);
            server.RPC("RequestMove", _localClient, dir);
        }
    }
}
