using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ConstraintCalculer : MonoBehaviour
{
    [SerializeField] List<PlayerMovementAdvanced> _players;
    [SerializeField] List<Transform> _positions;
    [SerializeField] Transform _mostNextToObj;
    [SerializeField] Transform _selectedTarget;
    [SerializeField] LookAtConstraint _constraint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerMovementAdvanced _playerMovementAdvanced))
        {
            _players.Add(_playerMovementAdvanced);
            _positions.Add(_playerMovementAdvanced.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovementAdvanced _playerMovementAdvanced))
        {
            _positions.Remove(_playerMovementAdvanced.transform);
            _players.Remove(_playerMovementAdvanced);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(_selectedTarget != null)
        {
            SynchronizingVectors();
        }
    }
    void SynchronizingVectors()
    {
        foreach (Transform _player in _positions)
        {
            if(Vector3.Distance(_player.position, transform.position) < Vector3.Distance(_selectedTarget.position, transform.position))
            {
                _mostNextToObj = _player;
            }
            _selectedTarget.position = SetPos(_mostNextToObj);
        }
    }
    Vector3 SetPos(Transform _targetPos)
    {
        return _targetPos.position;
    }
}
