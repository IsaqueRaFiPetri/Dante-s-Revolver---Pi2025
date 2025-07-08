using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;

public class DisconectManager : MonoBehaviour
{
    public static DisconectManager instance;
    private void Awake()
    {
        instance = this;
    }
    public void Disconnect(string sceneName)
    {
        StartCoroutine(DisconectAndLoad(sceneName));
    }
    IEnumerator DisconectAndLoad(string sceneName)
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene(sceneName);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponents<PlayerMovementAdvanced>() != null)
            Disconnect("Menu");
    }
}
