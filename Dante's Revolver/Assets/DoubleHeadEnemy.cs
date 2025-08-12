using NUnit.Framework;
using Photon.Pun;
using UnityEngine;

public class DoubleHeadEnemy : MonoBehaviourPunCallbacks, IKillable, ILifeable
{
    public GameObject GetGameObject()
    {
        throw new System.NotImplementedException();
    }

    public int GetViewID(int _id)
    {
        return _id;
    }

    public void OnHit()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int damage)
    {
        throw new System.NotImplementedException();
    }
}