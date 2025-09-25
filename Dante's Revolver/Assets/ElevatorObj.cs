using UnityEngine;

public class ElevatorObj : MonoBehaviour
{
    [SerializeField] bool isInitialElevator;
    private void OnBecameInvisible()
    {
        if (isInitialElevator)
        {
            Destroy(gameObject);
        }
    }
}
