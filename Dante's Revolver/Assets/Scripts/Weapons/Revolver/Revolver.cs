using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;
using Photon.Pun;

public class Revolver : DamageInteraction
{
    [SerializeField]Camera playerCamera;

    [SerializeField] bool canShoot = true;
    [SerializeField] bool canReload = true;
    float shootCooldown;
    float reloadCooldown;

    RevolverMoves revolverMoves;
    [SerializeField] Transform bulletHolder;
    [SerializeField] List<Image> bulletImage;
    [SerializeField] UnityEvent OnShoot;
    [SerializeField] UnityEvent OnDryfire;
    [SerializeField] UnityEvent OnReload;
    [HideInInspector]public int bulletCount;

    private void Awake()
    {
        if (!photonView.IsMine)
        {
            this.enabled = false;
        }
    }
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
            OnDryfire.Invoke();
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
            if(hit.collider.TryGetComponent(out PlayerController _player))
            {
                print("hit player: " + _player.name);
            }
            if (hit.collider.TryGetComponent(out WeakPoint headshot))
            {
                print("hit headshot");
                ShootParticle(headshot.HeadshotParticle(), hit);
                headshot.TakeDamage(20000);
            }
            if (hit.collider.TryGetComponent(out IKillable _target))
            {
                ShootParticle(bloodParticle.gameObject, hit);
                ShootParticle(damageParticle.gameObject, hit);
                DoDamage(_target);
            }
            if (!hit.collider.TryGetComponent(out ILifeable lifePoint))
            {
                ShootParticle(shootParticle, hit);
            }
            if (hit.collider.TryGetComponent(out DoubleHeadEnemy doubleHead))
            {
                //doubleHead.GetGameObject().GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, photonView.ViewID);
                doubleHead.TakeDamage(photonView.ViewID);
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
    public IEnumerator Reloading()
    {
        canShoot = false;
        canReload = false;
        OnReload.Invoke();
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
