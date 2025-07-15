using Photon.Pun;
using UnityEngine;

public class DoubleHeadEnemy : MonoBehaviourPunCallbacks, IKillable, ILifeable
{
    [SerializeField] int lastViewId;
    public int GetViewID(int photonViewID)
    {
        return photonViewID;
    }
    [PunRPC]public void TakeDamage(int damage)
    {
        if(lastViewId == 0)
        {
            lastViewId = GetViewID(damage);
        }
        else
        {
            if(lastViewId != GetViewID(damage))
            {
                print("===================== Killed by: " + damage);
            }
            else
            {
                print("Already Damaged =====================");
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}