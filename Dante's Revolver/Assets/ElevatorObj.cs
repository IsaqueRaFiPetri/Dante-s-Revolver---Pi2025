using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorObj : MonoBehaviour
{
    [SerializeField] bool isInitialElevator;
    [SerializeField] List<Rigidbody> _objInsideElevator;
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
            _objInsideElevator.Add(_pv.gameObject.GetComponent<Rigidbody>());
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PhotonView _pv))
        {
            _objInsideElevator.Remove(_pv.gameObject.GetComponent<Rigidbody>());
        }
    }
    public List<Rigidbody> GetPlayers()
    {
        return _objInsideElevator;
    }
}
