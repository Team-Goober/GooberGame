using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Input
{
    internal class SingleClickTrigger : IInputTrigger
    {
        private MouseButton button; // Which button to check for
        private bool heldPreviously; // Whether last cycle was pressed
        private bool triggered; // Whether all requirements have been satisfied this cycle

        public enum MouseButton
        {
            Left,
            Right,
            Middle
        }

        public SingleClickTrigger(MouseButton button)
        {
            this.button = button;
            this.heldPreviously = false;
            this.triggered = false;
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
            }

            // Trigger only when press is begun
            triggered = pressed && !heldPreviously;

            heldPreviously = pressed;
        }
    }
}
