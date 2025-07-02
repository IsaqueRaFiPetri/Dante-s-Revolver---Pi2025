using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class Explosives : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject explosivePrefab;
    public void SpawnExplosive(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PhotonNetwork.Instantiate(explosivePrefab.name, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
        }
    }
}
