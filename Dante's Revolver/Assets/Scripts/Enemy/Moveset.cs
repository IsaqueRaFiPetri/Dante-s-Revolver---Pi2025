using UnityEngine;
using UnityEngine.Events;

public class Moveset : MonoBehaviour
{
    [SerializeField] UnityEvent OnStartMoveset;
    [SerializeField] UnityEvent OnFinishMoveset;
    [SerializeField] float movesetDuration;

    public UnityEvent GetOnFinish()
    {
        return OnFinishMoveset;
    }
    public UnityEvent GetOnStart()
    {
        return OnStartMoveset;
    }
    public float MovesetDuration()
    {
        return movesetDuration;
    }
    public void Clear()
    {

    }
}
