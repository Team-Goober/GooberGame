using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint.Functions;
using Sprint.Input;
using Sprint.Interfaces;
using Sprint.Levels;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace Sprint
{
    internal class PauseState : IGameState
    {

        private IInputMap input;
        private IGameState returnState;
        private Goober game;

        public PauseState(Goober game, IGameState next)
        {

            returnState = next;
            this.game = game;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Draw the return-to state
            returnState.Draw(spriteBatch, gameTime);

        }

        public void Update(GameTime gameTime)
        {
            // Update commands
            input.Update(gameTime);
            // Don't update objects
        }

        public void PassToState(IGameState newState, bool reset)
        {
            game.GameState = newState;
            input.Sleep();
        }

        public List<SceneObjectManager> AllObjectManagers()
        {
            return returnState.AllObjectManagers();
        }

        public void MakeCommands()
        {
            input = new InputTable();
            // Register command to return to game
            input.RegisterMapping(new SingleKeyPressTrigger(Keys.Escape), new SwitchStatesCommand(this, returnState));
        }
    }
}
