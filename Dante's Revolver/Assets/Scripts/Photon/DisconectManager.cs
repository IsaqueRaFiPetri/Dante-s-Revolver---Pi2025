using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using System.Collections.Generic;

public class DisconectManager : MonoBehaviourPunCallbacks
{
    public static DisconectManager instance;
    public static HashSet<int> intentionallyLeftPlayers = new HashSet<int>();

    private void Awake()
    {
        instance = this;
    }

    public void Disconnect(string sceneName)
    {
        if (PhotonNetwork.LocalPlayer != null)
            intentionallyLeftPlayers.Add(PhotonNetwork.LocalPlayer.ActorNumber);

        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(sceneName);
    }

    public void LeaveRoom(string sceneName)
    {
        if (PhotonNetwork.LocalPlayer != null)
            intentionallyLeftPlayers.Add(PhotonNetwork.LocalPlayer.ActorNumber);

        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(sceneName);
    }

    public void DisconnectAndQuit()
    {
        if (PhotonNetwork.LocalPlayer != null)
            intentionallyLeftPlayers.Add(PhotonNetwork.LocalPlayer.ActorNumber);

        PhotonNetwork.Disconnect();
        Application.Quit();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected: " + cause);
    }
}
