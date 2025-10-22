using UnityEngine;
using UnityEngine.Events;

public class HoleCounter : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] UnityEvent OnEnemyEntered;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == layerMask.value)
        {
            OnEnemyEntered.Invoke();
        }
    }

}
