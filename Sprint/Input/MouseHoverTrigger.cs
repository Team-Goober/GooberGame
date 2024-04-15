using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint.Interfaces;
using System.Diagnostics;

namespace Sprint.Input
{
    internal class MouseHoverTrigger : IInputTrigger
    {

        Vector2 pos;
        float distance;
        private bool hoveredPreviously;
        bool triggered;

        public MouseHoverTrigger(Vector2 pos, float distance)
        {
            this.pos = pos;
            this.distance = distance;
            hoveredPreviously = false;
            triggered = false;
        }

        public bool IsSatisfied()
        {
            return triggered;
        }

        public void UpdateInput(GameTime gameTime, KeyboardState keys, MouseState mouse)
        {
            // See if mouse position is close enough to target
            bool hover = Vector2.Distance(pos, new Vector2(mouse.Position.X, mouse.Position.Y)) <= distance;
            // Only activate if the mouse just started hovering
            triggered = hover && !hoveredPreviously;
            hoveredPreviously = hover;
        }
    }
}
