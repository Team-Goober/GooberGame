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
        public Enemy MakeEnemy(String name, Vector2 position, SceneObjectManager scene)
        {
            // Consider storing enemies in file with reflection, and having enemies load their own sprites
            // This would make dealing with enemies much easier


            ISprite enemySprite = spriteLoader.BuildSprite(ANIM_FILE, name);

            switch (name)
            {
                case "jellyfish":
                    return new JellyfishEnemy(enemySprite, position, scene, spriteLoader);
                case "bluebubble":
                    return new BluebubbleEnemy(enemySprite, position, scene, spriteLoader);
                case "skeleton":
                    return new SkeletonEnemy(enemySprite, position, scene, spriteLoader);
                case "dog":
                    return new DogEnemy(enemySprite, position, scene, spriteLoader);
                case "bat":
                    return new BatEnemy(enemySprite, position, scene, spriteLoader);
                case "hand":
                    return new HandEnemy(enemySprite, position, scene, spriteLoader);
                case "dragonmov":
                    return new DragonEnemy(enemySprite, position, scene, spriteLoader);
                case "slime":
                    return new SlimeEnemy(enemySprite, position, scene, spriteLoader);
                case "spike":
                    return new SpikeEnemy(enemySprite, position, scene, spriteLoader);
                case "oldman":
                    return new OldMan(enemySprite, position, scene, spriteLoader);
                default:
                    return null;
            }



        }




    }
}
