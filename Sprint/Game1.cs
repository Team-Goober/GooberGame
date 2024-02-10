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
        private MoveSystems moveSystems;
        private MainCharacter mainCharacter;
        private Texture2D texture;
        private IInputMap inputTable;

        private CycleItem items;
        private CycleEnemy enemies;
        private SpriteFont font;
        private Vector2 characterLoc = new Vector2(100, 100);

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

            items = new CycleItem(this);
            enemies = new CycleEnemy(this);

            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.I), new NextItem(items));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.U), new BackItem(items));

            font = Content.Load<SpriteFont>("Font");

            //Uses the ICommand interface (MoveItems.cs) to execute command for the movement of the main character sprite
            moveSystems = new MoveSystems(this, characterLoc);
            mainCharacter= new MainCharacter(this);
            inputTable.RegisterMapping(new SingleKeyHoldTrigger(Keys.A), new MoveLeft(moveSystems));
            inputTable.RegisterMapping(new SingleKeyHoldTrigger(Keys.D), new MoveRight(moveSystems));
            inputTable.RegisterMapping(new SingleKeyHoldTrigger(Keys.W), new MoveUp(moveSystems));
            inputTable.RegisterMapping(new SingleKeyHoldTrigger(Keys.S), new MoveDown(moveSystems));

            inputTable.RegisterMapping(new SingleKeyHoldTrigger(Keys.Left), new MoveLeft(moveSystems));
            inputTable.RegisterMapping(new SingleKeyHoldTrigger(Keys.Right), new MoveRight(moveSystems));
            inputTable.RegisterMapping(new SingleKeyHoldTrigger(Keys.Up), new MoveUp(moveSystems));
            inputTable.RegisterMapping(new SingleKeyHoldTrigger(Keys.Down), new MoveDown(moveSystems));

            //Enemy cycling
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.O), new PreviousEnemyCommand(enemies));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.P), new NextEnemyCommand(enemies));


            // Shooting projectile
            Texture2D itemSheet = Content.Load<Texture2D>("zelda_items");
            ISprite projSprite = new AnimatedSprite(itemSheet);
            IAtlas projAtlas = new SingleAtlas(new Rectangle(0, 45, 16, 5), new Vector2(3, 8));
            projSprite.RegisterAnimation("def", projAtlas);
            projSprite.SetAnimation("def");
            projSprite.SetScale(4);
            IProjectileFactory projFactory = new SimpleProjectileFactory(objectManager, projSprite, 100, new Vector2(300, 300));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D1), new ShootCommand(projFactory));

        }

        protected override void Update(GameTime gameTime)
        {
            //Updates main character animation depending on "wasd" keys


            inputTable.Update(gameTime);
            objectManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Aquamarine);

            _spriteBatch.Begin();

            //Gets the vector coordinates (spriteLocation) from MoveSystems.cs and draws main character sprite
            mainCharacter.Draw(_spriteBatch, gameTime, moveSystems.spriteLocation, moveSystems.directionList[0]);
            enemies.Draw(_spriteBatch, new Vector2(500, 300), gameTime);
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
