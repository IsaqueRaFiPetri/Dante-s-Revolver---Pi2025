using UnityEngine;

public enum PlataformStates
{
    Falling
}
public class PlataformController : MonoBehaviour
{
    [SerializeField] GameObject plataform;
    PlataformStates states;


    void Update()
    {
        switch (states)
        {
            case PlataformStates.Falling:

                break;
            default:
                break;
        }
    }

    void PlataformFallsBySeconds()
    {

    }
}
