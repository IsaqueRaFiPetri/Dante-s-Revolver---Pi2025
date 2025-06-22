using Photon.Pun;
using UnityEngine;

public class SetIsMine : MonoBehaviourPunCallbacks
{
    [SerializeField] bool hasToBeMine;
    private void Awake()
    {
        if(photonView.IsMine == hasToBeMine)
        {
            gameObject.SetActive(false);
        }
    }
}
