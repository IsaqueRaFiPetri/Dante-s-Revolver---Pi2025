using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    public TMP_InputField input_create;
    public TMP_InputField input_join;

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(input_create.text, new RoomOptions() {MaxPlayers = 2 , IsVisible = true , IsOpen = true } , TypedLobby.Default , null);
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(input_join.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GamePlay");
        print(PhotonNetwork.CountOfPlayersInRooms);
    }
}
