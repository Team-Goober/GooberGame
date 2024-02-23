using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint.Interfaces;

namespace Sprint.Input
{
    internal class SingleKeyPressTrigger : IInputTrigger
    {

        private Keys key; // Key to check for press
        private bool heldPreviously; // Whether last cycle was pressed
        private bool triggered; // Whether all requirements have been satisfied this cycle

        public SingleKeyPressTrigger(Keys key)
        {
            this.key = key;
            this.heldPreviously = false;
            this.triggered = false;
        }

        public bool IsSatisfied()
        {
            return triggered;
        }

        public void UpdateInput(GameTime gameTime, KeyboardState keys, MouseState mouse)
        {

            bool pressed = keys.IsKeyDown(key);

            // Trigger only when press is begun
            triggered = pressed && !heldPreviously;

            heldPreviously = pressed;
        }
    }
}
