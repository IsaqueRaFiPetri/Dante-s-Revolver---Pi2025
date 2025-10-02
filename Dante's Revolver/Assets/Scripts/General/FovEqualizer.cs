using UnityEngine;
using Photon.Pun;

public class FovEqualizer : MonoBehaviourPunCallbacks
{
    Camera cam;
    [SerializeField] Camera fovCamera;

    private void Start()
    {
        cam = GetComponent<Camera>();

        if (!photonView.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
    private void Update()
    {
        cam.fieldOfView = fovCamera.fieldOfView;
    }
}
