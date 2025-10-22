using UnityEngine;
using UnityEngine.Events;

public class Layer2Manager : MonoBehaviour
{
    [SerializeField] int _minEnemyQuantity, _maxEnemyQuantity;
    [SerializeField] UnityEvent OnQuantityReached;
    public void DetectEnemyQuantity()
    {
        if(_minEnemyQuantity >= _maxEnemyQuantity)
        {
            OnQuantityReached.Invoke();
        }
        else
        {
            _minEnemyQuantity++;
        }
    }
}
