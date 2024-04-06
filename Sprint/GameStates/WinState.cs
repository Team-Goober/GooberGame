using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Characters;
using System.Collections.Generic;
using Sprint.Items;
using Sprint.Sprite;
using System.Diagnostics;
using System.Drawing;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;
using System;


namespace Sprint.GameStates
{
    internal class WinState : IGameState
    {
        private SceneObjectManager roomManager;
        private SceneObjectManager hudManger;
        private SceneObjectManager playerManger;
        private Player player;
        private Item triForce;

        private Vector2 arenaPosition;
        private int currtainAreaX;

        private int currtainPostionX2;
        private int currtainAreaX2;

        public WinState(Game game, SceneObjectManager hudManger, SceneObjectManager roomManger, Player player, SpriteLoader spriteLoader, Vector2 arenaPosition)
        {
            playerManger = new SceneObjectManager();
            this.roomManager = roomManger;
            this.hudManger = hudManger;
            this.player = player;
            this.arenaPosition = arenaPosition;

            player.WinPose();
            Vector2 position = player.GetPhysic().Position;
            
            ItemFactory itemFactory = new ItemFactory(spriteLoader);

            player.GetPhysic().SetPosition(position);

            position.Y -= 50;
            triForce = itemFactory.MakeItem("triforce", position);
            
            playerManger.Add(player);
            playerManger.Add(triForce);

            currtainAreaX = 0;

            currtainPostionX2 = 1024;
            currtainAreaX2 = 0;
        }

        public List<SceneObjectManager> AllObjectManagers()
        {
            return new List<SceneObjectManager>();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach (IGameObject g in hudManger.GetObjects())
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

            foreach (IGameObject g in playerManger.GetObjects())
            {
                g.Draw(spriteBatch, gameTime);
            }

            spriteBatch.End();
        }

        public void MakeCommands()
        {
            throw new NotImplementedException();
        }

        public void PassToState(IGameState newState)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            if(currtainAreaX != currtainPostionX2)
            {
                currtainAreaX += 10;

                currtainAreaX2 += 10;
                currtainPostionX2 -= 10;
            }

            triForce.Update(gameTime);
            hudManger.EndCycle();
            playerManger.EndCycle();
        }
    }
}
