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
        alreadyShoot.Add(id);
    }
}