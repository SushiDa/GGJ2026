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

    [SerializeField] internal List<PrimaryColorMask> AvailableColorMasks = new List<PrimaryColorMask>();

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

    internal bool TryAddMask(PrimaryColorMask mask)
    {
        if (!AvailableColorMasks.Contains(mask) || CurrentMaskStack.Contains(mask) || CurrentMaskStack.Count >= 3)
        {
            Debug.Log("Can't add mask " + mask);
            return false;
        }
        CurrentMaskStack.Push(mask);
        CurrentColorMask = ColorMaskCollection.GetColorMask(CurrentMaskStack.ToList());
        Debug.Log("Mask " + mask + " Added. Current Mask = " + CurrentColorMask);
        Notify();

        return true;
    }

    internal bool TryRemoveMask()
    {
        if (CurrentMaskStack.TryPop(out var mask))
        {
            CurrentColorMask = ColorMaskCollection.GetColorMask(CurrentMaskStack.ToList());
            Debug.Log("Mask " + mask + " Removed. Current Mask = " + CurrentColorMask);
            Notify();
            return true;
        }
        return false;
    }

    private void ResetManager()
    {
        CurrentColorMask = ColorMask.MASK_NONE;
        CurrentMaskStack.Clear();
        Notify();
    }

    private void Notify()
    {
        GlobalColorChangedEvent?.Invoke(CurrentColorMask);
    }
}
