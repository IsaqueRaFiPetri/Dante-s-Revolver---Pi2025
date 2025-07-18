using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField input;
    [SerializeField] string roomBaseName, sceneToLoad;
    [HideInInspector] public RoomOptions roomOpt;

    private void Awake()
    {
        input.text = roomBaseName;
        roomOpt.MaxPlayers = 2;
        roomOpt.IsVisible = true;
        roomOpt.IsOpen = true;
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(input.text , roomOpt , TypedLobby.Default);
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
