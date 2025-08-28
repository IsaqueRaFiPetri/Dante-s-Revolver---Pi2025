using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class Revive : MonoBehaviour
{
    [SerializeField] List<PhotonView> _players;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PhantomMode _playerPhantom))
        {
            _players.Add(_playerPhantom.gameObject.GetPhotonView());
        }
        if(other.TryGetComponent(out PlayerController _playerObj))
        {
            _players.Add(_playerObj.photonView);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerCam _playerCam))
        {
            _players.Remove(_playerCam.photonView);
        }
        if (other.TryGetComponent(out PlayerController _playerObj))
        {
            _players.Remove(_playerObj.photonView);
        }
    }
}
