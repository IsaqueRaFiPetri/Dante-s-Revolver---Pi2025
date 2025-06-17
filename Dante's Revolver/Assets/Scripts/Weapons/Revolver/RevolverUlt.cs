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
    [SerializeField]float chargeTime;
    [SerializeField]Image barrelImage;
    [SerializeField]Vector3 barrelRotation;
    bool rotateGun;


    public Charging charging;
    private void Start()
    {
        revolverClass = GetComponent<Revolver>();
        revolverMoves = GetComponent<RevolverMoves>();
    }
    private void FixedUpdate()
    {
        if (rotateGun)
        {
            barrelImage.rectTransform.Rotate(barrelRotation);
        }
    }
    public void ChargeShoot(InputAction.CallbackContext context)
    {
        switch (charging)
        {
            case Charging.Charged:
                if (context.canceled)
                {
                    print("ChargedShoot");
                    rotateGun = false;
                    revolverMoves.ResetTranform(barrelImage.transform);
                    charging = Charging.IsCharging;
                }
                break;
            case Charging.IsCharging:
                if (context.performed)
                {
                    print("StartedCharging");
                    rotateGun = true;
                    StartCoroutine(IsCharging());
                }
                if(context.canceled)
                {
                    print("CanceledCharging");
                    rotateGun = false;
                    revolverMoves.ResetTranform(barrelImage.transform);
                    StopCoroutine(IsCharging());
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
}
