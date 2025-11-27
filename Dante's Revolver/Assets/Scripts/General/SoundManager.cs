using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private float minPitch = 0.5f;
    [SerializeField] private float maxPitch = 1.5f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float changeInterval = 3;

    void Start()
    {
        StartCoroutine(VolumePitchValueChanger());
    }

    public IEnumerator VolumePitchValueChanger()
    {
        while (true)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            yield return new WaitForSeconds(changeInterval);
        }
    }
}