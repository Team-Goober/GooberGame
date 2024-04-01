using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint.Functions;
using Sprint.Input;
using Sprint.Interfaces;
using Sprint.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Sprint.GameStates
{
    internal class InventoryState : IGameState
    {

        private Goober game;
        private IInputMap input;

        public InventoryState(Goober game)
        {
            this.game = game;

            
        }

        public List<SceneObjectManager> AllObjectManagers()
        {
            return new List<SceneObjectManager>();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //
        }

        public void MakeCommands()
        {
            input = new InputTable();
            // Register command to return to game
            input.RegisterMapping(new SingleKeyPressTrigger(Keys.I), new ScrollStatesCommand(game, this, game.DungeonState, Directions.DOWN));
        }

        public void PassToState(IGameState newState)
        {
            game.GameState = newState;
            input.Sleep();
        }

        public void Update(GameTime gameTime)
        {
            // Update commands
            input.Update(gameTime);
        }
    }
}
