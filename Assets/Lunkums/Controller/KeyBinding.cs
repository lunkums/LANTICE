namespace Lunkums.Controller
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    internal class KeyBinding
    {
        private Dictionary<KeyCode, Action> binding;
        private KeyTrigger keyTrigger;

        public delegate bool KeyTrigger(KeyCode key);

        public KeyBinding(Dictionary<KeyCode, Action> binding, KeyTrigger keyTrigger)
        {
            this.binding = binding;
            this.keyTrigger = keyTrigger;
        }

        public void Rebind(Dictionary<KeyCode, Action> binding)
        {
            this.binding = binding;
        }

        public void Iterate()
        {
            foreach (KeyValuePair<KeyCode, Action> keyBind in binding)
            {
                if (keyTrigger(keyBind.Key))
                {
                    keyBind.Value();
                }
            }
        }
    }
}
