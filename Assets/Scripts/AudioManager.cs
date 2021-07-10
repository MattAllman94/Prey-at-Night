using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : Singleton<AudioManager>
{
    public List<AudioSource> sourcesPool;
    public List<AudioClip> powerSFX; 
    public List<AudioClip> generalSFX;
    public List<AudioClip> uiSFX;
    public List<AudioClip> music;

    int currentSource = 0;

    public AudioSource rainSource;


    public void PlaySFX(AudioClip _clip)
    {
        if (!_GM.settings.SFX)   // exits function if SFX == false
            return;

        // increments currentSource unless currentSource == sourcesPool.Count
        currentSource = currentSource == sourcesPool.Count - 1 ? 0 : currentSource + 1;

        sourcesPool[currentSource].clip = _clip;
        sourcesPool[currentSource].Play();
    }

    public void ChangeBackgroundVolume(float _volume)
    {
        if (rainSource != null)
            rainSource.volume = _volume;
    }
}
