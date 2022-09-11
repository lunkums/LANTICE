using Lunkums.Controller;
using Lunkums.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private RayGun rayGun;
    [SerializeField] private FirstPersonView firstPersonView;
    [SerializeField] private PointRenderer pointRenderer;

    private KeyboardController keyboardController;
    private MouseController mouseController;

    // Should be 1 or -1
    private int scrollInvert = -1;

    private Vector2 MouseMoveDelta => new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
    private float MouseScrollDelta => Input.GetAxisRaw("Mouse ScrollWheel") * scrollInvert;
    public bool InvertScrolling { get => scrollInvert.IsPositive(); set => scrollInvert = value ? 1 : -1; }

    private void Start()
    {
        SetKeyboardPlayState();
        SetMousePlayState();
    }

    private void Update()
    {
        keyboardController.Update();
        mouseController.Update(MouseMoveDelta, MouseScrollDelta);
    }

    private void Pause()
    {
        if (Menu.Instance.Pause())
        {
            SetKeyboardPauseState();
            SetMousePauseState();
        }
        else
        {
            SetKeyboardPlayState();
            SetMousePlayState();
        }
    }

    private void SetKeyboardPlayState()
    {
        keyboardController = new KeyboardController(
            new Dictionary<KeyCode, Action>(){
                { KeyCode.Space, movement.Jump },
                { KeyCode.C, pointRenderer.ClearAllPoints },
                { KeyCode.Escape, Pause },
                { KeyCode.F12, Menu.Instance.ToggleDebug },
            },
            new Dictionary<KeyCode, Action>() {
                { KeyCode.Mouse0, () => rayGun.Scanning = true },
            },
            new Dictionary<KeyCode, Action>() {
                { KeyCode.W, () => movement.ForwardMotion += 1 },
                { KeyCode.A, () => movement.SidewaysMotion -= 1 },
                { KeyCode.S, () => movement.ForwardMotion -= 1 },
                { KeyCode.D, () => movement.SidewaysMotion += 1 },
                { KeyCode.Mouse1, () => rayGun.Painting = true },
            });
    }

    private void SetKeyboardPauseState()
    {
        keyboardController = new KeyboardController(
            new Dictionary<KeyCode, Action>(){
                { KeyCode.Escape, Pause }
            },
            new Dictionary<KeyCode, Action>() { },
            new Dictionary<KeyCode, Action>() { });
    }

    private void SetMousePlayState()
    {
        mouseController = new MouseController(
            new List<Action<Vector2>>() { firstPersonView.AdjustView },
            new List<Action<float>>() { rayGun.AdjustPaintAngle });
    }

    private void SetMousePauseState()
    {
        mouseController = new MouseController();
    }
}
