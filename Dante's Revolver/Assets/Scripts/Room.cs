using UnityEngine;
using TMPro;

public class Room : MonoBehaviour
{
    public TMP_Text name;

    public void JoinRoom()
    {
        GameObject.Find("CreateAndJoin").GetComponent<CreateAndJoin>().JoinRoomInList(name.text);
    }
}
