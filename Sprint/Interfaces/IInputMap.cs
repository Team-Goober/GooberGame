using Microsoft.Xna.Framework;

namespace Sprint.Interfaces
{
    internal interface IInputMap
    {
        // Register input trigger to execution of command
        void RegisterMapping(IInputTrigger input, ICommand command);

        // Find active inputs and run corresponding commands each cycle
        void Update(GameTime gameTime);

        //Method that clears the dictionaryr
        void ClearDictionary();

    }
}
