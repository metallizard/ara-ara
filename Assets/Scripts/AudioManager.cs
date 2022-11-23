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

    [SerializeField]
    private AudioClip _loseSFX;

    [SerializeField]
    private AudioClip _winSFX;

    [SerializeField]
    private AudioClip _5secLeftSFX;

    private bool _isPlayingRainSFX = false;

    public void PlayBGM()
    {
        _audioSource.clip = _bgmNormal;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    public void StopBGM()
    {
        _audioSource.Stop();
    }

    public void OrchestrateGameOverSFX()
    {
        _rainSFX.volume = 0;
        AudioSource.PlayClipAtPoint(_loseSFX, Camera.main.transform.position);
    }

    public void WinSFX()
    {
        _rainSFX.volume = 0;
        AudioSource.PlayClipAtPoint(_winSFX, Camera.main.transform.position);
    }

    public void PlayRain()
    {
        if (_rainSFX.isPlaying) return;

        StartCoroutine(PlayRainHelper());
    }

    public void StopRain()
    {
        StartCoroutine(StopRainHelper());
    }

    public void Play5SecLeft()
    {
        AudioSource.PlayClipAtPoint(_5secLeftSFX, Camera.main.transform.position);
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
