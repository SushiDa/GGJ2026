using System.Collections.Generic;
using UnityEngine;


public class ColorMaskEnabler : MonoBehaviour
{
    [SerializeField] ColorMaskedElement ParentElement;
    [SerializeField] private List<Behaviour> BehaviourList;
    [SerializeField] private List<GameObject> GameObjects;
    [SerializeField] private bool DisableOnMatch;
    [SerializeField] private bool EnableOnAll;

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

        if (DisableOnMatch) match = !match;

        if (mask == ColorMask.MASK_ALL)
        {
            match = EnableOnAll;
        }


        foreach (var behaviour in BehaviourList)
        {
            behaviour.enabled = match;
        }

        foreach (var gameObject in GameObjects)
        {
            gameObject.SetActive(match);
        }
    }
}
