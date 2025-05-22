using UnityEngine;
using UnityEngine.InputSystem;

public class Revolver : DamageInteraction
{
    [SerializeField]Camera playerCamera;
    public void Fire(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction, out hit, weaponsStats.maxDistance))
            {
                if (hit.collider.GetComponent<IKillable>() != null)
                {
                    DoDamage(hit.collider.gameObject);
                }
            }
        }
    }
}
