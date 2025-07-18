using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class RoomList : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject roomPrefab;
    public List<GameObject> allRooms;
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for(int i = 0; i<allRooms.Count; i++)
        {
            if (allRooms[i] != null)
            {
                Destroy(allRooms[i]);
            }
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            
            if (roomList.Count == 0)
                return;

            else if (roomList[i].IsOpen && roomList[i].IsVisible && roomList[i].PlayerCount >= 1)
            {
                GameObject Room = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Content").transform);
                Room.GetComponent<Room>().r_name.text = roomList[i].Name;

                allRooms[i] = Room;
            }
        }
    }
    public override void OnLeftLobby()
    {
        foreach (GameObject room in allRooms)
        {
            for (int i = 0; i < allRooms.Count; i++)
            {
                if (room.name == allRooms[i].name)
                {
                    Destroy(room);
                }
            }
        }
    }
}
//https://www.youtube.com/watch?v=GipaiFVyQt0&list=PL_l4pPo5eSc2nHazxm_VjIrURNdWcWQpd&index=4&t=51s&ab_channel=BudGames