using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.Events;

public class Revolver : DamageInteraction
{
    [SerializeField]Camera playerCamera;
    [SerializeField]WeaponStats weaponStats;

    bool canReload = true;
    [SerializeField]bool canShoot = true;
    int ammo;
    int maxAmmo;
    float shootCooldown;
    float reloadCooldown;

    [SerializeField] Transform bulletHolder;
    [SerializeField] List<Image> bulletImage;
    [SerializeField] UnityEvent OnShoot;
    int bulletCount;

    private void Start()
    {
        ammo = weaponStats.ammoValue;
        maxAmmo = weaponStats.ammoTotal;
        shootCooldown = weaponsStats.shootCooldown;
        reloadCooldown = weaponsStats.reloadCooldown;
    }
    public void Fire(InputAction.CallbackContext context)
    {
        if(ammo >= maxAmmo)
        {
            ammo = maxAmmo;
        }
        if(ammo <= 0)
        {
            ammo = 0;
            canShoot = false;
        }
        else
        {
            canShoot = true;
        }
        if (!context.canceled && canShoot)
        {
            StartCoroutine(Shooting());
        }
    }
    public void Reload(InputAction.CallbackContext context)
    {
        print("try reload");
        if (!context.canceled && ammo < maxAmmo && canReload)
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
        bulletCount--;
        bulletImage[bulletCount].enabled = true;
        bulletHolder.DOLocalRotate(new Vector3(0, 0, 0), 0.25f);
        yield return new WaitForSeconds(reloadCooldown); //.2f
        canReload = true;
        print("can reload: " + canReload);
        print(ammo + " / " + maxAmmo);
        
    }
    IEnumerator Shooting()
    {
        OnShoot.Invoke();
        ammo--;
        bulletHolder.DOLocalRotate(new Vector3(0, 0, bulletHolder.eulerAngles.z + 60f), 0.25f);
        bulletImage[bulletCount].enabled = false;
        bulletCount++;
        canShoot = false;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, weaponsStats.maxDistance))
        {
            if (hit.collider.TryGetComponent(out EnemyController target))
            {
                DoDamage(target?.gameObject);
            }
        }
        yield return new WaitForSeconds(shootCooldown);

        canShoot = true;

    }
}
