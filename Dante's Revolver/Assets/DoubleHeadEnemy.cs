using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class DoubleHeadEnemy : MonoBehaviourPunCallbacks, IKillable, ILifeable
{
    [SerializeField] int lastViewId;
    [SerializeField] UnityEvent OnTakeDamage;
    public int GetViewID(int photonViewID)
    {
        return photonViewID;
    }
    [PunRPC]public void TakeDamage(int damage)
    {
        OnTakeDamage.Invoke();
        if(lastViewId == 0)
        {
            lastViewId = GetViewID(damage);
        }
        else
        {
            if(lastViewId != GetViewID(damage))
            {

            }
            else
            {

                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}