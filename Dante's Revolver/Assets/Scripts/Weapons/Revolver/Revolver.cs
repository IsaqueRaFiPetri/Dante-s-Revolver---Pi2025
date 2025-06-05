using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class Revolver : DamageInteraction
{
    [SerializeField]Camera playerCamera;
    [SerializeField]WeaponStats weaponStats;

    bool canReload = true;
    int ammo;
    int maxAmmo;
    public TMP_Text ammoText;

    private void Start()
    {
        ammo = weaponStats.ammoValue;
        maxAmmo = weaponStats.ammoTotal;
    }
    public void Fire(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            ammo--;
            print("ammo: " + ammo);
            ammoText.text = ammo.ToString() + "/" + maxAmmo.ToString();
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction, out hit, weaponsStats.maxDistance))
            {
                if (hit.collider.TryGetComponent(out EnemyController target))
                {
                    DoDamage(target?.gameObject);
                }
            }
        }
    }
    public void Reload(InputAction.CallbackContext context)
    {
        print("try reload");
        if (context.canceled && ammo < maxAmmo && canReload)
        {
            print("reload");
            StartCoroutine(Reloading());
        }
    }

    IEnumerator Reloading()
    {
        print("reloading");
        canReload = false;
        ammo++;
        ammoText.text = ammo.ToString() + "/" + maxAmmo.ToString();
        yield return new WaitForSeconds(.5f);
        canReload = true;
        print("can reload: " + canReload);
        print(ammo + " / " + maxAmmo);
        
    }
}
