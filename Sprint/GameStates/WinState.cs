using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Characters;
using System.Collections.Generic;
using Sprint.Items;
using Sprint.Sprite;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;
using Microsoft.Xna.Framework.Input;
using Sprint.Input;
using Sprint.Functions.States;
using Sprint.Interfaces.Powerups;


namespace Sprint.GameStates
{
    internal class WinState : IGameState
    {
        private Goober game;

        private IInputMap input;

        private SceneObjectManager roomManager;
        private SceneObjectManager hudManager;
        private Player player;
        private IPowerup triForce;

        private Vector2 arenaPosition;
        private int currtainAreaX;

        private int currtainPostionX2;
        private int currtainAreaX2;

        public WinState(Goober game, SceneObjectManager hudManger, SceneObjectManager roomManger, Player player, SpriteLoader spriteLoader, Vector2 arenaPosition)
        {
            this.game = game;
            this.input = new InputTable();
            this.roomManager = roomManger;
            this.hudManager = hudManger;
            this.arenaPosition = arenaPosition;

            this.player = player;
            triForce = player.GetInventory().GetPowerup("triforce");
           

            currtainAreaX = 0;

            currtainPostionX2 = 1024;
            currtainAreaX2 = 0;

            MakeCommands();
        }

        public List<SceneObjectManager> AllObjectManagers()
        {
            return new List<SceneObjectManager>();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach (IGameObject g in hudManager.GetObjects())
            {
                g.Draw(spriteBatch, gameTime);
            }

            spriteBatch.End();

            Matrix translateMat = Matrix.CreateTranslation(new Vector3(arenaPosition.X, arenaPosition.Y, 0));
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: translateMat);

            foreach (IGameObject g in roomManager.GetObjects())
            {
                g.Draw(spriteBatch, gameTime);
            }

            Texture2D backingColor;
            backingColor = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            backingColor.SetData(new Color[] { Color.Black });
            spriteBatch.Draw(backingColor, new Rectangle(0, 0, currtainAreaX, 698), Color.White);
            spriteBatch.Draw(backingColor, new Rectangle(currtainPostionX2, 0, currtainAreaX2, 698), Color.White);


            player.Draw(spriteBatch, gameTime);
            triForce.Draw(spriteBatch, player.GetPhysic().Position + new Vector2(0, -50), gameTime);

            spriteBatch.End();
        }

        public void MakeCommands()
        {
            input.RegisterMapping(new SingleKeyPressTrigger(Keys.R), new CloseWin(this));
        }

        public void PassToState(IGameState newState)
        {
            game.GameState = newState;
            input.Sleep();
        }

        public void CloseWin()
        {
            ((DungeonState)game.GetDungeonState()).ResetGame();
            PassToState(game.GetDungeonState());
        }


        public void Update(GameTime gameTime)
        {
            if(currtainAreaX != currtainPostionX2)
            {
                currtainAreaX += 10;

                currtainAreaX2 += 10;
                currtainPostionX2 -= 10;
            }
            player.Update(gameTime);
            input.Update(gameTime);
            triForce.Update(gameTime);
            hudManager.EndCycle();
        }
    }
}
