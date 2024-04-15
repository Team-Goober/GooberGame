using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint.Interfaces;
using System.Diagnostics;

namespace Sprint.Input
{
    internal class MultipleKeyPressTrigger : IInputTrigger
    {
        private Keys[] keys; // Keys to check for press
        private bool[] heldPreviously; // Whether each key was held previously
        private bool triggered; // Whether all requirements have been satisfied this cycle

        public MultipleKeyPressTrigger(Keys[] keys)
        {
            this.keys = keys;
            this.heldPreviously = new bool[keys.Length];
            this.triggered = false;
        }

        public bool IsSatisfied()
        {
            return triggered;
        }

        public void UpdateInput(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState)
        {
            // Check if all keys are currently pressed
            bool allPressed = true;
            for (int i = 0; i < keys.Length; i++)
            {
                if (!keyboardState.IsKeyDown(keys[i]))
                {
                    allPressed = false;
                    break;
                }
            }

            // Check if all keys were held down continuously since the last update
            bool allHeld = true;
            for (int i = 0; i < keys.Length; i++)
            {
                if (keyboardState.IsKeyDown(keys[i]) && !heldPreviously[i])
                {
                    allHeld = false;
                    break;
                }
            }

            // Trigger only when all keys are pressed and they were all held continuously
            triggered = allPressed && allHeld;

            // Update the status of each key for the next update
            for (int i = 0; i < keys.Length; i++)
            {
                heldPreviously[i] = keyboardState.IsKeyDown(keys[i]);
            }

            // Handle diagonal movement separately
            if (triggered && keys.Length == 2)
            {
                // Check if both keys are arrow keys (for diagonal movement)
                if ((keys[0] == Keys.Left && keys[1] == Keys.Up) ||
                    (keys[0] == Keys.Left && keys[1] == Keys.Down) ||
                    (keys[0] == Keys.Right && keys[1] == Keys.Up) ||
                    (keys[0] == Keys.Right && keys[1] == Keys.Down))
                {
                    triggered = false; // Diagonal movement should not trigger in this case
                }
            }
        }
    }
}
