using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Sprint.Input;
using Sprint.Interfaces;
using System.Collections;
using Sprint.Sprite;
using System.Diagnostics;
using Sprint.Commands;
using Sprint.Projectile;
using System.Collections.Generic;

namespace Sprint
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player player;

        private IInputMap inputTable;

        private CycleItem items;
        private CycleEnemy enemies;
        private SpriteFont font;
        private Vector2 characterLoc = new Vector2(20, 20);
        private bool resetGame = false;

        private GameObjectManager objectManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            objectManager = new GameObjectManager();
            inputTable = new InputTable();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            items = new CycleItem(this, new Vector2(500, 100));
            enemies = new CycleEnemy(this, new Vector2(500, 300));

            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.I), new NextItem(items));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.U), new BackItem(items));

            font = Content.Load<SpriteFont>("Font");

            //Uses the ICommand interface (MoveItems.cs) to execute command for the movement of the main character sprite
            player = new Player(this, characterLoc, inputTable, objectManager);
            inputTable.RegisterMapping(new SingleKeyHoldTrigger(Keys.A), new MoveLeft(player));
            inputTable.RegisterMapping(new SingleKeyHoldTrigger(Keys.D), new MoveRight(player));
            inputTable.RegisterMapping(new SingleKeyHoldTrigger(Keys.W), new MoveUp(player));
            inputTable.RegisterMapping(new SingleKeyHoldTrigger(Keys.S), new MoveDown(player));

            inputTable.RegisterMapping(new SingleKeyHoldTrigger(Keys.Left), new MoveLeft(player));
            inputTable.RegisterMapping(new SingleKeyHoldTrigger(Keys.Right), new MoveRight(player));
            inputTable.RegisterMapping(new SingleKeyHoldTrigger(Keys.Up), new MoveUp(player));
            inputTable.RegisterMapping(new SingleKeyHoldTrigger(Keys.Down), new MoveDown(player));

            //Enemy cycling
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.O), new PreviousEnemyCommand(enemies));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.P), new NextEnemyCommand(enemies));

            //Quit game
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Q), new Quit(this));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.R), new Reset(this));
        }

        //clears input dictionary and object manager
        public void ResetGame()
        {
            inputTable.ClearDictionary();
            objectManager.ClearObjects();
        }


        //checks if the user requested a reset for game
        public void ResetReq()
        {
            resetGame = true;
        }

        protected override void Update(GameTime gameTime)
        {
            
            //resets the game when user request a reset
            if(resetGame)
            {
                ResetGame();
                LoadContent();
                resetGame=false;
            }


            inputTable.Update(gameTime);
            objectManager.Update(gameTime);

            enemies.Update(gameTime);
            items.Update(gameTime);
            player.Update(gameTime);
            player.UpdateCheckMoving(Keyboard.GetState());
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Aquamarine);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            //Gets the vector coordinates (spriteLocation) from MoveSystems.cs and draws main character sprite
            player.Draw(_spriteBatch, gameTime);
            enemies.Draw(_spriteBatch, gameTime);
            items.Draw(_spriteBatch, gameTime);

            objectManager.Draw(_spriteBatch, gameTime);

            _spriteBatch.DrawString(font, "Credit", new Vector2(10, 300), Color.Black);
            _spriteBatch.DrawString(font, "Program Made By: Bill Yang", new Vector2(10, 330), Color.Black);
            _spriteBatch.DrawString(font, "Sprites from: www.mariomayhem.com/downloads/sprites/super_mario_bros_sprites.php", new Vector2(10, 360), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
