using Photon.Pun;
using UnityEngine;

public class LoadPhotonScene : MonoBehaviour
{
    public void LoadScene(string _sceneName)
    {
        PhotonNetwork.LoadLevel(_sceneName);
    }
}
