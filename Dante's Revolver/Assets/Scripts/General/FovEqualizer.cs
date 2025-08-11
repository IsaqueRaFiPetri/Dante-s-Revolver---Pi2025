using UnityEngine;

public class FovEqualizer : MonoBehaviour
{
    Camera cam;
    [SerializeField] Camera fovCamera;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }
    private void Update()
    {
        cam.fieldOfView = fovCamera.fieldOfView;
    }
}
