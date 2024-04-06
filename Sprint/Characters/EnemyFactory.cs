using Microsoft.Xna.Framework;
using System;
using Sprint.Sprite;
using Sprint.Input;
using Sprint.Levels;
using Sprint.Interfaces;
using System.Diagnostics;

namespace Sprint.Characters
{
    internal class EnemyFactory
    {

        private SpriteLoader spriteLoader;
        private const string ANIM_FILE = "enemyAnims";

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
        public Enemy MakeEnemy(String name, Vector2 position, Room room)
        {
            // Consider storing enemies in file with reflection, and having enemies load their own sprites
            // This would make dealing with enemies much easier


            ISprite enemySprite = spriteLoader.BuildSprite(ANIM_FILE, name);

            switch (name)
            {
                case "jellyfish":
                    return new JellyfishEnemy(enemySprite, position, room, spriteLoader);
                case "bluebubble":
                    return new BluebubbleEnemy(enemySprite, position, room, spriteLoader);
                case "skeleton":
                    return new SkeletonEnemy(enemySprite, position, room, spriteLoader);
                case "dog":
                    return new DogEnemy(enemySprite, position, room, spriteLoader);
                case "bat":
                    return new BatEnemy(enemySprite, position, room, spriteLoader);
                case "hand":
                    return new HandEnemy(enemySprite, position, room, spriteLoader);
                case "dragonmov":
                    return new DragonEnemy(enemySprite, position, room, spriteLoader);
                case "slime":
                    return new SlimeEnemy(enemySprite, position, room, spriteLoader);
                case "spike":
                    return new SpikeEnemy(enemySprite, position, room, spriteLoader);
                case "oldman":
                    return new OldMan(enemySprite, position, room, spriteLoader);
                default:
                    return null;
            }



        }




    }
}
