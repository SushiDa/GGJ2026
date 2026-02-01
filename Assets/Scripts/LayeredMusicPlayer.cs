using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayeredMusicPlayer : SerializedMonoBehaviour
{
    [SerializeField] AudioSource MainSource;
    [SerializeField] Dictionary<PrimaryColorMask, AudioSource> OverlaySources;
    [SerializeField] float FadeDuration;
    [SerializeField] float SyncInterval;

    private void Awake()
    {
        MainSource.volume = 1;
        foreach (var source in OverlaySources.Values)
        {
            source.volume = 0;
        }

        GlobalMaskManager.GlobalColorChangedEvent += OnGlobalColorChanged;

        // StartCoroutine(SyncSources());
    }

    private void OnDestroy()
    {
        GlobalMaskManager.GlobalColorChangedEvent -= OnGlobalColorChanged;
    }

    private void OnGlobalColorChanged(ColorMask mask)
    {
        var primaryMasks = ColorMaskCollection.GetPrimaryColorMasks(mask);
        foreach (var key in OverlaySources.Keys)
        {
            OverlaySources[key].DOKill();
            OverlaySources[key].DOFade(primaryMasks.Contains(key) ? 1 : 0, FadeDuration);
        }
    }

    private IEnumerator SyncSources()
    {
        while (true)
        {
            foreach (var source in OverlaySources.Values)
            {
                source.time = MainSource.time;
            }

            yield return new WaitForSeconds(SyncInterval);
        }
    }
}
