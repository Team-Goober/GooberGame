using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Sprint.Input;
using Sprint.Interfaces;
using System.Collections;
using Sprint.Sprite;
using System.Diagnostics;
using Sprint.Commands;

namespace Sprint
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private MoveSystems moveSystems;
        private Texture2D texture;
        private IInputMap inputTable;

        private CycleItem items;

        private SpriteFont font;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {


            inputTable = new InputTable();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            items = new CycleItem(this);

            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.I), new NextItem(items));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.U), new BackItem(items));

            font = Content.Load<SpriteFont>("Font");

            //Uses the ICommand interface (MoveItems.cs) to execute command for the movement of the main character sprite
            moveSystems = new MoveSystems(this);
            inputTable.RegisterMapping(new SingleKeyHoldTrigger(Keys.A), new MoveLeft(moveSystems));
            inputTable.RegisterMapping(new SingleKeyHoldTrigger(Keys.D), new MoveRight(moveSystems));
            inputTable.RegisterMapping(new SingleKeyHoldTrigger(Keys.W), new MoveUp(moveSystems));
            inputTable.RegisterMapping(new SingleKeyHoldTrigger(Keys.S), new MoveDown(moveSystems));


        }

        protected override void Update(GameTime gameTime)
        {
            //Updates main character animation depending on "wasd" keys


            inputTable.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Aquamarine);

            _spriteBatch.Begin();
            moveSystems.Draw(_spriteBatch, gameTime);
            items.Draw(_spriteBatch, gameTime);
            _spriteBatch.DrawString(font, "Credit", new Vector2(10, 300), Color.Black);
            _spriteBatch.DrawString(font, "Program Made By: Bill Yang", new Vector2(10, 330), Color.Black);
            _spriteBatch.DrawString(font, "Sprites from: www.mariomayhem.com/downloads/sprites/super_mario_bros_sprites.php", new Vector2(10, 360), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
