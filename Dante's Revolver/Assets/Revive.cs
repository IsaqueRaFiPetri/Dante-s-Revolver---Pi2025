using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class Revive : MonoBehaviourPunCallbacks
{
    [SerializeField] bool _isPlayerIn;
    [SerializeField] bool _isSoulIn;
    PhantomMode _phantomPlayer;

    [SerializeField] AudioSource audioSource;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PhantomMode _playerPhantom))
        {
            _phantomPlayer = _playerPhantom;
            photonView.RPC("SetSoul", RpcTarget.AllBuffered, true);
            print("AAAAAAAAAAAAAAAAAAAAAAAAAA");
        }
        if(other.TryGetComponent(out PlayerController _playerObj))
        {
            _isPlayerIn = true;
            photonView.RPC("SetHuman", RpcTarget.AllBuffered, true);
        }
        DetectPlayers();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PhantomMode _playerPhantom))
        {
            _phantomPlayer = null;
            photonView.RPC("SetSoul", RpcTarget.AllBuffered, false);
        }
        if (other.TryGetComponent(out PlayerController _playerObj))
        {
            photonView.RPC("SetHuman", RpcTarget.AllBuffered, false);
        }
    }
    [PunRPC]
    public bool SetSoul(bool _soul)
    {
        return _isSoulIn = _soul;
    }
    [PunRPC]
    public bool SetHuman(bool _human)
    {
        return _isPlayerIn = _human;
    }

    void DetectPlayers()
    {
        if(_isPlayerIn && _isSoulIn)
        {
            _phantomPlayer.isInReviveArea(new Vector3(transform.position.x, transform.position.y + 5, transform.position.z));
            _phantomPlayer = null;
            PhotonNetwork.Destroy(gameObject);
            audioSource.Play();
        }
    }
}
