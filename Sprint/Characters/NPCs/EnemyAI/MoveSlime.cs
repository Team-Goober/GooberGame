using Microsoft.Xna.Framework;
using System;

namespace Sprint.Characters
{
    internal class MoveSlime : EnemyAI
    {
        private float elapsedTime;
        public Vector2 moveDirection; // Movement direction for the random pattern
        public Vector2 directionFace;
        CalculateDistance calcDistance;
        private Timer waitTime;
        bool waitTF = false;

        Physics physics;

        Player player;


        public MoveSlime(Physics physics, Player player)
        {

            this.physics = physics;
            this.player = player;
            waitTime = new Timer(1);
            





            // Initialize the move direction randomly
            RandomizeMoveDirection();
        }




        // Move AI for MoveVert
        public override void MoveAI(GameTime gameTime)
        {
            float speed = 100; // Adjust the speed as needed
            float moveTime = (float)1; // Time before changing direction (in seconds)
            float waitTime = 0.5f;

            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (waitTF)
            {
                if(elapsedTime > waitTime)
                {
                    RandomizeMoveDirection();
                    waitTF = false;
                    elapsedTime = 0;
                }
            }
            else
            {
                if(elapsedTime > moveTime) {

                    waitTF = true;
                    elapsedTime = 0;
                }
                else
                {
                    Vector2 newPosition = physics.Position + moveDirection * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    physics.SetPosition(newPosition);
                }
            }



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

            if (calcDistance != null)
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
