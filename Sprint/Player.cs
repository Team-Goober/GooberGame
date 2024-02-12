using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Sprint.Interfaces;
using Sprint.Sprite;
using System;
using Sprint.Projectile;

namespace Sprint
{

    internal class Player : Character
    {

        private ISprite sprite;

        private Physics physics;

        private ProjectileSystem secondaryItems;

        public Directions Facing { get; private set; }


        //declares the move systems for the main character sprite
        public Player(Game1 game, Vector2 pos, IInputMap inputTable, GameObjectManager objManager)
        {

            physics = new Physics(game, pos);

            //Loads sprite sheet for link
            Texture2D zeldaSheet = game.Content.Load<Texture2D>("zelda_links");
            sprite = new AnimatedSprite(zeldaSheet);

            //declares autoatlas on the location of animation down
            //0, 0, 16, 47 is the coordinates of the x, y for down animation in sprite sheet
            //Important: the second part 16, 47 is the width and height of sprite. For instance is the sprite size is 
            //22x47, then you have to state that in the second part of the rectangle
            //the next 2, 1 is the rows and cols of the sprites
            //the next 2 is the padding between sprites
            //true (boolean) is whether animation should loop
            //5 is the speed 
            IAtlas downAtlas = new AutoAtlas(new Rectangle(0,0,22,47), 2, 1, 2, true, 5);
            sprite.RegisterAnimation("down", downAtlas);


            IAtlas leftAtlas = new AutoAtlas(new Rectangle(23,0,22,48), 2, 1, 2, true, 5);
            sprite.RegisterAnimation("left", leftAtlas);


            IAtlas rightAtlas = new AutoAtlas(new Rectangle(88, 0, 22, 47), 2, 1, 2, true,5);
            sprite.RegisterAnimation("right", rightAtlas);

            IAtlas upAtlas = new AutoAtlas(new Rectangle(55, 0,22,47), 2, 1, 2, true, 5);
            sprite.RegisterAnimation("up", upAtlas);

            IAtlas stillAtlas = new SingleAtlas(new Rectangle(0, 0, 22, 22), new Vector2(0,0));  
            sprite.RegisterAnimation("still", stillAtlas);


            sprite.SetAnimation("still");
            Facing = Directions.STILL;
            sprite.SetScale(3);


            // Set up projectiles
            secondaryItems = new ProjectileSystem(physics.Position, inputTable, objManager, game.Content);


        }

        public void UpdateCheckMoving(KeyboardState keyState)
        {
            bool checkKey=keyState.IsKeyDown(Keys.W) ||keyState.IsKeyDown(Keys.A)||keyState.IsKeyDown(Keys.S)||keyState.IsKeyDown(Keys.D);

            if(!checkKey)
            {
                sprite.SetAnimation("still");
                Facing = Directions.STILL;
            }
        }


        public void MoveLeft()
        {
            //Moves the sprite to the left
            physics.Move(new Vector2(-5, 0));
            sprite.SetAnimation("left");
            Facing = Directions.LEFT;
        }

        public void MoveRight()
        {
            //Moves the sprite to the right
            physics.Move(new Vector2(5, 0));
            sprite.SetAnimation("right");
            Facing = Directions.RIGHT;
        }

        public void MoveUp()
        {
            //Moves the sprite up
            physics.Move(new Vector2(0, -5));
            sprite.SetAnimation("up");
            Facing = Directions.UP;
        }

        public void MoveDown()
        {
            //Moves the sprite down
            physics.Move(new Vector2(0, 5));
            sprite.SetAnimation("down");
            Facing = Directions.DOWN;
        }

        public Physics GetPhysic()
        {
            return physics;
        }


        public override void Update(GameTime gameTime)
        {
            secondaryItems.UpdateDirection(Facing);
            secondaryItems.UpdatePostion(physics.Position);

            physics.Update(gameTime);
            sprite.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //Draws sprite animation using AnimationSprite class
            sprite.Draw(spriteBatch, physics.Position, gameTime);
            
        }

    }
}

