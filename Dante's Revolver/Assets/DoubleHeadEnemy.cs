using Photon.Pun;
using UnityEngine;

public class DoubleHeadEnemy : MonoBehaviourPunCallbacks, ILifeable
{
    [SerializeField] int id;
    [SerializeField] int lastId;
    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
    [PunRPC]
    public void TakeDamage(int _pv)
    {
        if (id == 0)
        {
            id = _pv;
            lastId = id;
        }
        else
        {
            id = _pv;
            if(lastId != id)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                lastId = _pv;
            }
        }
    }
}