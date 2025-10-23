using UnityEngine;
using UnityEngine.Events;

public class HoleCounter : MonoBehaviour
{
    [SerializeField] int layerToFind;
    [SerializeField] UnityEvent OnEnemyEntered;
    
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer == layerToFind)
        {
            OnEnemyEntered.Invoke();
        }
    }
}
