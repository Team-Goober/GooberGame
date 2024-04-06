using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Microsoft.Xna.Framework;
using Sprint.Commands.SecondaryItem;
using System;
using Sprint.Projectile;
using Sprint.Sprite;
using Sprint.Levels;
using Sprint.Collision;
using Sprint;

namespace Sprint.Characters
{
    public class MoveHand : EnemyAI
    {
        private float elapsedTime;
        private Timer timeAttack;
        public Vector2 moveDirection; // Movement direction for the random pattern
        public DirectionFace directionFace;



        private Vector2 initialPosition;

        Physics physics;


        public MoveHand(Physics physics)
        {

            this.physics = physics;

            // Initialize the move direction randomly
            RandomizeMoveDirection();
        }




        // Move AI for MoveVert
        public override void MoveAI(GameTime gameTime)
        {
            float speed = 100; // Adjust the speed as needed
            float moveTime = (float)1; // Time before changing direction (in seconds)

            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

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
        public void RandomizeMoveDirection()
        {
            // Generate a random movement direction
            Random random = new Random();
            int indDir = random.Next(4);
            directionFace = (DirectionFace)indDir;
            SetDirection(directionFace);
            

        }



        // Set the direction of the move AI
        public void SetDirection(DirectionFace direction)
        {
            switch (direction)
            {
                case DirectionFace.UP:
                    moveDirection = new Vector2(0, -1);
                    break;
                case DirectionFace.LEFT:
                    moveDirection = new Vector2(-1, 0);
                    break;
                case DirectionFace.DOWN:
                    moveDirection = new Vector2(0, 1);
                    break;
                case DirectionFace.RIGHT:
                    moveDirection = new Vector2(1, 0);
                    break;
                default:
                    moveDirection = Vector2.Zero;
                    break;
            }

            

        }

        //reverses move direction when collides with wall
        public void ReverseHorDir()
        {
            moveDirection.X = -moveDirection.X;
        }

        public void ReverseVerDir()
        {
            moveDirection.Y = -moveDirection.Y;
        }
    }
}
