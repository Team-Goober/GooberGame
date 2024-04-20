using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Microsoft.Xna.Framework;
using Sprint.Functions.SecondaryItem;
using System;
using Sprint.Projectile;
using Sprint.Sprite;
using Sprint.Levels;
using Sprint.Collision;
using Sprint;

namespace Sprint.Characters
{
    internal class MoveRandom : EnemyAI
    {
        private float elapsedTime;
        private float elapsedTimeCount;
        public Vector2 moveDirection; // Movement direction for the random pattern
        public Vector2 directionFace;
        private CalculateDistance calcDistance;
        private Player player;
        private Timer timeMove;
        private Timer randomTimeMove;
        private Vector2 newPosition;


        Physics physics;

        public MoveRandom(Physics physics, Player player)
        {

            this.physics = physics;
            this.player = player;
            timeMove = new Timer(2 + new Random().NextDouble() * 2);
            randomTimeMove = new Timer(1 + new Random().NextDouble() * 2);

            // Initialize the move direction randomly
            RandomizeMoveDirection();
        }




        // Move AI for MoveVert
        public override void MoveAI(GameTime gameTime)
        {
            float speed = 100; // Adjust the speed as needed

            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTimeCount += (float)gameTime.ElapsedGameTime.TotalSeconds;

            randomTimeMove.Update(gameTime);

            if (randomTimeMove.Ended)
            {
                RandomizeMoveDirection();
                randomTimeMove.Start();
            }

            timeMove.Update(gameTime);

            timeMove.Update(gameTime);

            if (!timeMove.Ended)
            {
                
                // Move in the current direction
                newPosition = physics.Position + calcDistance.FindRandomMove() * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                physics.SetPosition(newPosition);
            }
            else
            {
                timeMove.Start();
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
            float angle = (float)random.NextDouble() * MathHelper.TwoPi;
            moveDirection = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));



            if (calcDistance != null)
            {
                //SetDirection(calcDistance.FindRandomMove());
                SetDirection(moveDirection);
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


