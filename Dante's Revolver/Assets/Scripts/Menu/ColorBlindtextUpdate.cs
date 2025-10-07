using TMPro;
using UnityEngine;

public class ColorBlindtextUpdate : MonoBehaviour
{
    ColorblindTypes _colorBlindness;
    [SerializeField] TextMeshProUGUI text;

    private void Start()
    {
        UpdateNameColorblind();
    }
    public void UpdateNameColorblind()
    {
        _colorBlindness = (ColorblindTypes)Colorblindness._currentType;
        text.SetText(_colorBlindness.ToString());
    }
}
