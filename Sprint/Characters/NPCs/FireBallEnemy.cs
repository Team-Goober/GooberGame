using Microsoft.Xna.Framework;
using Sprint.Collision;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Sprite;

namespace Sprint.Characters.NPCs
{
    internal class FireBallEnemy : Enemy
    {

        public FireBallEnemy(ISprite sprite, ISprite damagedSprite, Vector2 initialPosition, Room room, SpriteLoader spriteLoader) : base(sprite, damagedSprite, initialPosition, room)
        {
            // NPC
            health = 99999;
        }

    }
}
