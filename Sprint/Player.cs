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

        private const float speed = 200;

        private Timer attackTimer;
        private Timer damageTimer;

        // TODO: replace this with state machine
        // Animation to return to as base after a played animation ends
        private enum AnimationCycle 
        {
            Idle,
            Walk
        }
        private AnimationCycle baseAnim;


        //declares the move systems for the main character sprite
        public Player(Game1 game, Vector2 pos, IInputMap inputTable, GameObjectManager objManager)
        {

            physics = new Physics(game, pos);

            //Loads sprite sheet for link
            Texture2D zeldaSheet = game.Content.Load<Texture2D>("zelda_links");
            sprite = new AnimatedSprite(zeldaSheet);

            // Duration of one sword swing or item use
            attackTimer = new Timer(0.5);
            // Duration of the damage state
            damageTimer = new Timer(0.3);

            //declares autoatlas on the location of animation down
            //0, 0, 16, 47 is the coordinates of the x, y for down animation in sprite sheet
            //Important: the second part 16, 47 is the width and height of sprite. For instance is the sprite size is 
            //22x47, then you have to state that in the second part of the rectangle
            //the next 2, 1 is the rows and cols of the sprites
            //the next 2 is the padding between sprites
            //true (boolean) is whether animation should loop
            //5 is the speed 
            Vector2 centerOffset = new Vector2(8, 8);

            IAtlas downAtlas = new AutoAtlas(new Rectangle(0,0,16,46), 2, 1, 14, centerOffset, true, 5);
            sprite.RegisterAnimation("down", downAtlas);

            IAtlas leftAtlas = new AutoAtlas(new Rectangle(30,0,16,46), 2, 1, 14, centerOffset, true, 5);
            sprite.RegisterAnimation("left", leftAtlas);

            IAtlas rightAtlas = new AutoAtlas(new Rectangle(90, 0, 16, 46), 2, 1, 14, centerOffset, true, 5);
            sprite.RegisterAnimation("right", rightAtlas);

            IAtlas upAtlas = new AutoAtlas(new Rectangle(60, 0, 16, 46), 2, 1, 14, centerOffset, true, 5);
            sprite.RegisterAnimation("up", upAtlas);

            IAtlas stillAtlas = new SingleAtlas(new Rectangle(0, 0, 16, 16), centerOffset);  
            sprite.RegisterAnimation("still", stillAtlas);

            IAtlas downStill = new SingleAtlas(new Rectangle(0, 0, 16, 16), centerOffset);
            sprite.RegisterAnimation("downStill", downStill);

            IAtlas leftStill = new SingleAtlas(new Rectangle(30, 0, 16, 16), centerOffset);
            sprite.RegisterAnimation("leftStill", leftStill);

            IAtlas upStill = new SingleAtlas(new Rectangle(60, 0, 16, 16), centerOffset);
            sprite.RegisterAnimation("upStill", upStill);

            IAtlas rightStill = new SingleAtlas(new Rectangle(90, 0, 16, 16), centerOffset);   
            sprite.RegisterAnimation("rightStill", rightStill);

            sprite.SetAnimation("still");
            Facing = Directions.STILL;
            sprite.SetScale(3);

            //Set up damage atlas
            IAtlas damage = new SingleAtlas(new Rectangle(0, 150, 16, 16), centerOffset);
            sprite.RegisterAnimation("damage", damage);

            // sword animations RIGHT 
            IAtlas swordRightAtlas = new SingleAtlas(new Rectangle(84, 90, 27, 15), new Vector2(9, 7));
            sprite.RegisterAnimation("swordRight", swordRightAtlas);

            // sword animations LEFT 
            IAtlas swordLeftAtlas = new SingleAtlas(new Rectangle(24, 90, 27, 15), new Vector2(18, 7));
            sprite.RegisterAnimation("swordLeft", swordLeftAtlas);

            // sword animations UP 
            IAtlas swordUpAtlas = new SingleAtlas(new Rectangle(60, 84, 16, 28), new Vector2(8, 21));
            sprite.RegisterAnimation("swordUp", swordUpAtlas);

            // sword animations DOWN 
            IAtlas swordDownAtlas = new SingleAtlas(new Rectangle(0, 84, 16, 27), new Vector2(8, 7));
            sprite.RegisterAnimation("swordDown", swordDownAtlas);

            // Start out idle
            baseAnim = AnimationCycle.Idle;


            // Set up projectiles
            secondaryItems = new ProjectileSystem(physics.Position, inputTable, objManager, game.Content);


        }

        //Melee attack according to direction
        public void Attack()
        {

            // Only attack if not already attacking
            if (!attackTimer.Ended)
            {
                return;
            }

            // Player shouldn't move while attacking
            StopMoving();

            // Start timer for attack
            attackTimer.Start();

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

        public void StopMoving()
        {
            physics.SetVelocity(new Vector2(0, 0));
/*            // TODO: replace these checks once we have a state machine
            string currAnim = sprite.GetCurrentAnimation();

            // If the current animation is movement, stop it
            if (currAnim.Equals("left") || currAnim.Equals("right") || currAnim.Equals("up") || currAnim.Equals("down"))
            {
                animateStill();
            }*/
            baseAnim = AnimationCycle.Idle;
        }

        // Return to base animation cycle based on states and facing dir
        // TODO: replace with a state machine
        private void returnToBaseAnim()
        {
            if (baseAnim == AnimationCycle.Idle)
            {
                if (Facing == Directions.DOWN)
                {
                    sprite.SetAnimation("downStill");
                }
                else if (Facing == Directions.LEFT)
                {
                    sprite.SetAnimation("leftStill");
                }
                else if (Facing == Directions.UP)
                {
                    sprite.SetAnimation("upStill");
                }
                else if (Facing == Directions.RIGHT)
                {
                    sprite.SetAnimation("rightStill");
                }
            }
            else if (baseAnim == AnimationCycle.Walk)
            {
                if (Facing == Directions.DOWN)
                {
                    sprite.SetAnimation("down");
                }
                else if (Facing == Directions.LEFT)
                {
                    sprite.SetAnimation("left");
                }
                else if (Facing == Directions.UP)
                {
                    sprite.SetAnimation("up");
                }
                else if (Facing == Directions.RIGHT)
                {
                    sprite.SetAnimation("right");
                }
            }

        }

        public void MoveLeft()
        {
            // Sets velocity towards left
            physics.SetVelocity(new Vector2(-speed, 0));

            sprite.SetAnimation("left");
            Facing = Directions.LEFT;
            baseAnim = AnimationCycle.Walk;
        }

        public void MoveRight()
        {
            // Sets velocity towards right
            physics.SetVelocity(new Vector2(speed, 0));

            sprite.SetAnimation("right");
             Facing = Directions.RIGHT;
            baseAnim = AnimationCycle.Walk;
        }

        public void MoveUp()
        {
            // Sets velocity towards up
            physics.SetVelocity(new Vector2(0, -speed));

            sprite.SetAnimation("up");
            Facing = Directions.UP;
            baseAnim = AnimationCycle.Walk;
        }

        public void MoveDown()
        {
            // Sets velocity towards down
            physics.SetVelocity(new Vector2(0, speed));

            sprite.SetAnimation("down");
            Facing = Directions.DOWN;
            baseAnim = AnimationCycle.Walk;
        }

        public Physics GetPhysic()
        {
            return physics;
        }

        public void TakeDamage()
        {
            
            sprite.SetAnimation("damage");
            damageTimer.Start();

        }


        public override void Update(GameTime gameTime)
        {

            // Check for end of sword swing
            attackTimer.Update(gameTime);
            if (attackTimer.JustEnded)
            {
                returnToBaseAnim();
            }

            secondaryItems.UpdateDirection(Facing);
            secondaryItems.UpdatePostion(physics.Position);

            // Checks for damage state
            damageTimer.Update(gameTime);
            if (damageTimer.JustEnded)
            {
                returnToBaseAnim();
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

