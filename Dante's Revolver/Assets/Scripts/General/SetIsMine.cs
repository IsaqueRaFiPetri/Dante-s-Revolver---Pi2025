using Photon.Pun;
using UnityEngine;

public enum SetIsMineEnum
{
    Renderer, Object
}
public class SetIsMine : MonoBehaviourPunCallbacks
{
    [SerializeField] bool hasToBeMine;
    [SerializeField] SkinnedMeshRenderer skinRender;
    [SerializeField] SetIsMineEnum setIsMineEnum;
    private void Awake()
    {
        if (photonView.IsMine == hasToBeMine)
        {
            switch (setIsMineEnum)
            {
                case SetIsMineEnum.Renderer:
                    skinRender.enabled = false;
                    break;
                case SetIsMineEnum.Object:
                    gameObject.SetActive(false);
                    break;
            }
        }
        if (!photonView.IsMine)
        {
            gameObject.SetActive(false);
        }
    }
}
