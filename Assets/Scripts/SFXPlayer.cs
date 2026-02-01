using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    internal static SFXPlayer instance;
    internal static void Play(string sfx)
    {
        if(instance != null)
        {
            instance.PlaySFX(sfx);
        }
    }

    [SerializeField] private AudioSource source;
    Dictionary<string, AudioClip> LoadedClips = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void PlaySFX(string sfx)
    {
        AudioClip clip = null;
        if (LoadedClips.ContainsKey(sfx))
        {
            clip = LoadedClips[sfx];
        }
        else
        {
            clip = Resources.Load<AudioClip>("Audio/" + sfx);
            if (clip != null) LoadedClips[sfx] = clip;
        }

        if(clip != null) source.PlayOneShot(clip);
    }
}
