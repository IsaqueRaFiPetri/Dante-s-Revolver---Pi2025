using UnityEngine;
using TMPro;

public class Room : MonoBehaviour
{
    public TMP_Text r_name;

    public void JoinRoom()
    {
        GameObject.Find("CreateAndJoin").GetComponent<CreateAndJoin>().JoinRoomInList(r_name.text);
    }
}
