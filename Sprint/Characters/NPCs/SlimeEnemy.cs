using Sprint.Interfaces;
using Microsoft.Xna.Framework;
using Sprint.Projectile;
using Sprint.Sprite;
using Sprint.Levels;

namespace Sprint.Characters
{

    //Code based on the BluebubbleEnemy.cs file
    internal class SlimeEnemy : Enemy
    {
        private MoveSlime moveSlime;

        private Timer timeAttack;


        public SlimeEnemy(ISprite sprite, ISprite damagedSprite, Vector2 initialPosition, Room room, SpriteLoader spriteLoader, Player player)
            : base(sprite, damagedSprite, initialPosition, room)
        {

            timeAttack = new Timer(2);
            timeAttack.Start();

            health = CharacterConstants.LOW_HP;

            moveSlime = new MoveSlime(physics, player);
        }

        // Update logic
        public override void Update(GameTime gameTime)
        {

            moveSlime.MoveAI(gameTime);

            base.Update(gameTime);

        }

    }
}
