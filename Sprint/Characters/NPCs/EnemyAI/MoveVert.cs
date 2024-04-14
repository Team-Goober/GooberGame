using Microsoft.Xna.Framework;
using System;

namespace Sprint.Characters
{
    internal class MoveVert : EnemyAI
    {
        private float elapsedTime;
        public Vector2 moveDirection; // Movement direction for the random pattern
        public Vector2 directionFace;
        CalculateDistance calcDistance;

        Physics physics;

        Player player;


        public MoveVert(Physics physics, Player player)
        {

            this.physics = physics;
            this.player = player;
            

            

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

            if (calcDistance.proxiDetection())
            {
                speed = 300;
            }
            else
            {
                speed = 100;
            }

            // Move in the current direction
            Vector2 newPosition = physics.Position + moveDirection * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            physics.SetPosition(newPosition);
        }



        // Choose a random direction to move
        public void RandomizeMoveDirection()
        {

            if (player != null)
            {
                calcDistance = new CalculateDistance(physics, player);
            }

            // Generate a random movement direction
            Random random = new Random();
            int indDir = random.Next(4);
            directionFace = Directions.GetDirectionFromIndex(indDir);

            if(calcDistance != null)
            {
                SetDirection(calcDistance.FindDirection());
            }
            
            

        }



        // Set the direction of the move AI
        public void SetDirection(Vector2 direction)
        {
            moveDirection = direction;

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
