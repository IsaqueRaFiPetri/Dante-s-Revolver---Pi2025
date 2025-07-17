using Photon.Pun;
using UnityEngine;

public class SetIsMine : MonoBehaviourPunCallbacks
{
    [SerializeField] bool hasToBeMine;
    [SerializeField] SkinnedMeshRenderer skinRender;
    private void Awake()
    {
        if(photonView.IsMine == hasToBeMine)
        {
            skinRender.enabled = false;
        }
    }
}
