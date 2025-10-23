using UnityEngine;
using UnityEngine.Events;

public class Moveset : MonoBehaviour
{
    [SerializeField] bool isInfinite;
    [SerializeField] UnityEvent OnStartMoveset;
    [SerializeField] UnityEvent OnFinishMoveset;
    [SerializeField] float movesetDuration;


    private void Start()
    {
        if (isInfinite)
        {
            movesetDuration = Mathf.Infinity;
        }
    }
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
