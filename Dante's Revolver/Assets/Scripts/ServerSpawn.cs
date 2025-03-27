using UnityEngine;
using Photon.Pun;

public class ServerSpawn : MonoBehaviour
{
    private void Start()
    {
        if(PhotonNetwork.IsConnected && PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.Instantiate("FirstPersonController", new Vector3(0, 1, 0), Quaternion.identity);
        }
    }
}
