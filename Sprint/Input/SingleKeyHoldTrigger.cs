using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint.Interfaces;

namespace Sprint.Input
{
    internal class SingleKeyHoldTrigger : IInputTrigger
    {

        private Keys key; // Key to check for hold
        private bool triggered; // Whether all requirements have been satisfied this cycle

        public SingleKeyHoldTrigger(Keys key)
        {
            this.key = key;
        }

        public bool IsSatisfied()
        {
            return triggered;
        }

        public void UpdateInput(GameTime gameTime, KeyboardState keys, MouseState mouse)
        {
            // Set activation to whenever key is down
            triggered = keys.IsKeyDown(key);
        }
    }
}
