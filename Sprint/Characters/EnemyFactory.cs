using Microsoft.Xna.Framework;
using System;
using Sprint.Sprite;
using Sprint.Loader;
using Sprint.Input;
using Sprint.Levels;
using Sprint.Interfaces;

namespace Sprint.Characters
{
    internal class EnemyFactory
    {


        private GameObjectManager objectManager;
        private SpriteLoader spriteLoader;
        private const string ANIM_FILE = "enemyAnims";

        public EnemyFactory(GameObjectManager objectManager, SpriteLoader spriteLoader)
        {
    
            this.objectManager = objectManager;
            this.spriteLoader = spriteLoader;
        }


        /// <summary>
        /// Builds enemy from string name
        /// </summary>
        /// <param name="name">Name of enemy to make</param>
        /// <param name="position">World position to spawn at</param>
        /// <returns></returns>
        public Enemy MakeEnemy(String name, Vector2 position)
        {
            // Consider storing enemies in file with reflection, and having enemies load their own sprites
            // This would make dealing with enemies much easier


            ISprite enemySprite = spriteLoader.BuildSprite(ANIM_FILE, name);

            switch (name)
            {
                case "jellyfish":
                    return new JellyfishEnemy(enemySprite, position, objectManager, spriteLoader);
                case "bluebubble":
                    return new BluebubbleEnemy(enemySprite, position, objectManager, spriteLoader);
                case "skeleton":
                    return new SkeletonEnemy(enemySprite, position, objectManager, spriteLoader);
                case "dog":
                    return new DogEnemy(enemySprite, position, objectManager, spriteLoader);
                case "bat":
                    return new BatEnemy(enemySprite, position, objectManager, spriteLoader);
                case "hand":
                    return new HandEnemy(enemySprite, position, objectManager, spriteLoader);
                case "dragonmov":
                    return new DragonEnemy(enemySprite, position, objectManager, spriteLoader);
                case "slime":
                    return new SlimeEnemy(enemySprite, position, objectManager, spriteLoader);
                default:
                    return null;
            }



        }




    }
}
