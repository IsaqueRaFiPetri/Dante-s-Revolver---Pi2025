using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;

public class DisconectManager : MonoBehaviour
{
    public IEnumerator DisconectAndLoad(string sceneName)
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene(sceneName);
    }   
}
