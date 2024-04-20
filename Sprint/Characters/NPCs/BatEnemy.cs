using Sprint.Interfaces;
using Microsoft.Xna.Framework;
using Sprint.Projectile;
using Sprint.Sprite;
using Sprint.Levels;
using Sprint.Collision;

namespace Sprint.Characters
{

    //Code based on the BluebubbleEnemy.cs file
    internal class BatEnemy : Enemy
    {
        private Vector2 moveDirection; // Movement direction for the random pattern
        private SimpleProjectileFactory itemFactory;
        private MoveRandom moveRandom;
        

        private Timer timeAttack;

        public override CollisionTypes[] CollisionType => new CollisionTypes[] { CollisionTypes.FLYING, CollisionTypes.ENEMY, CollisionTypes.CHARACTER };

        public BatEnemy(ISprite sprite, ISprite damagedSprite, Vector2 initialPosition, Room room, SpriteLoader spriteLoader)
            : base(sprite, damagedSprite, initialPosition, room)
        {

            health = CharacterConstants.LOW_HP;

            timeAttack = new Timer(2);
            timeAttack.Start();

            itemFactory = new SimpleProjectileFactory(spriteLoader, 30, true, room);


            moveRandom = new MoveRandom(physics);


 
        }

        // Update logic
        public override void Update(GameTime gameTime)
        {

            timeAttack.Update(gameTime);
            base.Update(gameTime);
            //uses timer to shoot arrows ever 3 seconds
            if (timeAttack.JustEnded)
            {
                itemFactory.SetStartPosition(physics.Position);

                itemFactory.SetDirection(moveDirection);

                timeAttack.Start();

            }

            // Move randomly within a specified area
            moveRandom.MoveAI(gameTime);

            // Update the sprite and physics
            sprite.Update(gameTime);
            physics.Update(gameTime);


        }

    }
}
