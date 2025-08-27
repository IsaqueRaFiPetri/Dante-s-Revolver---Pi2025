using UnityEngine;
using Photon.Pun;

public class SpectatorManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        if (!photonView.IsMine)
        {
            this.enabled = false;
        }
    }

}
