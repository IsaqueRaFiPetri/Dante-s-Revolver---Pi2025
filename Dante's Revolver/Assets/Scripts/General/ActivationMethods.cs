using UnityEngine;
using UnityEngine.Events;

public class ActivationMethods : MonoBehaviour
{
    int playersInside = 0;

    [SerializeField] bool requireBoth = true;

    [SerializeField] UnityEvent ActiveCall;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInside++;

            CheckActivation();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInside--;

            CheckActivation();
        }
    }

    private void CheckActivation()
    {
        if (requireBoth)
            ActiveCall.Invoke();
        else
            ActiveCall.Invoke();
    }

}
