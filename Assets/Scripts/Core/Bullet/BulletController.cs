using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BulletController : MonoBehaviourPun
{

    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient) Destroy(this);
    }
}
