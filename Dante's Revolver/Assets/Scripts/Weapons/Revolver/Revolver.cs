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
    float lastRotationZ;
    float r;
    int bulletCount;

    private void Start()
    {
        ammo = weaponStats.ammoValue;
        maxAmmo = weaponStats.ammoTotal;
    }
    private void FixedUpdate()
    {
        float angle = Mathf.SmoothDampAngle(bulletHolder.eulerAngles.z, bulletHolder.eulerAngles.z + 60, ref r, .1f);
        bulletHolder.rotation = Quaternion.Euler(bulletHolder.eulerAngles.x, bulletHolder.eulerAngles.y, angle);
    }
    public void Fire(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            lastRotationZ = bulletHolder.eulerAngles.z + 60;
            ammo--;
            bulletImage[bulletCount].enabled = false;
            bulletCount++;

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
            bulletHolder.rotation = Quaternion.Euler(bulletHolder.eulerAngles.x, bulletHolder.eulerAngles.y, 0);
            lastRotationZ = bulletHolder.eulerAngles.z;
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
        yield return new WaitForSeconds(.2f);
        canReload = true;
        print("can reload: " + canReload);
        print(ammo + " / " + maxAmmo);
        
    }
}
