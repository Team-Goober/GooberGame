using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Levels;
using System.Collections.Generic;

namespace Sprint.Interfaces
{
    public interface IGameState
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        // Close out this state and set game to a new state
        void PassToState(IGameState newState);

        // Generate all commands
        void MakeCommands();

        // Return all the object managers of the state
        List<SceneObjectManager> AllObjectManagers();

    }
}
