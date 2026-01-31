using System;
using UnityEngine;

public class PlayerHub : MonoBehaviour
{

    internal Vector2 InputMovement;
    internal Action JumpEvent;
    internal Action DashEvent;
    internal Action RollEvent;
    internal Action RedMaskEvent;
    internal Action BlueMaskEvent;
    internal Action YellowMaskEvent;
    internal Action MaskOffEvent;

    internal bool canMove = true;
    internal bool canJump = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
