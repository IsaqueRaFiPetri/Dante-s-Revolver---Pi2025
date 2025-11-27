using UnityEngine;

public class SetInputState : MonoBehaviour
{
    public void SetCursorLocked(bool _locked)
    {
        if (_locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void SetCursorVisible(bool _isVisible)
    {
        Cursor.visible = _isVisible;
    }
}
