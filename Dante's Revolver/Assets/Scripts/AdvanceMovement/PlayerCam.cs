using UnityEngine;
using DG.Tweening;
using Photon.Pun;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviourPunCallbacks
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform camHolder;

    float xRotation;
    float yRotation;

    [SerializeField] Animator playerAnim;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (!photonView.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    public void Camera(InputAction.CallbackContext value)
    {
        float mouseX = value.ReadValue<Vector2>().x * sensX;
        float mouseY = value.ReadValue<Vector2>().y * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        playerAnim.SetFloat("CameraPos", xRotation);
    }

    public void DoFov(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }
    public void MoveYCamera(float yPos)
    {
        transform.DOLocalMoveY(yPos, 0.25f);
    }
}