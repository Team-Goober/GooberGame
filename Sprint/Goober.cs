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
        private Player player;

        private SpriteFont font;
        private Vector2 characterLoc = new Vector2(gameWidth/2, gameHeight/2);
        private bool resetGame = false;

        private IInputMap inputTable;
        private GameObjectManager objectManager;
        private CollisionDetector collisionDetector;
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

            objectManager = new GameObjectManager();
            inputTable = new InputTable();
            collisionDetector = new CollisionDetector();
            spriteLoader = new SpriteLoader(Content);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            LevelLoader loader = new LevelLoader(Content, objectManager, spriteLoader, inputTable);
            loader.LoadLevelXML("LevelOne/Level1");

            font = Content.Load<SpriteFont>("Font");

            // Pass the Goober instance to the Player constructor
            player = new Player(characterLoc, inputTable, objectManager, spriteLoader, this); // 'this' refers to the current instance of Goober

            MakeCommands();

            // Add player as a persistent object
            objectManager.Add(player, true);
        }


        public void MakeCommands()
        {
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.A), new MoveLeft(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D), new MoveRight(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.W), new MoveUp(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.S), new MoveDown(player));

            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Left), new MoveLeft(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Right), new MoveRight(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Up), new MoveUp(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Down), new MoveDown(player));

            Keys[] moveKeys = { Keys.A, Keys.D, Keys.W, Keys.S, Keys.Left, Keys.Right, Keys.Up, Keys.Down };
            inputTable.RegisterMapping(new MultipleKeyReleaseTrigger(moveKeys), new StopMoving(player));

            //Melee Regular Sword Attack
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Z), new Melee(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.N), new Melee(player));

            //Player uses a cast move
            // TODO: shouldnt bind separately from shoot commands
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D1), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D2), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D3), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D4), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D5), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D6), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D7), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D8), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D9), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D0), new Cast(player));


            //Take Damage
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.E), new TakeDamageCommand(player));

            //Quit game
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Q), new Quit(this));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.R), new Reset(this));

            // Switching rooms
            inputTable.RegisterMapping(new SingleClickTrigger(SingleClickTrigger.MouseButton.Right), new NextRoomCommand(objectManager));
            inputTable.RegisterMapping(new SingleClickTrigger(SingleClickTrigger.MouseButton.Left), new PrevRoomCommand(objectManager));
        }


        //clears input dictionary and object manager
        public void ResetGame()
        {
            // delete all game objects
            objectManager.ClearObjects(true);

            objectManager.EndCycle();

            objectManager.ClearRooms();

            inputTable.ClearDictionary();


            // reload the level
            LevelLoader loader = new LevelLoader(Content, objectManager, spriteLoader, inputTable);
            loader.LoadLevelXML("LevelOne/Level1");

            // new player
            player = new Player(characterLoc, inputTable, objectManager, spriteLoader, this);

            // remake commands
            MakeCommands();

            objectManager.Add(player, true);
        }


        //checks if the user requested a reset for game
        public void ResetReq()
        {
            resetGame = true;
        }

        protected override void Update(GameTime gameTime)
        {

            // Check if the user requested a reset for the game
            if (resetGame)
            {
                ResetGame();
                resetGame = false;
            }


            inputTable.Update(gameTime);

            List<IGameObject> objects = objectManager.GetObjects();
            foreach (IGameObject obj in objects)
                obj.Update(gameTime);


            objectManager.EndCycle();

            collisionDetector.Update(gameTime, objectManager.GetMovers(), objectManager.GetStatics());

            objectManager.EndCycle();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            List<IGameObject> objects = objectManager.GetObjects();
            foreach (IGameObject obj in objects)
                obj.Draw(_spriteBatch, gameTime);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
