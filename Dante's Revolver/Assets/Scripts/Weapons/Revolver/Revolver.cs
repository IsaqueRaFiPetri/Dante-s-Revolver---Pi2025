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
    public void Fire(InputAction.CallbackContext context)
    {
        if (!context.canceled && canShoot)
        {
            StartCoroutine(Shooting());
        }
    }
    public void Reload(InputAction.CallbackContext context)
    {
        if (!context.canceled)
        {
            StartCoroutine(Reloading());
        }
    }
    IEnumerator Reloading()
    {
        canShoot = false;
        bulletCount = 0;
        for (int i = 0; i < bulletImage.Count; i++)
        {
            bulletImage[i].enabled = true;
        }
        StartCoroutine(revolverMoves.Taunting(bulletHolder));
        yield return new WaitForSeconds(reloadCooldown); //.2f
        canShoot = true;
        
    }
    IEnumerator Shooting()
    {
        OnShoot.Invoke();
        bulletImage[bulletCount].enabled = false;
        bulletCount++;
        canShoot = false;

        StartCoroutine(revolverMoves.Rebound(bulletHolder));

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
