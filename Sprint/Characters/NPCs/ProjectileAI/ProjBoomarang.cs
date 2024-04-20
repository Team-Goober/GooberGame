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
using Sprint.Door;

namespace Sprint.Characters
{
    internal class ProjBoomarang
    {
        private SimpleProjectileFactory itemFactory;
        private ICommand projectileCommand;
        private Timer timeAttack;
        public bool shootingTF;



        private Vector2 initialPosition;
        
        Physics physics;


        public ProjBoomarang(SpriteLoader spriteLoader, Room room, Vector2 moveDirection)
        {

            timeAttack = new Timer(3);
            timeAttack.Start();
            itemFactory = new SimpleProjectileFactory(spriteLoader, 30, true, room);

            projectileCommand = new ShootBoomerangC(itemFactory);
        }




        // Move AI for MoveVert
        public void Update(GameTime gameTime, Physics physics, Vector2 moveDirection)
        {
            timeAttack.Update(gameTime);
            

            // Uses timer to shoot projectiles every 2 seconds
            if (timeAttack.JustEnded)
            {
                itemFactory.SetStartPosition(physics.Position);
                itemFactory.SetDirection(moveDirection);
                projectileCommand.Execute();
                timeAttack.Start();
                shootingTF = true;

            }
            else
            {
                shootingTF = false;
            }

        }



 
    }
}
