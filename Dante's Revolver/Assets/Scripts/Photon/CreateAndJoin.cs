using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField input;
    [SerializeField] string roomBaseName;
    [SerializeField] string sceneToLoad;

    private void Awake()
    {
        input.text = roomBaseName;
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 2,
            IsVisible = true,
            IsOpen = true,
            EmptyRoomTtl = 0
        };

        PhotonNetwork.CreateRoom(input.text, roomOptions, TypedLobby.Default);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(input.text);
    }

    public void JoinRoomInList(string RoomName)
    {
        PhotonNetwork.JoinRoom(RoomName);
        print(RoomName + "=============================================================================================");
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(sceneToLoad);
        Debug.Log("Players in room: " + PhotonNetwork.CurrentRoom.PlayerCount);
    }
}

