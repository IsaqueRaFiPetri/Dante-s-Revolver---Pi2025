using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class Revive : MonoBehaviour
{
    [SerializeField] List<PhotonView> _players;
    int playerQuantity;
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
        for (int i = 0; i < _players.Count; i++)
        {
            if (_players[i].GetComponent<PhotonView>())
            {
                playerQuantity++;

                if(playerQuantity >= 2)
                {
                    _playerPhantom.isInReviveArea(new Vector3(transform.position.x, transform.position.y + 5, transform.position.z));
                    print("2 people");
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PhantomMode _playerPhantom))
        {
            _players.Remove(_playerPhantom.gameObject.GetPhotonView());
        }
        if (other.TryGetComponent(out PlayerController _playerObj))
        {
            _players.Remove(_playerObj.photonView);
        }
    }
}
