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
    internal class CalculateDistance
    {
        private Player player;
        private Physics playerPhysics;
        private Physics physics;
        private Vector2 enemyVector;
        private Vector2 playerVector;
        private Random random;

        public CalculateDistance(Physics enemyPhysics, Player player)
        {
           
            this.physics = enemyPhysics;
            this.player = player;
            this.playerPhysics = player.GetPhysic();
            this.playerVector = playerPhysics.Position;
            this.enemyVector = enemyPhysics.Position;
            this.random = new Random();



        }


        public Vector2 FindDirection()
        {
            Vector2 disHor;


            if(enemyVector.X > playerVector.X)
            {
                disHor = Directions.LEFT;
            }else 
            {
                disHor = Directions.RIGHT;
            }


            Vector2 disVert;

            if (enemyVector.Y > playerVector.Y)
            {
                disVert = Directions.UP;
            }
            else
            {
                disVert = Directions.DOWN;
            }

            Vector2[] dirIndices = new Vector2[] { disHor, disVert };

            int randInd = random.Next(dirIndices.Length);
            return dirIndices[randInd];


        }


        public bool proxiDetection()
        {
            float XBound = 30;
            float YBound = 30;

            float XLocation = Math.Abs(enemyVector.X - playerVector.X);
            float YLocation = Math.Abs(enemyVector.Y - playerVector.Y);

            if(XBound > XLocation || YBound > YLocation)
            {
                return true;
            }
            else
            {
                return false;
            }

            

        }






        




    }
}
