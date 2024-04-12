using Microsoft.Xna.Framework;
using Sprint.Sprite;
using Sprint.Levels;
using Sprint.Interfaces;
using Sprint.Characters.NPCs;

namespace Sprint.Characters
{
    internal class EnemyFactory
    {

        private SpriteLoader spriteLoader;
        private const string ANIM_FILE = "enemyAnims";
        private const string DAMAGED_ANIM_FILE = "enemyDamagedAnims";

        public EnemyFactory(SpriteLoader spriteLoader)
        {
            this.spriteLoader = spriteLoader;
        }


        /// <summary>
        /// Builds enemy from string name
        /// </summary>
        /// <param name="name">Name of enemy to make</param>
        /// <param name="position">World position to spawn at</param>
        /// <returns></returns>
        public Enemy MakeEnemy(string name, Vector2 position, Room room, Player player)
        {
            // Consider storing enemies in file with reflection, and having enemies load their own sprites
            // This would make dealing with enemies much easier


            ISprite enemySprite = spriteLoader.BuildSprite(ANIM_FILE, name);
            ISprite damagedSprite = spriteLoader.BuildSprite(DAMAGED_ANIM_FILE, name);

            switch (name)
            {
                case "jellyfish":
                    return new JellyfishEnemy(enemySprite, damagedSprite, position, room, spriteLoader);
                case "bluebubble":
                    return new BluebubbleEnemy(enemySprite, damagedSprite, position, room, spriteLoader);
                case "skeleton":
                    return new SkeletonEnemy(enemySprite, damagedSprite, position, room, spriteLoader, player);
                case "dog":
                    return new DogEnemy(enemySprite, damagedSprite, position, room, spriteLoader);
                case "bat":
                    return new BatEnemy(enemySprite, damagedSprite, position, room, spriteLoader);
                case "hand":
                    return new HandEnemy(enemySprite, damagedSprite, position, room, spriteLoader);
                case "dragonmov":
                    return new DragonEnemy(enemySprite, damagedSprite, position, room, spriteLoader);
                case "slime":
                    return new SlimeEnemy(enemySprite, damagedSprite, position, room, spriteLoader);
                case "spike":
                    return new SpikeEnemy(enemySprite, damagedSprite, position, room, spriteLoader);
                case "oldman":
                    return new OldMan(enemySprite, damagedSprite, position, room, spriteLoader);
                case "fireball":
                    return new FireBallEnemy(enemySprite, damagedSprite, position, room, spriteLoader);
                default:
                    return null;
            }



        }




    }
}
