using Lunkums.Controller;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private RayGun rayGun;

    private KeyboardController keyboardController;

    private void Start()
    {
        SetPlayStateController();
    }

    private void Update()
    {
        keyboardController.Update();
    }

    private void Pause()
    {
        if (PauseMenu.Instance.Pause())
        {
            SetPauseStateController();
        }
        else
        {
            SetPlayStateController();
        }
    }

    private void SetPlayStateController()
    {
        keyboardController = new KeyboardController(
            new Dictionary<KeyCode, Action>(){
                { KeyCode.Escape, Pause }
            },
            new Dictionary<KeyCode, Action>() { },
            new Dictionary<KeyCode, Action>() {
                { KeyCode.W, () => movement.ForwardMotion += 1 },
                { KeyCode.A, () => movement.SidewaysMotion -= 1 },
                { KeyCode.S, () => movement.ForwardMotion -= 1 },
                { KeyCode.D, () => movement.SidewaysMotion += 1 },
                { KeyCode.Mouse0, () => rayGun.Painting = true },
                { KeyCode.Mouse1, () => rayGun.Scanning = true },
            });
    }

    private void SetPauseStateController()
    {
        keyboardController = new KeyboardController(
            new Dictionary<KeyCode, Action>(){
                { KeyCode.Escape, Pause }
            },
            new Dictionary<KeyCode, Action>() { },
            new Dictionary<KeyCode, Action>() { });
    }
}
