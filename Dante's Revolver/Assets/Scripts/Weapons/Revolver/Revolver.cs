using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Revolver : DamageInteraction
{
    [SerializeField]Camera playerCamera;
    [SerializeField]WeaponStats weaponStats;

    bool canReload = true;
    int ammo;
    int maxAmmo;
    [SerializeField] Transform bulletHolder;
    [SerializeField] List<Image> bulletImage;
    int bulletCount;

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
            bulletImage[bulletCount].enabled = false;
            bulletCount++;
            print("ammo: " + ammo);
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
            bulletHolder.rotation = Quaternion.Lerp(bulletHolder.rotation, Quaternion.Euler(bulletHolder.rotation.x, bulletHolder.rotation.y, 60f), Time.deltaTime * 0.01f);
            StartCoroutine(Reloading());
        }
    }

    IEnumerator Reloading()
    {
        print("reloading");
        canReload = false;
        ammo++;
        bulletCount--;
        bulletImage[bulletCount].enabled = true;
        yield return new WaitForSeconds(.5f);
        canReload = true;
        print("can reload: " + canReload);
        print(ammo + " / " + maxAmmo);
        
    }
}
