using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Knife : DamageInteraction, IPowerable
{
    [SerializeField] Camera playerCamera;
    [SerializeField] UnityEvent OnAttack;
    [SerializeField] Image knifeImage;

    bool canAttack = true;

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
            if (hit.collider.TryGetComponent(out EnemyController target))
            {
                DoDamage(target?.gameObject);
                ShootParticle(bloodParticle.gameObject ,hit);
                ShootParticle(damageParticle.gameObject, hit);
            }
            if (!hit.collider.GetComponent<EnemyController>())
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
