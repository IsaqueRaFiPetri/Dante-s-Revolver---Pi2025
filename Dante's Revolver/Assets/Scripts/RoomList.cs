using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class RoomList : MonoBehaviourPunCallbacks
{
    public GameObject roomPrefab;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            print (roomList[i].Name);
            GameObject Room = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Content").transform);
            Room.GetComponent<Room>().name.text = roomList[i].Name;
        }
    }
}
