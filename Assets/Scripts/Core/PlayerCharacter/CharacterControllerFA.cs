using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CharacterControllerFA : MonoBehaviour
{
    [SerializeField] ServerManager _server;
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
        _server.RPC("RequestConnect", _localClient);
    }

    void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(h, v);
        _server.RPC("RequestMove", _localClient, dir);
    }
}
