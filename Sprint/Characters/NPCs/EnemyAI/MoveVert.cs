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
    public class MoveVert : EnemyAI
    {
        private float elapsedTime;
        private Timer timeAttack;
        private Vector2 moveDirection; // Movement direction for the random pattern


        private Vector2 initialPosition;

        Physics physics;

        public MoveVert(Physics physics)
        {

            this.physics = physics;

            // Initialize the move direction randomly
            RandomizeMoveDirection();
        }




        // Move AI for MoveVert
        public override void MoveAI(GameTime gameTime)
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
            physics.SetPosition(newPosition);
        }



        // Choose a random direction to move
        private void RandomizeMoveDirection()
        {
            // Generate a random movement direction
            Random random = new Random();
            int indDir = random.Next(4);
            Directions direction = (Directions)indDir;
            SetDirection(direction);

        }



        // Set the direction and update the animation accordingly
        public void SetDirection(Directions direction)
        {
            switch (direction)
            {
                case Directions.UP:
                    
                    break;
                case Directions.LEFT:
                    
                    break;
                case Directions.DOWN:
                    
                    break;
                case Directions.RIGHT:
                    
                    break;
                default:
                    
                    break;
            }

        }
    }
}
