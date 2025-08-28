using Photon.Pun;
using UnityEngine;

public class PhantomMode : MonoBehaviour
{
    [SerializeField] GameObject playerBody;
    public void isInReviveArea(Vector3 _pos)
    {
        print("revive");
        PhotonNetwork.Instantiate(playerBody.name, _pos, Quaternion.identity).GetComponentInChildren<Camera>().enabled = true;
        PhotonNetwork.Destroy(gameObject);
    }
}
