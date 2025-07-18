using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField input;
    [SerializeField] string roomBaseName, sceneToLoad;

    private void Awake()
    {
        input.text = roomBaseName;
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(input.text , new RoomOptions() {MaxPlayers = 2 , IsVisible = true , IsOpen = true} , TypedLobby.Default);
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(input.text);
    }
    public void JoinRoomInList(string RoomName)
    {
        PhotonNetwork.JoinRoom(RoomName);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(sceneToLoad);
        print(PhotonNetwork.CountOfPlayersInRooms);
    }
}
