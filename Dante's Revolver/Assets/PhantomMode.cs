using Photon.Pun;
using UnityEngine;

public class PhantomMode : MonoBehaviour
{
    [SerializeField] GameObject playerBody;
    [SerializeField] GameObject _grave;
    public void isInReviveArea(Vector3 _pos)
    {
        print("revive");
        PhotonNetwork.Instantiate(playerBody.name, _pos, Quaternion.identity).GetComponentInChildren<Camera>().enabled = true;
        PhotonNetwork.Destroy(gameObject);
        GameOver.instance.RemoveFromDeathList(_grave);
    }
}
