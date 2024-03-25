using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint.Input;
using Sprint.Interfaces;
using Sprint.Commands;
using Sprint.Sprite;


namespace Sprint
{
    public class Goober : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont font;

        private IInputMap inputTable; // Table of commands available no matter what state the game is loaded


        public IGameState GameState; // Current state of the game

        public IGameState DungeonState; // State where player can move in a room
        public IGameState InventoryState; // State where player can see map and select items
        public IGameState GameOverState; // State where player died and can restart game

        private SpriteLoader spriteLoader; // Loads sprites from file and caches them for reuse
        // Dimensions of window
        public static readonly int gameWidth = 1024;
        public static readonly int gameHeight = 956;

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

            DungeonState = new DungeonState(this, spriteLoader, Content);
            GameState = DungeonState;

            //Quit game
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Q), new Quit(this));

        }
 

        protected override void Update(GameTime gameTime)
        {

            GameState.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            GameState.Draw(_spriteBatch, gameTime);

            base.Draw(gameTime);
        }

    }
}
