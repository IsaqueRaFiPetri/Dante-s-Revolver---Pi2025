using Photon.Pun;
using UnityEngine;

public interface IDoubleableHeart
{
    public void TakeDamage(PhotonView playerId);
}
public class DoubleHeadEnemy : MonoBehaviour, IDoubleableHeart
{
    [SerializeField] PhotonView lastPlayerId;

    [PunRPC]public void TakeDamage(PhotonView playerId)
    {
        if(lastPlayerId == null)
        {
            lastPlayerId = playerId;
            return;
        }
        if(playerId == lastPlayerId)
        {
            print("alreadyShoot");
            return;
        }
        else
        {
            PhotonNetwork.Destroy(gameObject);
            //Destroy(gameObject);
        }
    }
}