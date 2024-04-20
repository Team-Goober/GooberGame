using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint.Characters;
using Sprint.Functions.States;
using Sprint.HUD;
using Sprint.Input;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Loader;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Sprint.GameStates
{
    internal class GameOverState : IGameState
    {
        private Goober game;
        private IInputMap input;

        private SceneObjectManager saveHud;
        private SceneObjectManager hudManager;
        private SceneObjectManager menuManager;

        private HUDLoader hudLoader;

        private HUDAnimSprite heartPointer;
        private Vector2 originalPos;


        public GameOverState(Goober game, HUDLoader hudLoader) 
        {
            this.game = game;
            this.hudLoader = hudLoader;

            menuManager = new SceneObjectManager();
            menuManager.Add(hudLoader.MakeDeathMenu());

            this.heartPointer = hudLoader.MakeDeathHeart();
            menuManager.Add(heartPointer);

            originalPos = heartPointer.GetPosition();

            input = new InputTable();
            this.hudManager = new SceneObjectManager();
        }

        public void GetHUDScene(SceneObjectManager scenes)
        {
            //this.hudManager = scenes;

            foreach(IGameObject obj in scenes.GetObjects())
            {
                this.hudManager.Add(obj);
            }

            IHUD over = hudLoader.MakeGameOver();
            hudManager.Add(over);

            System.Timers.Timer timer = new System.Timers.Timer(2000);
            timer.Elapsed += SwitchScene;
            timer.AutoReset = false;
            timer.Start();
        }

        public void MakeCommands()
        {
            input.RegisterMapping(new SingleKeyPressTrigger(Keys.Up),  new MoveHeartUp(this));
            input.RegisterMapping(new SingleKeyPressTrigger(Keys.Down), new MoveHeartDown(this));
            input.RegisterMapping(new SingleKeyPressTrigger(Keys.Enter), new CloseDeathMenu(this));
        }

        public void MoveHeartDown()
        {
            Vector2 position = heartPointer.GetPosition();
            position.Y += 188;

            if(position.Y - originalPos.Y == 188) 
            {
                heartPointer.SetPosition(position);
            }
            
        }

        public void MoveHeartUp()
        {
            Vector2 position = heartPointer.GetPosition();
            position.Y -= 188;

            if(position.Y - originalPos.Y == 0)
            {
                heartPointer.SetPosition(position);
            }
        }

        public void PassToState(IGameState newState)
        {
            game.GameState = newState;
            input.Sleep();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

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
            MakeCommands();

        }

        public void Update(GameTime gameTime)
        {
            input.Update(gameTime);
            hudManager.EndCycle();
            menuManager.EndCycle();
        }

        public void CloseDeathMenu()
        {
            if(originalPos == heartPointer.GetPosition())
            {
                ((DungeonState)game.GetDungeonState()).Continue();

            } else
            {
                ((DungeonState)game.GetDungeonState()).ResetGame();
            }

            PassToState(game.GetDungeonState());
        }
    }
}
