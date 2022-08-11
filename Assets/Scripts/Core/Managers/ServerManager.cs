using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ServerManager : MonoBehaviourPunCallbacks
{
    [SerializeField] string _namePrefab;
    Dictionary<Player, Character> _characters;
    Player _server;
    private void Awake()
    {
        _server = PhotonNetwork.MasterClient;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    [PunRPC]
    void RequestConnect(Player client) 
    {
        CreatePlayer(client);
    }
    void CreatePlayer(Player client) 
    {
        var obj = PhotonNetwork.Instantiate(_namePrefab, Vector3.zero, Quaternion.identity);
        var character = obj.GetComponent<Character>();
        if(character != null) 
        {
            _characters[client] = character;
        }
    }
    [PunRPC]
    public void RPC(string name, params object[] p) 
    {
        photonView.RPC(name, _server, p);
    }
    [PunRPC]
    void RequestMove(Player client, Vector3 dir) 
    {
        if (_characters.ContainsKey(client)) 
        {
            _characters[client].Move(dir);
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer) 
    {
        if (IsServer)
        {
            if (_characters.ContainsKey(otherPlayer))
            {
                PhotonNetwork.Destroy(_characters[otherPlayer].gameObject);
            }
        }
    }
    public Player GetServer => _server;
    public bool IsServer => PhotonNetwork.IsMasterClient;
}
