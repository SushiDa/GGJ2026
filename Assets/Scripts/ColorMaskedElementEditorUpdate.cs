using UnityEngine;

[ExecuteInEditMode]
public class ColorMaskedElementEditorUpdate : MonoBehaviour
{
#if UNITY_EDITOR
    ColorMaskedElement _elm;
    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying) return;
        UpdateColors();
        UpdateSprites();
    }

    private void UpdateColors()
    {
        if (_elm == null) _elm = GetComponent<ColorMaskedElement>();
        if (_elm == null || _elm.MaskCollection == null) return;

        Color currentColor = _elm.MaskCollection.GetColor(ColorMaskCollection.GetColorMask(_elm.BaseMask));

        foreach (var sprite in _elm.ColoredSprites)
        {
            sprite.color = currentColor;
        }
    }

    private void UpdateSprites()
    {

    }
#endif
}
