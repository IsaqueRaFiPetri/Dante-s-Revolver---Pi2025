using Photon.Pun;
using UnityEngine;

public class DoubleHeadEnemy : MonoBehaviourPunCallbacks, IKillable, ILifeable
{
    [SerializeField] bool _isHited;
    [SerializeField] int id = 0;
    [SerializeField] int lastId;
    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
    public void TakeDamage(int damage)
    {
        id = damage;
        if (!_isHited)
        {
            lastId = id;
            _isHited = true;
        }
        else
        {
            if(id != lastId)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                print("enemy");
            }
        }
    }
}