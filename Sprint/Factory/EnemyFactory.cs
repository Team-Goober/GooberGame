using Microsoft.Xna.Framework;
using System;
using Sprint.Sprite;
using Sprint.Loader;
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
        public Enemy MakeEnemy(String name, Vector2 position, SceneObjectManager scene)
        {
            // Consider storing enemies in file with reflection, and having enemies load their own sprites
            // This would make dealing with enemies much easier


            ISprite enemySprite = spriteLoader.BuildSprite(ANIM_FILE, name);
            ISprite damagedSprite = spriteLoader.BuildSprite(DAMAGED_ANIM_FILE, name);

            switch (name)
            {
                case "jellyfish":
                    return new JellyfishEnemy(enemySprite, damagedSprite, position, scene, spriteLoader);
                case "bluebubble":
                    return new BluebubbleEnemy(enemySprite, damagedSprite, position, scene, spriteLoader);
                case "skeleton":
                    return new SkeletonEnemy(enemySprite, damagedSprite, position, scene, spriteLoader);
                case "dog":
                    return new DogEnemy(enemySprite, damagedSprite, position, scene, spriteLoader);
                case "bat":
                    return new BatEnemy(enemySprite, damagedSprite, position, scene, spriteLoader);
                case "hand":
                    return new HandEnemy(enemySprite, damagedSprite, position, scene, spriteLoader);
                case "dragonmov":
                    return new DragonEnemy(enemySprite, damagedSprite, position, scene, spriteLoader);
                case "slime":
                    return new SlimeEnemy(enemySprite, damagedSprite, position, scene, spriteLoader);
                case "spike":
                    return new SpikeEnemy(enemySprite, damagedSprite, position, scene, spriteLoader);
                case "oldman":
                    return new OldMan(enemySprite, damagedSprite, position, scene, spriteLoader);
                default:
                    return null;
            }



        }




    }
}
