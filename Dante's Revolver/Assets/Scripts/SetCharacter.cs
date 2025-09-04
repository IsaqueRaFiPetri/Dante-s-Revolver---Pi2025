using UnityEngine;
using Photon.Pun;
using TMPro;
public class SetCharacter : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text cardText;

    private void Start()
    {
        NameSelector();    
    }

    public void NameSelector()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            cardText.text = "D - A - N - T - E";
        }
        else
        {
            cardText.text = "V I R G Í L I O";

        }
    }
}
