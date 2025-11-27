using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class HoleCounter : MonoBehaviour
{
    [SerializeField] int layerToFind;
    [SerializeField] Layer2Manager _layerManager;
    [SerializeField] UnityEvent OnEnemyEntered;
    
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer == layerToFind)
        {
            OnEnemyEntered.Invoke();
            _layerManager.GetComponent<PhotonView>().RPC("DetectEnemyQuantity", RpcTarget.AllBuffered);
        }
    }
}
