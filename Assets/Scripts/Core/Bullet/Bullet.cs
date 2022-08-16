using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ProjectNet.Core.Managers;
using ProjectNet.Core.Interfaces;

[RequireComponent(typeof(BulletController))]
public class Bullet : MonoBehaviourPun, IMove
{
    private BulletController _bulletController;
    public Vector2 direction;

    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient) Destroy(this);
        _bulletController = GetComponent<BulletController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        ServerManager.Instance.RPC("MoveBullet", direction);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector2 dir)
    {
        
    }
}
