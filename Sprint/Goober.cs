using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint.Input;
using Sprint.Interfaces;
using Sprint.Functions;
using Sprint.Sprite;
using Sprint.GameStates;
using Sprint.Music.Sfx;


namespace Sprint
{
    public class Goober : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private IInputMap inputTable; // Table of commands available no matter what state the game is loaded


        public IGameState GameState; // Current state of the game

        private DungeonState dungeonState; // State where player can move in a room
        private InventoryState inventoryState; // State where player can see map and select items
        private SfxFactory sfxFactory;

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

        public static ContentManager content;
        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = gameWidth;
            _graphics.PreferredBackBufferHeight = gameHeight;
            _graphics.ApplyChanges();

            spriteLoader = new SpriteLoader(Content);
            inputTable = new InputTable();
            content = Content;


            base.Initialize();
        }

        protected override void LoadContent()
        {

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            dungeonState = new DungeonState(this, spriteLoader, Content);
            inventoryState = new InventoryState(this);
            dungeonState.MakeCommands();
            inventoryState.MakeCommands();
            GameState = dungeonState;
            sfxFactory = SfxFactory.GetInstance();
            sfxFactory.MakeSongs();

            //Quit game
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Q), new Quit(this));
        }
 

        protected override void Update(GameTime gameTime)
        {
            inputTable.Update(gameTime);
            GameState.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            GameState.Draw(_spriteBatch, gameTime);

            base.Draw(gameTime);
        }

        public IGameState GetDungeonState()
        {
            return dungeonState;
        }

        public IGameState GetInventoryState()
        {
            return inventoryState;
        }

    }
}
