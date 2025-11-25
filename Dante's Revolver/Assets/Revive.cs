using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class Revive : MonoBehaviour
{
    [SerializeField] bool _isPlayerIn;
    [SerializeField] bool _isSoulIn;
    PhantomMode _phantomPlayer;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PhantomMode _playerPhantom))
        {
            _phantomPlayer = _playerPhantom;
            _isSoulIn = true;
        }
        if(other.TryGetComponent(out PlayerController _playerObj))
        {
            _isPlayerIn = true;
        }
        DetectPlayers();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PhantomMode _playerPhantom))
        {
            _phantomPlayer = null;
            _isSoulIn = false;
        }
        if (other.TryGetComponent(out PlayerController _playerObj))
        {
            _isPlayerIn = false;
        }
    }
    void DetectPlayers()
    {
        if(_isPlayerIn && _isSoulIn)
        {
            _phantomPlayer.isInReviveArea(new Vector3(transform.position.x, transform.position.y + 5, transform.position.z));
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
