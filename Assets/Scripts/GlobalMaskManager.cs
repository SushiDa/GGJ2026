using DG.Tweening;
using NUnit.Framework;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GlobalMaskManager : SerializedMonoBehaviour
{
    internal static Action<ColorMask> GlobalColorChangedEvent;
    internal static Action<PrimaryColorMask> ColorMaskAddedEvent;
    internal static Action<PrimaryColorMask> ColorMaskRemovedEvent;
    internal static Action ResetManagerEvent;
    internal static Action UpdateAbilitiesEvent;

    internal static ColorMask CurrentColorMask;
    internal static Stack<PrimaryColorMask> CurrentMaskStack = new Stack<PrimaryColorMask>();
    internal static List<PrimaryColorMask> AvailableColorMasks = new List<PrimaryColorMask>();


    private void Awake()
    {
        ResetManager();
        GlobalColorChangedEvent += UpdateGlobalUI;
        ColorMaskAddedEvent += OnMaskAdded;
        ColorMaskRemovedEvent += OnMaskRemoved;
        ResetManagerEvent += OnResetManager;
        UpdateAbilitiesEvent += UpdateAvailableAbilitiesUI;
    }

    private void OnDestroy()
    {
        GlobalColorChangedEvent -= UpdateGlobalUI;
        ColorMaskAddedEvent -= OnMaskAdded;
        ColorMaskRemovedEvent -= OnMaskRemoved;
        ResetManagerEvent -= OnResetManager;
        UpdateAbilitiesEvent -= UpdateAvailableAbilitiesUI;
    }

    internal static bool TryAddMask(PrimaryColorMask mask)
    {
        if (!AvailableColorMasks.Contains(mask) || CurrentMaskStack.Contains(mask) || CurrentMaskStack.Count >= 3)
        {
            SFXPlayer.Play("No");
            return false;
        }
        CurrentMaskStack.Push(mask);
        CurrentColorMask = ColorMaskCollection.GetColorMask(CurrentMaskStack.ToList());
        SFXPlayer.Play("Calque");
        Notify();
        ColorMaskAddedEvent?.Invoke(mask);
        return true;
    }

    internal static bool TryRemoveMask()
    {
        if (CurrentMaskStack.TryPop(out var mask))
        {
            CurrentColorMask = ColorMaskCollection.GetColorMask(CurrentMaskStack.ToList());
            SFXPlayer.Play("Calque");
            Notify();
            ColorMaskRemovedEvent?.Invoke(mask);
            return true;
        }
        SFXPlayer.Play("No");
        return false;
    }

    internal static void SetAvailableMasks(List<PrimaryColorMask> masks)
    {
        AvailableColorMasks.Clear();
        AvailableColorMasks.AddRange(masks);
        UpdateAbilitiesEvent?.Invoke();
    }

    internal static void ResetManager()
    {
        ResetManagerEvent?.Invoke();
        CurrentColorMask = ColorMask.MASK_NONE;
        AvailableColorMasks.Clear();
        CurrentMaskStack.Clear();
        Notify();
    }

    private static void Notify()
    {
        GlobalColorChangedEvent?.Invoke(CurrentColorMask);
    }

    [SerializeField] ColorMaskCollection MaskCollection;
    [SerializeField] private Image ChromaPastille;

    [SerializeField] private Sprite EmptySprite;

    [SerializeField] private Dictionary<PrimaryColorMask, Sprite> MaskSprites;
    [SerializeField] private List<Image> MaskLayers;

    [SerializeField] private Dictionary<PrimaryColorMask, Image> AbilityImages;

    private void UpdateGlobalUI(ColorMask mask)
    {
        ChromaPastille.DOKill();
        Color color = MaskCollection.GetColor(mask);
        ChromaPastille.DOColor(color, .5f);
    }
    private void OnResetManager()
    {
        foreach (var layer in MaskLayers)
        {
            layer.DOKill();
            layer.color = new Color(1, 1, 1, 0);
        }

        ChromaPastille.DOKill();
        ChromaPastille.color = MaskCollection.GetColor(ColorMask.MASK_NONE);
        UpdateAvailableAbilitiesUI();

    }

    private void UpdateAvailableAbilitiesUI()
    {
        foreach(var key in AbilityImages.Keys)
        {
            if(AvailableColorMasks.Contains(key))
            {
                AbilityImages[key].transform.GetChild(0).GetComponent<Image>().enabled = false;
                AbilityImages[key].color = new Color(1, 1, 1, .55f);
            }
            else
            {
                AbilityImages[key].transform.GetChild(0).GetComponent<Image>().enabled = true;
                AbilityImages[key].color = Color.clear;
            }
        }
    }

    private void OnMaskRemoved(PrimaryColorMask mask)
    {
        int index = CurrentMaskStack.Count;

        MaskLayers[index].DOKill();
        MaskLayers[index].GetComponent<RectTransform>().DOKill();
        MaskLayers[index].color = Color.white;
        MaskLayers[index].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        MaskLayers[index].DOColor(new Color(1, 1, 1, 0), .5f);
        MaskLayers[index].GetComponent<RectTransform>().DOAnchorPos(Vector2.up * 20, .5f);

        AbilityImages[mask].DOKill();
        AbilityImages[mask].DOColor(new Color(1, 1, 1, .55f), .5f);
    }

    private void OnMaskAdded(PrimaryColorMask mask)
    {
        int index = CurrentMaskStack.Count - 1;
        MaskLayers[index].DOKill();
        MaskLayers[index].GetComponent<RectTransform>().DOKill();
        MaskLayers[index].sprite = MaskSprites[mask];
        MaskLayers[index].color = new Color(1, 1, 1, 0);
        MaskLayers[index].GetComponent<RectTransform>().anchoredPosition = Vector2.up * 20;

        MaskLayers[index].DOColor(Color.white,.5f);
        MaskLayers[index].GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, .5f);


        AbilityImages[mask].DOKill();
        AbilityImages[mask].DOColor(Color.white, .5f);

    }

}
