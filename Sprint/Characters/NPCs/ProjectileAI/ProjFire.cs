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
using Sprint.Door;

namespace Sprint.Characters
{
    internal class ProjFire
    {
        private SimpleProjectileFactory itemFactory;
        private ICommand projectileCommand;
        private Timer timeAttack;
        public bool shootingTF;



        private Vector2 initialPosition;
        
        Physics physics;


        public ProjFire(SpriteLoader spriteLoader, Room room, Vector2 moveDirection)
        {

            timeAttack = new Timer(2);
            timeAttack.Start();
            itemFactory = new SimpleProjectileFactory(spriteLoader, 30, true, room);

            projectileCommand = new ShootFireBallC(itemFactory);
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
