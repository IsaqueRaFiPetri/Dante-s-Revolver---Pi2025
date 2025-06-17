using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum Charging
{
    Charged, IsCharging
}
public class RevolverUlt : MonoBehaviour
{
    Revolver revolverClass;
    RevolverMoves revolverMoves;
    [SerializeField] float chargeTime;
    [SerializeField] float shootingTime;

    public Charging charging;
    private void Start()
    {
        revolverClass = GetComponent<Revolver>();
        revolverMoves = GetComponent<RevolverMoves>();
    }
    public void ChargeShoot(InputAction.CallbackContext context)
    {
        if (!revolverClass.GetCanShoot())
        {
            return;
        }
        switch (charging)
        {
            case Charging.Charged:
                if (context.canceled)
                {
                    print("ChargedShoot");
                    transform.DOLocalRotate(new Vector3(transform.rotation.x, transform.rotation.y, 0), .25f);
                    StartCoroutine(ChargeShooting());
                    charging = Charging.IsCharging;
                }
                break;
            case Charging.IsCharging:
                if (context.performed)
                {
                    print("StartedCharging");
                    StartCoroutine(IsCharging());
                }
                if(context.canceled)
                {
                    print("CanceledCharging");
                    StopCoroutine(IsCharging());
                    revolverMoves.SetTransform(transform, new Vector3(transform.rotation.x, transform.rotation.y, 0), .25f);
                }
                break;
        }
    }
    IEnumerator IsCharging()
    {
        print("Charging");
        StartCoroutine(revolverMoves.InfiniteTaunting(.25f, .25f));
        yield return new WaitForSeconds(chargeTime);
        print("Charged");
        StopCoroutine(revolverMoves.InfiniteTaunting(.25f, .25f));
        charging = Charging.Charged;
    }
    IEnumerator ChargeShooting()
    {
        if (!revolverClass.GetCanShoot())
        {
            StopCoroutine(ChargeShooting());
            print("ended");
        }
        revolverClass.FireRay();
        yield return new WaitForSeconds(shootingTime);
        StartCoroutine(ChargeShooting());
    }
}
