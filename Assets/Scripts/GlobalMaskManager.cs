using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalMaskManager : MonoBehaviour
{
    internal static Action<ColorMask> GlobalColorChangedEvent;

    internal static ColorMask CurrentColorMask;
    internal static Stack<PrimaryColorMask> CurrentMaskStack = new Stack<PrimaryColorMask>();
    internal static List<PrimaryColorMask> AvailableColorMasks = new List<PrimaryColorMask>();

    private void Awake()
    {
        ResetManager();
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            TryAddMask(PrimaryColorMask.MASK_1);
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            TryAddMask(PrimaryColorMask.MASK_2);
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            TryAddMask(PrimaryColorMask.MASK_3);
        }
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            TryRemoveMask();
        }
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

        return true;
    }

    internal static bool TryRemoveMask()
    {
        if (CurrentMaskStack.TryPop(out var mask))
        {
            CurrentColorMask = ColorMaskCollection.GetColorMask(CurrentMaskStack.ToList());
            SFXPlayer.Play("Calque");
            Notify();
            return true;
        }
        SFXPlayer.Play("No");
        return false;
    }

    internal static void SetAvailableMasks(List<PrimaryColorMask> masks)
    {
        AvailableColorMasks.Clear();
        AvailableColorMasks.AddRange(masks);
    }

    internal static void ResetManager()
    {
        CurrentColorMask = ColorMask.MASK_NONE;
        CurrentMaskStack.Clear();
        Notify();
    }

    private static void Notify()
    {
        GlobalColorChangedEvent?.Invoke(CurrentColorMask);
    }
}
