using UnityEngine;

public class CerberusDogHead : MonoBehaviour
{
    [SerializeField]GameObject playerTransform;
    private void FixedUpdate()
    {
        transform.LookAt(playerTransform.transform.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        playerTransform = other.gameObject;
    }
}
