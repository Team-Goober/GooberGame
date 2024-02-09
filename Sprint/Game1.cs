using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Sprint.Input;
using Sprint.Interfaces;
using System.Collections;
using Sprint.Sprite;
using System.Diagnostics;
using Sprint.Commands;
using Sprint.Projectile;

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
        private Player player;
        private CycleItem items;
        private EnemyManager enemyManager;
        private SpriteFont font;
        private Vector2 characterLoc = new Vector2(100, 100);


        private ProjectileSystem ps;

        //list for directions of sprites. directionList[0] is for main character
        private List<string> directionList = new List<string>() {"still" };





        private EntityManager entityManager;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            entityManager = new EntityManager();
            inputTable = new InputTable();
           
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            items = new CycleItem(this);
            enemyManager = new EnemyManager(this);


            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.I), new NextItem(items));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.U), new BackItem(items));

            font = Content.Load<SpriteFont>("Font");

            //Uses the ICommand interface (MoveItems.cs) to execute command for the movement of the main character sprite
            moveSystems = new MoveSystems(this, characterLoc, directionList);
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
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.O), new PreviousEnemyCommand(enemyManager));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.P), new NextEnemyCommand(enemyManager));


            // Shooting projectile
            ps = new ProjectileSystem(Content, entityManager, inputTable, moveSystems);

        }

        protected override void Update(GameTime gameTime)
        {
            //Updates main character animation depending on "wasd" keys


            inputTable.Update(gameTime);
            entityManager.Update(gameTime);
            ps.UpdatePostion();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Aquamarine);

            _spriteBatch.Begin();
            mainCharacter.Draw(_spriteBatch, gameTime, moveSystems.spriteLocation, directionList[0]);
            //Gets the vector coordinates (spriteLocation) from MoveSystems.cs and draws main character sprite
            
            enemyManager.Draw(_spriteBatch, new Vector2(500, 300), gameTime);
            items.Draw(_spriteBatch, gameTime);

            entityManager.Draw(_spriteBatch, gameTime);

            _spriteBatch.DrawString(font, "Credit", new Vector2(10, 300), Color.Black);
            _spriteBatch.DrawString(font, "Program Made By: Bill Yang", new Vector2(10, 330), Color.Black);
            _spriteBatch.DrawString(font, "Sprites from: www.mariomayhem.com/downloads/sprites/super_mario_bros_sprites.php", new Vector2(10, 360), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
