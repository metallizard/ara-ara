using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _bgmNormal;

    [SerializeField]
    private AudioSource _rainSFX;

    public void Play()
    {
        _audioSource.clip = _bgmNormal;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    public void PlayRain()
    {
        StartCoroutine(PlayRainHelper());
    }

    public void StopRain()
    {
        StartCoroutine(StopRainHelper());
    }

    public IEnumerator PlayRainHelper()
    {
        _rainSFX.volume = 0;
        _rainSFX.Play();
        float v = 0;
        while(v < 1)
        {
            v += Time.deltaTime;

            _rainSFX.volume = v;
            yield return null;
        }
    }

    public IEnumerator StopRainHelper()
    {
        _rainSFX.volume = 1;
        float v = 1;
        while (v > 0)
        {
            v -= Time.deltaTime;

            _rainSFX.volume = v;
            yield return null;
        }

        _rainSFX.Stop();
    }
}
