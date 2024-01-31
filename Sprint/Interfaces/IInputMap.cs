using Microsoft.Xna.Framework;

namespace Sprint.Interfaces
{
    internal interface IInputMap
    {
        // Register input trigger to execution of command
        void RegisterMapping(IInput input, ICommand command);

        // Find active inputs and run corresponding commands each cycle
        void Update(GameTime gameTime);

    }
}
