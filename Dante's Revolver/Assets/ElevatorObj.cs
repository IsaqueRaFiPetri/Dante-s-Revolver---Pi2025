using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorObj : MonoBehaviour
{
    [SerializeField] bool isInitialElevator;
    [SerializeField] List<GameObject> _objInsideElevator;
    private void OnBecameInvisible()
    {
        if (isInitialElevator)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out PhotonView _pv))
        {

        }
    }
    private void OnCollisionExit(Collision collision)
    {
        
    }
}
