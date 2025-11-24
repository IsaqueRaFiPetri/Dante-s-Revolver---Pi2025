using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoatTravel : MonoBehaviour
{
    [SerializeField] UnityEvent OnFinishPath;
    [SerializeField] List<PlayerMovementAdvanced> _playerInBoat;

    public void FinishPath()
    {
        OnFinishPath.Invoke();
    }
}
