using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Sprint.Input;
using Sprint.Interfaces;
using Sprint.Commands;
using Sprint.Characters;
using System.Collections.Generic;
using Sprint.Collision;
using System.Xml;
using XMLData;
using System.Diagnostics;
using System.Security;
using System.Collections.Generic;
using Sprint.Sprite;
using Sprint.Loader;
using Sprint.Levels;
using Sprint.Functions;
using Sprint.Functions.RoomTransition;

namespace Sprint
{
    public class Goober : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont font;

        private IInputMap inputTable;


        public IGameState gameState;

        public IGameState dungeonState;
        public IGameState inventoryState;
        public IGameState gameOverState;

        private SpriteLoader spriteLoader;
        public static int gameWidth = 1024;
        public static readonly int gameHeight = 700;

        public Goober()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = gameWidth;
            _graphics.PreferredBackBufferHeight = gameHeight;
            _graphics.ApplyChanges();

            spriteLoader = new SpriteLoader(Content);
            inputTable = new InputTable();

            base.Initialize();
        }

        protected override void LoadContent()
        { 

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Font");

            dungeonState = new DungeonState(spriteLoader, Content);
            gameState = dungeonState;


            //Quit game
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Q), new Quit(this));

        }
 

        protected override void Update(GameTime gameTime)
        {
            
            gameState.Update(gameTime);
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            gameState.Draw(_spriteBatch, gameTime);

            base.Draw(gameTime);
        }

    }
}
