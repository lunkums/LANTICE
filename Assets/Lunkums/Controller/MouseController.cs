namespace Lunkums.Controller
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class MouseController
    {
        private ICollection<Action<Vector2>> mouseMoveActions;
        private ICollection<Action<float>> mouseScrollActions;

        public MouseController() : this(
            new List<Action<Vector2>>(),
            new List<Action<float>>())
        { }

        public MouseController(
            ICollection<Action<Vector2>> mouseMoveActions,
            ICollection<Action<float>> mouseScrollActions)
        {
            this.mouseMoveActions = mouseMoveActions;
            this.mouseScrollActions = mouseScrollActions;
        }

        public void Update(Vector2 mouseMoveDelta, float mouseScrollDelta)
        {
            foreach (Action<Vector2> mouseAction in mouseMoveActions)
            {
                mouseAction.Invoke(mouseMoveDelta);
            }

            foreach (Action<float> mouseAction in mouseScrollActions)
            {
                mouseAction.Invoke(mouseScrollDelta);
            }
        }

        public void RebindMouseMoveActions(ICollection<Action<Vector2>> mouseMoveActions)
        {
            this.mouseMoveActions = mouseMoveActions;
        }

        public void RebindMouseScrollActions(ICollection<Action<float>> mouseScrollActions)
        {
            this.mouseScrollActions = mouseScrollActions;
        }
    }
}
