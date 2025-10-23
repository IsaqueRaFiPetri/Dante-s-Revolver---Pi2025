using UnityEngine;
using UnityEngine.Events;

public class InfiniteMovesetController : MonoBehaviour
{
    [SerializeField] int _quantiy;
    [SerializeField] int _minQuantityNeededToActivate;
    [SerializeField] Moveset _moveset;
    [SerializeField] UnityEvent OnQuantityReached;

    public void DetectQuantityToActive()
    {
        if (_quantiy >= _minQuantityNeededToActivate)
        {
            OnQuantityReached.Invoke();
            _moveset.GetOnFinish().Invoke();
        }
        else
        {
            _quantiy++;
        }
    }
}
