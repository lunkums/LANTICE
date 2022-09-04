namespace Lunkums.Controller
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class KeyboardController : IController
    {
        private KeyBinding keyPressBindings;
        private KeyBinding keyReleaseBindings;
        private KeyBinding keyHeldBindings;

        public KeyboardController() : this(new Dictionary<KeyCode, Action>(),
            new Dictionary<KeyCode, Action>(),
            new Dictionary<KeyCode, Action>()){}

        public KeyboardController(Dictionary<KeyCode, Action> keyPressActions,
            Dictionary<KeyCode, Action> keyReleaseActions,
            Dictionary<KeyCode, Action> keyHeldActions)
        {
            keyPressBindings = new KeyBinding(keyPressActions, Input.GetKeyDown);
            keyReleaseBindings = new KeyBinding(keyReleaseActions, Input.GetKeyUp);
            keyHeldBindings = new KeyBinding(keyHeldActions, Input.GetKey);
        }

        public void Update()
        {
            keyPressBindings.Iterate();
            keyReleaseBindings.Iterate();
            keyHeldBindings.Iterate();
        }

        public void RebindKeyPressActions(Dictionary<KeyCode, Action> bindings)
        {
            keyPressBindings.Rebind(bindings);
        }

        public void RebindKeyReleaseActions(Dictionary<KeyCode, Action> bindings)
        {
            keyReleaseBindings.Rebind(bindings);
        }

        public void RebindKeyHeldActions(Dictionary<KeyCode, Action> bindings)
        {
            keyHeldBindings.Rebind(bindings);
        }
    }
}
