using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource sfxSource;
    [SerializeField] private AudioSource themeSource;
    [SerializeField] private AudioSource combatSource;
    [SerializeField] private AudioClip defaultCardSfx;

    protected override void OnAwake()
    {
        base.OnAwake();

        sfxSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
        else
            sfxSource.PlayOneShot(defaultCardSfx);
    }

    public void PlayCombat()
    {
        StartCoroutine(Combat());
    }
    public void PlayExplore()
    {
        StartCoroutine(Explore());
    }
    IEnumerator Combat()
    {
        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            themeSource.volume = (1 - i);
            combatSource.volume = (i);
            yield return null;
        }
        combatSource.volume = 1;
        themeSource.volume = 0;
    }
    IEnumerator Explore()
    {
        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            themeSource.volume = (i);
            combatSource.volume = (1-i);
            yield return null;
        }
        combatSource.volume = 0f;
        themeSource.volume = 1;
    }
}
