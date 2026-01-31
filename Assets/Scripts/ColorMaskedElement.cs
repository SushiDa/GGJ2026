using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;


public class ColorMaskedElement : MonoBehaviour
{
    [SerializeField] internal ColorMaskCollection MaskCollection;
    [SerializeField] internal List<SpriteRenderer> ColoredSprites;

    [SerializeField] internal List<PrimaryColorMask> BaseMask;

    [SerializeField] private ColorMask CurrentMask;
    internal Action<ColorMask> ColorChangedEvent;

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
        
        ColorChangedEvent?.Invoke(newMask);

        UpdateSpriteColors();

    }

    private void UpdateSpriteColors()
    {   
        Color currentColor = MaskCollection.GetColor(CurrentMask);

        foreach (var sprite in ColoredSprites)
        {
            sprite.DOColor(currentColor, .5f);
        }
    }
}
