using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;


public class ColorMaskedElement : MonoBehaviour
{
    [SerializeField] internal ColorMaskCollection MaskCollection;
    [SerializeField] internal List<SpriteRenderer> ColoredSprites;

    [SerializeField] internal List<PrimaryColorMask> BaseMask;
    [SerializeField] internal float UnmatchAlpha = 1;
    [SerializeField] internal float MatchAlpha = 1;
    [SerializeField] internal bool UnmatchIfAll;

    private ColorMask CurrentMask;
    internal Action<ColorMask> ColorChangedEvent;
    private bool match;

    private void Awake()
    {
        GlobalMaskManager.GlobalColorChangedEvent += OnGlobalColorChanged;
    }

    private void OnDestroy()
    {
        GlobalMaskManager.GlobalColorChangedEvent -= OnGlobalColorChanged;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentMask =  ColorMaskCollection.GetColorMask(BaseMask);
        match = false;
        UpdateSpriteColors();
        ColorChangedEvent?.Invoke(CurrentMask);
    }

    private void OnGlobalColorChanged(ColorMask mask)
    {
        var primaryMasks = ColorMaskCollection.GetPrimaryColorMasks(mask);
        List<PrimaryColorMask> total = new List<PrimaryColorMask>();
        total.AddRange(primaryMasks);
        total.AddRange(BaseMask);

        var newMask = ColorMaskCollection.GetColorMask(total);

        CurrentMask = newMask;

        match = CurrentMask == mask;

        if (UnmatchIfAll && CurrentMask == ColorMask.MASK_ALL) match = false;

        ColorChangedEvent?.Invoke(newMask);

        UpdateSpriteColors();
    }

    private void UpdateSpriteColors()
    {   
        Color currentColor = MaskCollection.GetColor(CurrentMask);
        currentColor.a = match ? MatchAlpha : UnmatchAlpha;

        foreach (var sprite in ColoredSprites)
        {
            sprite.DOColor(currentColor, .5f);
        }
    }
}
