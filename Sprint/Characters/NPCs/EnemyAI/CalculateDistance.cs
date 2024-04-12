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

        public CalculateDistance(Physics enemyPhysics, Player player)
        {

            this.physics = enemyPhysics;
            this.player = player;
            this.playerPhysics = player.GetPhysic();
            this.playerVector = playerPhysics.Position;
            this.enemyVector = enemyPhysics.Position;


        }




        //// Move AI for MoveVert
        //public Vector2 Calculate()
        //{

        //}




    }
}
