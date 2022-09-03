using Lunkums.Controller;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;

    private KeyboardController keyboardController;

    private void Awake()
    {
        keyboardController = new KeyboardController();
        keyboardController = new KeyboardController(
            new Dictionary<KeyCode, Action>() { },
            new Dictionary<KeyCode, Action>() { },
            new Dictionary<KeyCode, Action>() {
                { KeyCode.W, () => { movement.ForwardMotion += 1; } },
                { KeyCode.A, () => { movement.SidewaysMotion -= 1; } },
                { KeyCode.S, () => { movement.ForwardMotion -= 1; } },
                { KeyCode.D, () => { movement.SidewaysMotion += 1; } },
            });
    }

    private void Start()
    {
        keyboardController.RebindKeyPressActions(new Dictionary<KeyCode, Action>(){
            { KeyCode.Escape, PauseMenu.Instance.Pause },
        });
    }

    private void Update()
    {
        keyboardController.Update();
    }
}
