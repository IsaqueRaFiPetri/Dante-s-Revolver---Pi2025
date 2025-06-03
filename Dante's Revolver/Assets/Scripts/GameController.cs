using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController gameControl;
    private void Start()
    {
        gameControl = this;
    }
}
