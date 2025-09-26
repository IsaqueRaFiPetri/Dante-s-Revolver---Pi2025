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
        if (collision.gameObject.TryGetComponent(out PhotonView _pv))
        {
            _objInsideElevator.Add(_pv.gameObject);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PhotonView _pv))
        {
            _objInsideElevator.Remove(_pv.gameObject);
        }
    }
    public List<GameObject> GetPlayers()
    {
        return _objInsideElevator;
    }
}
