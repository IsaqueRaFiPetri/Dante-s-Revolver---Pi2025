using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class DoubleHeadEnemy : MonoBehaviourPunCallbacks, IKillable, ILifeable
{
    [SerializeField] List<int> alreadyShoot;
    int id = 0;
    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
    public void TakeDamage(int damage)
    {
        id = damage;
        if(alreadyShoot.Count > 0)
        {
            for (int i = 0; i < alreadyShoot.Count; i++)
            {
                if (id != alreadyShoot[i - 1])
                {
                    alreadyShoot.Add(id);
                    if(alreadyShoot.Count >= 2)
                    {
                        PhotonNetwork.Destroy(gameObject);
                    }
                }
                else
                {
                    id = 0;
                }
            }
        }
        else
        {
            alreadyShoot.Add(id);
        }
    }
}