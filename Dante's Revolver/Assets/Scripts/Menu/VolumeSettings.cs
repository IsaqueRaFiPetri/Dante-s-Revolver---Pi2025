using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer myMixer;
    [SerializeField] Slider musicSlider, _SfxSlider;

    private void Start()
    {
        SetMusicVolume();
        SetSfxVolume();
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
    }

    public void SetSfxVolume()
    {
        float volume = _SfxSlider.value;
        myMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
    }
}
