using System.Collections.Generic;
using UnityEngine;

public class ColorMaskSpriteChanger : MonoBehaviour
{
    [SerializeField] ColorMaskedElement ParentElement;
    [SerializeField] private Sprite BaseSprite;
    [SerializeField] private Sprite MatchSprite;
    [SerializeField] private SpriteRenderer Sprite;
    [SerializeField] private bool ReverseOnAll;

    private void Awake()
    {
        ParentElement.ColorChangedEvent += OnColorChange;
    }

    private void OnDestroy()
    {
        ParentElement.ColorChangedEvent -= OnColorChange;
    }

    private void OnColorChange(ColorMask mask)
    {
        bool match = mask == GlobalMaskManager.CurrentColorMask;
        if (mask == ColorMask.MASK_ALL && ReverseOnAll) match = false;

        Sprite.sprite = match ? MatchSprite : BaseSprite;
    }
}
