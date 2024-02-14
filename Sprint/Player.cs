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
        private bool isDamaged = false;
        private int damageCheck = 0;

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

            IAtlas downStill = new SingleAtlas(new Rectangle(0,0,22,22), new Vector2(0, 0));
            sprite.RegisterAnimation("downStill", downStill);

            IAtlas leftStill = new SingleAtlas(new Rectangle(23, 0, 22, 22), new Vector2(0, 0));
            sprite.RegisterAnimation("leftStill", leftStill);

            IAtlas upStill = new SingleAtlas(new Rectangle(55, 0,22,22),new Vector2(0, 0));
            sprite.RegisterAnimation("upStill", upStill);

            IAtlas rightStill = new SingleAtlas(new Rectangle(88, 0,22,22),new Vector2(0, 0));   
            sprite.RegisterAnimation("rightStill", rightStill);

            sprite.SetAnimation("still");
            Facing = Directions.STILL;
            sprite.SetScale(3);

            //Set up damage atlas
            IAtlas damage = new AutoAtlas(new Rectangle(0, 150, 22, 22), 1, 1, 0, true, 10);
            sprite.RegisterAnimation("damage", damage);

            // sword animations RIGHT 
            IAtlas swordRightAtlas = new SingleAtlas(new Rectangle(84, 90, 27, 15), new Vector2(0, 0));
            sprite.RegisterAnimation("swordRight", swordRightAtlas);

            // sword animations LEFT 
            IAtlas swordLeftAtlas = new SingleAtlas(new Rectangle(24, 90, 27, 15), new Vector2(0, 0));
            sprite.RegisterAnimation("swordLeft", swordLeftAtlas);

            // sword animations UP 
            IAtlas swordUpAtlas = new SingleAtlas(new Rectangle(60, 84, 16, 28), new Vector2(0, 0));
            sprite.RegisterAnimation("swordUp", swordUpAtlas);

            // sword animations DOWN 
            IAtlas swordDownAtlas = new SingleAtlas(new Rectangle(0, 84, 16, 27), new Vector2(0, 0));
            sprite.RegisterAnimation("swordDown", swordDownAtlas);




            // Set up projectiles
            secondaryItems = new ProjectileSystem(physics.Position, inputTable, objManager, game.Content);


        }

        //Melee attack according to direction
        public void Attack()
        {
            switch (Facing)
            {
                case Directions.RIGHT:
                    sprite.SetAnimation("swordRight");
                    break;
                case Directions.LEFT:
                    sprite.SetAnimation("swordLeft");
                    break;
                case Directions.UP:
                    sprite.SetAnimation("swordUp");
                    break;
                case Directions.DOWN:
                    sprite.SetAnimation("swordDown");
                    break;
                default:
                    break;
            }
        }

        public void UpdateCheckMoving(KeyboardState keyState)
        {
            bool checkKey=keyState.IsKeyDown(Keys.W) ||keyState.IsKeyDown(Keys.A)||keyState.IsKeyDown(Keys.S)||keyState.IsKeyDown(Keys.D)||keyState.IsKeyDown(Keys.Left)||keyState.IsKeyDown(Keys.Right)||keyState.IsKeyDown(Keys.Up)||keyState.IsKeyDown(Keys.Down);

            if ( !checkKey )
            {
                if (isDamaged)
                {
                    return;
                } else if (Facing == Directions.DOWN)
                {
                    sprite.SetAnimation("downStill");
                } else if (Facing == Directions.LEFT)
                {
                    sprite.SetAnimation("leftStill");
                } else if (Facing == Directions.UP)
                {
                    sprite.SetAnimation("upStill");
                } else if (Facing == Directions.RIGHT)
                {
                    sprite.SetAnimation("rightStill");
                }

            }
            
        }


        public void MoveLeft()
        {
            //Moves the sprite to the left
            if (isDamaged)
            {
                return;
            }
            physics.Move(new Vector2(-5, 0));
            sprite.SetAnimation("left");
            Facing = Directions.LEFT;
            
        }

        public void MoveRight()
        {
            //Moves the sprite to the right
            if (isDamaged)
            {
                return;
            }
            physics.Move(new Vector2(5, 0));
            sprite.SetAnimation("right");
             Facing = Directions.RIGHT;
        }

        public void MoveUp()
        {
            //Moves the sprite up
            if (isDamaged)
            {
                return;
            }
            physics.Move(new Vector2(0, -5));
            sprite.SetAnimation("up");
            Facing = Directions.UP;
        }

        public void MoveDown()
        {
            //Moves the sprite down
            if (isDamaged)
            {
                return;
            }
            physics.Move(new Vector2(0, 5));
            sprite.SetAnimation("down");
            Facing = Directions.DOWN;
        }

        public Physics GetPhysic()
        {
            return physics;
        }

        public void TakeDamage()
        {
            isDamaged = true;
            
            sprite.SetAnimation("damage");
            
        }


        public override void Update(GameTime gameTime)
        {
            secondaryItems.UpdateDirection(Facing);
            secondaryItems.UpdatePostion(physics.Position);

            //Checks for damage state
            if (isDamaged)
            {
                if(damageCheck == 10)
                {
                    isDamaged = false;
                    damageCheck = 0;
                }
                else
                {
                    damageCheck++;
                }

            }
            
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

