using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint.Interfaces;
using System.Collections.Generic;

namespace Sprint.Input
{
    internal class InputTable : IInputMap
    {

        private Dictionary<IInputTrigger, ICommand> inputMapping;
        private bool awake; // Whether the table is being passed updates

        public InputTable()
        {
            // Create input map table
            inputMapping = new Dictionary<IInputTrigger, ICommand>();
        }

        public void RegisterMapping(IInputTrigger input, ICommand command)
        {
            // Add input to command pairing to table
            inputMapping[input] = command;
        }

        public void Update(GameTime gameTime)
        {

            KeyboardState keyState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            // Check all input triggers
            foreach ((IInputTrigger input, ICommand command) in inputMapping)
            {
                // Update state for input trigger
                input.UpdateInput(gameTime, keyState, mouseState);
                // Execute command if requirements met
                // Make sure that first cycle doesn't trigger commands, so rising/falling detectors aren't falsely triggered
                if (awake && input.IsSatisfied())
                {
                    command.Execute();
                }
            }

            awake = true;

        }
    
        // Used to mark that the input table stopped checking for inputs
        public void Sleep()
        {
            awake = false;
        }

        public void ClearDictionary()
        {
            inputMapping.Clear();
        }
    }
}
