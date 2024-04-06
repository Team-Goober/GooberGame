using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.HUD;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Loader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Timers;


namespace Sprint.GameStates
{
    internal class GameOverState : IGameState
    {
        private Goober game;
        private IInputMap input;

        private SceneObjectManager roomManager;
        private SceneObjectManager hudManager;
        private SceneObjectManager menuManager;

        private HUDLoader hudLoader;

        private HUDAnimSprite heartPointer;


        public GameOverState(Goober game, HUDLoader hudLoader) 
        {
            this.game = game;
            this.hudLoader = hudLoader;

            menuManager = new SceneObjectManager();
            menuManager.Add(hudLoader.MakeDeathMenu());

            heartPointer = hudLoader.MakeDeathHeart();
        }

        public void GetRoomScene(SceneObjectManager scenes)
        {
            this.roomManager = scenes;
        }

        public void GetHUDScene(SceneObjectManager scenes)
        {
            this.hudManager = scenes;
            IHUD over = hudLoader.MakeGameOver();
            hudManager.Add(over);

            System.Timers.Timer timer = new System.Timers.Timer(2000);
            timer.Elapsed += SwitchScene;
            timer.AutoReset = false;
            timer.Start();
        }

        public List<SceneObjectManager> AllObjectManagers()
        {
            return new List<SceneObjectManager>();
        }

        public void MakeCommands()
        {
            throw new NotImplementedException();
        }

        public void PassToState(IGameState newState)
        {
            game.GameState = newState;
            input.Sleep();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Define a custom BlendState for blending two colors together
            //BlendState blend = new BlendState();
            //blend.ColorBlendFunction = BlendFunction.Add;
            //blend.ColorSourceBlend = Blend.DestinationColor;
            //blend.ColorDestinationBlend = Blend.Zero;

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach (IGameObject g in hudManager.GetObjects())
            {
                g.Draw(spriteBatch, gameTime);
            }

            spriteBatch.End();
        }

        private void SwitchScene(Object sender, ElapsedEventArgs eventArgs)
        {
            hudManager.ClearObjects();
            hudManager = menuManager;
        }


        public void Update(GameTime gameTime)
        {
            hudManager.EndCycle();
            menuManager.EndCycle();
        }
    }
}
