using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class Revive : MonoBehaviourPunCallbacks
{
    [SerializeField] bool _isPlayerIn;
    [SerializeField] bool _isSoulIn;
    PhantomMode _phantomPlayer;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PhantomMode _playerPhantom))
        {
            _phantomPlayer = _playerPhantom;
            photonView.RPC("SetBoolTrue", RpcTarget.AllBuffered, _isSoulIn);
        }
        if(other.TryGetComponent(out PlayerController _playerObj))
        {
            _isPlayerIn = true;
            photonView.RPC("SetBoolTrue", RpcTarget.AllBuffered, _isPlayerIn);
        }
        DetectPlayers();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PhantomMode _playerPhantom))
        {
            _phantomPlayer = null;
            photonView.RPC("SetBoolFalse", RpcTarget.AllBuffered, _isSoulIn);
        }
        if (other.TryGetComponent(out PlayerController _playerObj))
        {
            photonView.RPC("SetBoolFalse", RpcTarget.AllBuffered,_isPlayerIn);
        }
    }
    [PunRPC]
    public bool SetBoolTrue(bool _bool)
    {
        return _bool = true;
    }
    [PunRPC]
    public bool SetBoolFalse(bool _bool)
    {
        return _bool = false;
    }

    void DetectPlayers()
    {
        if(_isPlayerIn && _isSoulIn)
        {
            _phantomPlayer.isInReviveArea(new Vector3(transform.position.x, transform.position.y + 5, transform.position.z));
            _phantomPlayer = null;
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
