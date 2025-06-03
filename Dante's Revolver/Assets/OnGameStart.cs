using UnityEngine;

public class OnGameStart : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 240;
    }
}
