using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPhotonScene : MonoBehaviour
{
    [SerializeField] bool isPhotonScene;
    public void LoadScene(string _sceneName)
    {
        if (isPhotonScene)
        {
            PhotonNetwork.LoadLevel(_sceneName);
        }
        else
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}
