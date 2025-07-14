using Photon.Pun;
using UnityEngine;

public interface IDoubleableHeart
{
    public void TakeDamage(int playerId);
}
public class DoubleHeadEnemy : MonoBehaviour, IDoubleableHeart
{
    [SerializeField] int lastPlayerId;

    [PunRPC]public void TakeDamage(int playerId)
    {
        print("================================================" + playerId);
        if(lastPlayerId == 0)
        {
            lastPlayerId = playerId;
        }
        if(playerId != lastPlayerId)
        {
            Destroy(gameObject);
        }
    }
}