using Sprint.Interfaces;
using Microsoft.Xna.Framework;
using Sprint.Projectile;
using Sprint.Sprite;
using Sprint.Levels;

namespace Sprint.Characters
{

    //Code based on the BluebubbleEnemy.cs file
    internal class SpikeEnemy : Enemy
    {

        private Timer timeAttack;

        private MoveSpike moveSpike;




        public SpikeEnemy(ISprite sprite, ISprite damagedSprite, Vector2 initialPosition, Room room, SpriteLoader spriteLoader)
            : base(sprite, damagedSprite, initialPosition, room)
        {

            health = CharacterConstants.MAX_HP;
            timeAttack = new Timer(2);
            timeAttack.Start();

            moveSpike = new MoveSpike(physics);
        }


        // Update logic
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            moveSpike.MoveAI(gameTime);

            // Update the sprite and physics
            sprite.Update(gameTime);
            physics.Update(gameTime);


        }

    }
}
