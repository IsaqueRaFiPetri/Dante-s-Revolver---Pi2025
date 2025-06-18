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
    [SerializeField] float restartTime;

    [SerializeField] Image barrelImage;
    [SerializeField] Vector3 barrelRotation;
    [SerializeField]bool canRotate;
    public Charging charging;
    private void Start()
    {
        revolverClass = GetComponent<Revolver>();
        revolverMoves = GetComponent<RevolverMoves>();
    }
    private void FixedUpdate()
    {
        if (!canRotate)
        {
            return;
        }
        switch (charging)
        {
            case Charging.Charged:
                barrelImage.rectTransform.Rotate(barrelRotation * 3);
                break;
            case Charging.IsCharging:
                barrelImage.rectTransform.Rotate(barrelRotation);
                break;
        }
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
                    StartCoroutine(ChargeShooting());
                    charging = Charging.IsCharging;
                }
                break;
            case Charging.IsCharging:
                if (context.performed)
                {
                    print("StartedCharging");
                    canRotate = true;
                    StartCoroutine(IsCharging());
                }
                if(context.canceled)
                {
                    print("CanceledCharging");
                    canRotate = false;
                    revolverMoves.Taunting(.25f);
                    charging = Charging.IsCharging;
                    StopAllCoroutines();
                }
                break;
        }
    }
    IEnumerator IsCharging()
    {
        print("Charging");
        yield return new WaitForSeconds(chargeTime);
        print("Charged");
        charging = Charging.Charged;
    }
    IEnumerator ChargeShooting()
    {
        if (!revolverClass.GetCanShoot())
        {
            print("ended");
            StopCoroutine(ChargeShooting());
            StartCoroutine(RestartGun());
            charging = Charging.IsCharging;
            canRotate = false;
        }
        revolverClass.FireRay();
        yield return new WaitForSeconds(shootingTime);
        StartCoroutine(ChargeShooting());
    }
    IEnumerator RestartGun()
    {
        yield return new WaitForSeconds(restartTime);
        if(revolverClass.bulletCount == 6)
        {
            StartCoroutine(revolverClass.Reloading());
        }
    }
}
