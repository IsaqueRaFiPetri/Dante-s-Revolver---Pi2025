using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class Revolver : DamageInteraction
{
    [SerializeField]Camera playerCamera;
    [SerializeField]WeaponStats weaponStats;

    bool canShoot = true;
    bool canReload = true;
    float shootCooldown;
    float reloadCooldown;

    RevolverMoves revolverMoves;
    [SerializeField] Transform bulletHolder;
    [SerializeField] List<Image> bulletImage;
    [SerializeField] UnityEvent OnShoot;
    int bulletCount;

    private void Start()
    {
        shootCooldown = weaponsStats.shootCooldown;
        reloadCooldown = weaponsStats.reloadCooldown;
        revolverMoves = GetComponent<RevolverMoves>();
    }

    public bool GetCanShoot()
    {
        return canShoot;
    }
    public bool SetCanShoot(bool setCanShoot)
    {
        return canShoot = setCanShoot;
    }
    public void Fire(InputAction.CallbackContext context)
    {
        if (context.performed && canShoot)
        {
            StartCoroutine(Shooting());
        }
    }
    public void FireRay()
    {
        if (bulletCount >= 6)
        {
            canShoot = false;
            StopCoroutine(Shooting());
        }
        OnShoot.Invoke();
        bulletImage[bulletCount].enabled = false;
        bulletCount++;
        canShoot = false;
        canReload = false;

        StartCoroutine(revolverMoves.Rebound(bulletHolder));

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, weaponsStats.maxDistance))
        {
            if (hit.collider.TryGetComponent(out EnemyController target))
            {
                DoDamage(target?.gameObject);
                target.BloodParticle(hit.point);
            }
        }
    }
    public void Reload(InputAction.CallbackContext context)
    {
        if (context.performed && canReload)
        {
            StartCoroutine(Reloading());
        }
    }
    IEnumerator Reloading()
    {
        canShoot = false;
        canReload = false;
        bulletCount = 0;
        for (int i = 0; i < bulletImage.Count; i++)
        {
            bulletImage[i].enabled = true;
        }
        StartCoroutine(revolverMoves.Taunting(.1f));
        revolverMoves.SetTransform(bulletHolder, new Vector3(0, 0, 0), .25f);
        yield return new WaitForSeconds(reloadCooldown); //.2f
        canShoot = true;
        canReload = true;
    }
    IEnumerator Shooting()
    {
        FireRay();
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
        canReload = true;
    }
}
