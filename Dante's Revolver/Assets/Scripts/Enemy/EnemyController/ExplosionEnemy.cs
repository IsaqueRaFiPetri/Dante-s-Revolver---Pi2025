using Photon.Pun;
using UnityEngine;

public class ExplosionEnemy : EnemyController
{
    [SerializeField] GameObject _prefab;
    private void FixedUpdate()
    {
        Walk();
        if(GetDistance() <= 1 && GetDistance() != 0)
        {
            print("dead");
            PhotonNetwork.Instantiate(_prefab.name, transform.position, Quaternion.identity);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
