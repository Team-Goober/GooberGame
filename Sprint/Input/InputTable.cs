using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint.Commands;
using Sprint.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sprint.Input
{
    internal class InputTable : IInputMap
    {

        private Dictionary<IInputTrigger, ICommand> inputMapping;

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
                // Exxecute command if requirements met
                if (input.IsSatisfied())
                {
                    command.Execute();
                }
            }
        }

        public void ClearDictionary()
        {
            inputMapping.Clear();
        }
    }
}
