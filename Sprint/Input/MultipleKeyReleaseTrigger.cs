using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint.Interfaces;

namespace Sprint.Input
{
    internal class MultipleKeyReleaseTrigger : IInputTrigger
    {

        private Keys[] keyList; // Keys to check for press
        private bool heldPreviously; // Whether last cycle was pressed
        private bool triggered; // Whether all requirements have been satisfied this cycle

        public MultipleKeyReleaseTrigger(Keys[] keyList)
        {
            this.keyList = keyList;
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
            for (int i=0; i < keyList.Length; i++)
            {
                if ( keys.IsKeyDown(keyList[i]))
                {
                    pressed = true;
                }
            }

            // Trigger only when last remaining presses are ended
            triggered = !pressed && heldPreviously;

            heldPreviously = pressed;
        }
    }
}
