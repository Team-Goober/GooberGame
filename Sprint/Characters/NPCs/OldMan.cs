using Microsoft.Xna.Framework;
using Sprint.Commands.SecondaryItem;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Projectile;
using Sprint.Sprite;
using System;


namespace Sprint.Characters
{
    internal class OldMan : Enemy
    {
        private float elapsedTime;
        private Vector2 initialPosition;
        private Vector2 moveDirection; // Movement direction for the random pattern
        private SimpleProjectileFactory itemFactory;
        private ICommand projectileCommand;

        private Timer timeAttack;


        public OldMan(ISprite sprite, ISprite damagedSprite, Vector2 initialPosition, Room room, SpriteLoader spriteLoader)
            : base(sprite, damagedSprite, initialPosition, room)
        {

            // Store the initial position for reference
            this.initialPosition = initialPosition;

            timeAttack = new Timer(2);
            timeAttack.Start();

            itemFactory = new SimpleProjectileFactory(spriteLoader, 30, true, room);

            projectileCommand = new ShootArrowCommand(itemFactory);


            // Initialize the move direction randomly
            RandomizeMoveDirection();
        }

        // Update logic
        public void Update(GameTime gameTime)
        {


            base.Update(gameTime);
            // Calculate movement based on elapsed time for the random pattern
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move randomly within a specified area
            MoveRandomly(gameTime);

            // Update the sprite and physics
            sprite.Update(gameTime);
            physics.Update(gameTime);


        }

        // Move OldMan randomly within the game area
        private void MoveRandomly(GameTime gameTime)
        {
            float speed = 0; // Adjust the speed as needed
            float moveTime = 2; // Time before changing direction (in seconds)

            if (elapsedTime > moveTime)
            {
                // Change direction after the specified time
                RandomizeMoveDirection();
                elapsedTime = 0;
            }

            // Move in the current direction
            Vector2 newPosition = physics.Position + moveDirection * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;


            physics.SetPosition(newPosition);
        }



        // Generate a random movement direction
        private void RandomizeMoveDirection()
        {
            // Generate a random movement direction
            Random random = new Random();
            float angle = (float)random.NextDouble() * MathHelper.TwoPi;
            moveDirection = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

        }
    }
}
