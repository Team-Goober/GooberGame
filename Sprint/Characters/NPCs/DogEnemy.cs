using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Microsoft.Xna.Framework;
using Sprint.Commands.SecondaryItem;
using System;
using Sprint.Projectile;
using Sprint.Sprite;
using Sprint.Levels;

namespace Sprint.Characters
{
    public class DogEnemy : Enemy
    {
        private float elapsedTime;
        private Timer timeAttack;
        private Vector2 moveDirection; // Movement direction for the random pattern
        private ICommand projectileCommand;
        private SimpleProjectileFactory itemFactory;
        private Vector2 initialPosition;
        private string lastAnimationName;

        public DogEnemy(ISprite sprite, ISprite damagedSprite, Vector2 initialPosition, SceneObjectManager objectManager, SpriteLoader spriteLoader)
            : base(sprite, initialPosition, objectManager)
        {

            // Store the initial position for reference
            this.initialPosition = initialPosition;

            timeAttack = new Timer(2);
            timeAttack.Start();

            itemFactory = new SimpleProjectileFactory(spriteLoader, 30, true, objectManager);

            projectileCommand = new ShootBombC(itemFactory);

            // Initialize the move direction randomly
            RandomizeMoveDirection();
        }

        // Register a directional animation for DogEnemy sprite
        public void RegisterDirectionalAnimation(string animationLabel, IAtlas atlas)
        {
            sprite.RegisterAnimation(animationLabel, atlas);
        }

        // Set the current animation for DogEnemy sprite
        public void SetAnimation(string animationLabel)
        {
            sprite.SetAnimation(animationLabel);
        }

        // Set the scale of DogEnemy sprite
        public void SetScale(int scale)
        {
            sprite.SetScale(scale);
        }

        // Update DogEnemy logic
        public override void Update(GameTime gameTime)
        {
            timeAttack.Update(gameTime);

            // Uses timer to shoot projectiles every 2 seconds
            if (timeAttack.JustEnded)
            {
                itemFactory.SetStartPosition(physics.Position);
                itemFactory.SetDirection(moveDirection);
                projectileCommand.Execute();
                timeAttack.Start();
            }

            // Calculate movement based on elapsed time for the random pattern
            MoveRandomly(gameTime);

            // Set animation based on the new direction
            SetAnimationBasedOnDirection();

            // Update the sprite and physics
            sprite.Update(gameTime);
            physics.Update(gameTime);
        }

        // Set animation based on the direction of movement
        private void SetAnimationBasedOnDirection()
        {
            string newAnim = "";
            if (Math.Abs(moveDirection.X) > Math.Abs(moveDirection.Y))
            {

                if (moveDirection.X > 0)
                    newAnim = "rightFacing";
                else
                    newAnim = "leftFacing";

            }
            else
            {

                if (moveDirection.Y > 0)
                    newAnim = "upFacing";
                else
                    newAnim = "downFacing";
            }

            if(newAnim != lastAnimationName)
            {
                lastAnimationName = newAnim;
                SetAnimation(newAnim);
            }


        }

        // Move DogEnemy randomly within the game area
        private void MoveRandomly(GameTime gameTime)
        {
            float speed = 50; // Adjust the speed as needed
            float moveTime = 2; // Time before changing direction (in seconds)

            if (elapsedTime > moveTime)
            {
                // Change direction after the specified time
                RandomizeMoveDirection();
                elapsedTime = 0;
            }

            // Move in the current direction
            Vector2 newPosition = physics.Position + moveDirection * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            CheckBounds(newPosition, 3); // Ensure enemy stays within game bounds
            physics.SetPosition(newPosition);
        }

        // Ensure that the enemy always stays within the game bounds
        private void CheckBounds(Vector2 pos, float scale)
        {
            //int gameX = Goober.gameWidth;
            //int gameY = Goober.gameHeight;

            // Make the enemy go to the other direction when it reaches a certain distance so that it doesn't go over the window
            //    if (pos.X + scale > gameX)
            //    {
            //        moveDirection.X = -moveDirection.X;
            //    }

            //    if (pos.Y + scale > gameY)
            //    {
            //        moveDirection.Y = -moveDirection.Y;
            //    }
        }

        // Generate a random movement direction for DogEnemy
        private void RandomizeMoveDirection()
        {
            // Generate a random movement direction
            Random random = new Random();
            float angle = (float)random.NextDouble() * MathHelper.TwoPi;
            moveDirection = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

            // Normalize the moveDirection vector
            moveDirection.Normalize();
        }

    }
}
