using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Knife : DamageInteraction, IPowerable
{
    [SerializeField] Camera playerCamera;
    [SerializeField] UnityEvent OnAttack;
    [SerializeField] Image knifeImage;
    [SerializeField] IKillable _player;

    bool canAttack = true;
    private void Start()
    {
        _player = GetComponentInParent<IKillable>();
    }
    public void MeleeAttack(InputAction.CallbackContext context)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (!canAttack)
        {
            print("cant attack");
            return;
        }
        print("startedAttacking");
        StartCoroutine(Attacking());
    }

    bool SetCanAttack(bool canAttackIndex)
    {
        return canAttack = canAttackIndex;
    }

    IEnumerator Attacking()
    {
        print("attacking");
        OnAttack.Invoke();
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, weaponsStats.maxDistance))
        {
            if (hit.collider.TryGetComponent(out IKillable _target))
            {
                if (_target != _player)
                {
                    ShootParticle(bloodParticle.gameObject, hit);
                    ShootParticle(damageParticle.gameObject, hit);
                    DoDamage(_target);
                }
            }
            if (!hit.collider.TryGetComponent(out ILifeable lifePoint))
            {
                ShootParticle(shootParticle, hit);
            }
        }
        SetCanAttack(false);
        PowerImage(knifeImage, canAttack);
        yield return new WaitForSeconds(weaponsStats.shootCooldown);
        print("can attack again");
        SetCanAttack(true);
        PowerImage(knifeImage, canAttack);
    }

    public void PowerImage(Image image, bool active)
    {
        image.enabled = active;
    }
}
