using Photon.Pun;
using UnityEngine;

public class SplitEnemy : EnemyController
{
    private void FixedUpdate()
    {
        Walk();
    }
    public void CreateDemons(GameObject _enemy)
    {
        PhotonNetwork.Instantiate(_enemy.name, transform.position, Quaternion.identity);
    }
}
