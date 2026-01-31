using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorMaskCollection", menuName = "GGJ26/ColorMaskDefinition", order = 0)]
public class ColorMaskCollection : SerializedScriptableObject
{
    [SerializeField] private Dictionary<ColorMask, Color> ColorDefinitions;

    internal static ColorMask GetColorMask(List<PrimaryColorMask> inputColor)
    {
        // Additioner tous les éléments
        int result = 0;
        foreach(var color in inputColor.Distinct())
        {
            result += (int) color;
        }

        // convertir en Color Mask
        return (ColorMask) result;
    }

    internal static HashSet<PrimaryColorMask> GetPrimaryColorMasks(params ColorMask[] inputMasks)
    {
        var result = new HashSet<PrimaryColorMask>();

        foreach(var inputMask in inputMasks)
        foreach (var mask in Enum.GetValues(typeof(PrimaryColorMask)))
        {
            if(((int)inputMask & (int)mask) > 0)
            {
                result.Add((PrimaryColorMask) mask);
            }
        }
        return result;
    }

    internal Color GetColor(ColorMask inputMask)
    {
        Color color = Color.white;

        if(ColorDefinitions.ContainsKey(inputMask))
        {
            color = ColorDefinitions[inputMask];
        }

        return color;
    }


}

public enum PrimaryColorMask
{
    MASK_1 = 1,
    MASK_2 = 2,
    MASK_3 = 4
}

public enum ColorMask
{
    MASK_NONE = 0,
    MASK_1 = 1,
    MASK_2 = 2,
    MASK_3 = 4,
    MASK_1_2 = 3,
    MASK_1_3 = 5,
    MASK_2_3 = 6,
    MASK_ALL = 7
}
