using UnityEngine;

public class FovEqualizer : MonoBehaviour
{
    Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }
    private void Update()
    {
        cam.fieldOfView = Camera.main.fieldOfView;
    }
}
