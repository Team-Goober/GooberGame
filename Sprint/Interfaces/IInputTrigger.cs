using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Sprint.Interfaces
{
    internal interface IInputTrigger
    {
        
        // Update internal vars based on controllers
        void UpdateInput(GameTime gameTime, KeyboardState keys, MouseState mouse);

        // Return whether all requirements for this input trigger are met
        bool IsSatisfied();


    }
}
