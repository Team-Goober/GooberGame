using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint.Interfaces;
using System.Collections.Generic;

namespace Sprint.Input
{
    internal class InputTable : IInputMap
    {

        private Dictionary<IInput, ICommand> inputMapping;

        public InputTable()
        {
            // Create input map table
            inputMapping = new Dictionary<IInput, ICommand>();
        }

        public void RegisterMapping(IInput input, ICommand command)
        {
            // Add input to command pairing to table
            inputMapping[input] = command;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            // Check all input triggers
            foreach ((IInput input, ICommand command) in inputMapping)
            {
                // Update state for input trigger
                input.UpdateInput(gameTime, keyState, mouseState);
                // Exxecute command if requirements met
                if (input.IsSatisfied())
                    command.Execute();
            }
        }
    }
}
