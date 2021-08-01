using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class AudioManager : Singleton<AudioManager>
{
    public List<AudioSource> sourcesPool;
    public List<AudioClip> powerSFX; 
    public List<AudioClip> generalSFX;
    public List<AudioClip> uiSFX;
    public List<AudioClip> music;

    int currentSource = 0;

    public AudioSource rainSource;
    public AudioSource musicSource;

    public AudioSource castAudioSource;
    public AudioSource playerAudioSource;
    public AudioSource uiAudioSource;

    [Header("Player Sounds")]
    public AudioClip stakeHitNpc;
    public AudioClip stakeHitEnviro;

    public AudioClip playerAttack;
    public AudioClip playerHurt;

    [Header("UI Sounds")]
    public AudioClip uiHover;
    public AudioClip uiClick;
    public AudioClip uiDrag;
    public AudioClip uiPlay;

    [Header("Music")]
    public AudioClip titleMusic;
    public AudioClip inGameMusic;
    

    public void PlaySFX(AudioClip _clip, Vector3 _pos, bool _randomPitch = true)
    {
        if (!_GM.settings.SFX)   // exits function if SFX == false
            return;

        // increments currentSource unless currentSource == sourcesPool.Count
        currentSource = currentSource == sourcesPool.Count - 1 ? 0 : currentSource + 1;

        sourcesPool[currentSource].clip = _clip;
        sourcesPool[currentSource].gameObject.transform.position = _pos;

        sourcesPool[currentSource].pitch = _randomPitch ? Random.Range(0.8f, 1f) : 1f;

        sourcesPool[currentSource].Play();
    }
    
    public void PlayCastSound(AudioClip _clip)
    {
        if (!castAudioSource.isPlaying)
        {
            castAudioSource.clip = _clip;
            castAudioSource.pitch = Random.Range(0.8f, 1f);
            castAudioSource.Play();
        }        
    }

    public void PlayerAttackSound(bool _Attacking = true)
    {      
            playerAudioSource.pitch = Random.Range(0.9f, 1f);
            if (_Attacking)
                playerAudioSource.clip = playerAttack;
            else
                playerAudioSource.clip = playerHurt;
            playerAudioSource.Play();              
    }

    public void NPCHurtSound(AudioSource _source)
    {
        if(!_source.isPlaying)
        {
            _source.pitch = Random.Range(0.9f, 1f);
            _source.Play();
        }
    }

    public void PlayFootStep(AudioSource _source)
    {
        if (!_source.isPlaying)
        {
            _source.pitch = Random.Range(0.8f, 1f);
            _source.Play();
        }   
    }


    public void PlayUISound(int _clipNum)
    {
        switch (_clipNum)
        {
            case (1):
                uiAudioSource.clip = uiHover;
                break;
            case (2):
                uiAudioSource.clip = uiClick;
                break;
            case (3):
                uiAudioSource.clip = uiDrag;
                break;
            case (4):
                uiAudioSource.clip = uiPlay;
                break;
        }
        uiAudioSource.pitch = Random.Range(0.95f, 1f);
        uiAudioSource.Play();
    }

    public void ChangeRainVolume(float _volume)
    {
        if (rainSource != null)
            rainSource.volume = _volume;
    }
    public void ChangeMusic(bool _toInGame = true)
    {
        if (_toInGame)
        {
            musicSource.clip = inGameMusic;
            musicSource.Play();
        }
        else
        {
            musicSource.clip = titleMusic;
            musicSource.Play();
        }
    }
}
