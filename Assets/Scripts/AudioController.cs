using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Ins;

    [SerializeField] AudioSource _musicAudioSource;
    [SerializeField] AudioSource _soundAudioSource;
    [SerializeField] AudioClip _button;
    [SerializeField] AudioClip _damage;
    [SerializeField] AudioClip _place;
    [SerializeField] AudioClip _destroy;
    [SerializeField] AudioClip _finalBoss;
    [SerializeField] AudioClip _win;
    [SerializeField] AudioClip _lose;
    [SerializeField] List<AudioClip> _ingameList;
    [SerializeField] AudioClip _menu;
    [SerializeField] AudioClip _lightningSFX;

    private void Awake()
    {
        if (Ins)
        {
            Destroy(Ins);
        }
        Ins = this;
        DontDestroyOnLoad(Ins);
        GameManager.OnResetLevel += PlayIngameSFX;
    }

    public void PlayButtonSFX()
    {
        _soundAudioSource.PlayOneShot(_button);
    }

    public void PlayDamageSFX()
    {
        _soundAudioSource.PlayOneShot(_damage);
    }

    public void PlayPlaceSFX()
    {
        _soundAudioSource.PlayOneShot(_place);
    }

    public void PlayDestroySFX()
    {
        _soundAudioSource.PlayOneShot(_destroy);
    }

    public void PlayWinSFX()
    {
        _musicAudioSource.Stop();
        _soundAudioSource.PlayOneShot(_win);
    }

    public void PlayLoseSFX()
    {
        _musicAudioSource.Stop();
        _soundAudioSource.PlayOneShot(_lose);
    }

    public void PlayIngameSFX()
    {
        _musicAudioSource.Stop();
        _musicAudioSource.clip = _ingameList[Random.Range(0, _ingameList.Count - 1)];
        _musicAudioSource.Play();
    }

    public void PlayMenuSoundtrack()
    {
        _musicAudioSource.Stop();
        _musicAudioSource.clip = _menu;
        _musicAudioSource.Play();
    }

    public void PlayBossFight() {
        _musicAudioSource.Stop();
        _musicAudioSource.clip = _finalBoss;
        _musicAudioSource.Play();
    }

    public void PlayLightningSFX()
    {
        _soundAudioSource.PlayOneShot(_lightningSFX);
    }
}
