using Photon.Pun;

namespace ProjectNet.Core.Bullet
{
    public class BulletController : MonoBehaviourPun
    {

        private void Awake()
        {
            if (!PhotonNetwork.IsMasterClient) Destroy(this);
        }
    }
}
