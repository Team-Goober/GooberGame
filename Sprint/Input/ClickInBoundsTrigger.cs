using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint.Interfaces;
using Sprint.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Input
{
    internal class ClickInBoundsTrigger : IInputTrigger
    {
        private MouseButton button; // Which button to check for
        private bool heldPreviously; // Whether last cycle was pressed
        private bool triggered; // Whether all requirements have been satisfied this cycle

        private Rectangle target;

        public enum MouseButton
        {
            Left,
            Right,
            Middle
        }

        public ClickInBoundsTrigger(MouseButton button, Rectangle rect)
        {
            this.button = button;
            this.heldPreviously = false;
            this.triggered = false;
            this.target = rect;
        }

        public bool IsSatisfied()
        {
            return triggered;
        }

        public void UpdateInput(GameTime gameTime, KeyboardState keys, MouseState mouse)
        {
            bool pressed = false;

            switch (button)
            {
                case MouseButton.Left:
                    pressed = mouse.LeftButton == ButtonState.Pressed;
                    break;
                case MouseButton.Right:
                    pressed = mouse.RightButton == ButtonState.Pressed;
                    break;
                case MouseButton.Middle:
                    pressed = mouse.MiddleButton == ButtonState.Pressed;
                    break;
                default:
                    pressed = false;
                    break;
            }

            // Trigger only when press is begun
            if (pressed && !heldPreviously)
            {
                triggered = target.Contains(mouse.X, mouse.Y);
            }
            else
            {
                triggered = false;
            }

            heldPreviously = pressed;

        }
    }
}
