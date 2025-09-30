using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivationMethods : MonoBehaviour
{
    [SerializeField] List<PlayerMovementAdvanced> _playersInside;

    [SerializeField] UnityEvent ActiveCall;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovementAdvanced _playerMovement))
        {
            _playersInside.Add(_playerMovement);
            CheckActivation();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out PlayerMovementAdvanced _playerMovement))
        {
            _playersInside.Remove(_playerMovement);
            CheckActivation();
        }
    }

    private void CheckActivation()
    {
        if(_playersInside.Count >= 2)
        {
            ActiveCall?.Invoke();
        }
    }

}
