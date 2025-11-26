using UnityEngine;
using UnityEngine.Events;

public class Floor2Teleport : MonoBehaviour
{
    [SerializeField] UnityEvent OnCollision;
    [SerializeField] Transform _position;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out PlayerMovementAdvanced _pma))
        {
            _pma.transform.position = _position.position;
            OnCollision.Invoke();
        }
    }
}
