using Microsoft.Xna.Framework;
using System;

namespace Sprint.Characters
{
    internal class EnemyFactory
    {


        private Vector2 position;
        private GameObjectManager objectManager;

        public EnemyFactory(GameObjectManager objectManager, SpriteLoader spriteLoader)
        {
            this.position = pos;
            this.objectManager = objectManager;
        }


        /// <summary>
        /// Builds enemy from string name
        /// </summary>
        /// <param name="name">Name of enemy to make</param>
        /// <param name="position">World position to spawn at</param>
        /// <returns></returns>
        public Enemy MakeEnemy(Goober game, String name, Vector2 position)
        {
            // TODO: Implement this function
            // Consider storing enemies in file with reflection, and having enemies load their own sprites
            // This would make dealing with enemies much easier
            this.position = pos;
            this.objectManager = objectManager;


            ISprite enemySprite = spriteLoader.BuildSprite(ANIM_FILE, makeStr);

            switch (name)
            {
                case "jellyfish":
                    return new JellyfishEnemy(game, enemySprite, position, objectManager, spriteLoader);
                case "bluebubble":
                    return new BluebubbleEnemy(game, enemySprite, position, objectManager, spriteLoader);
                case "skeleton":
                    return new SkeletonEnemy(game, enemySprite, position, objectManager, spriteLoader);
                case "dog":
                    return new DogEnemy(game,enemySprite, position, objectManager, spriteLoader);
                case "bat":
                    return new BatEnemy(game, enemySprite, position, objectManager, spriteLoader);
                case "hand":
                    return new HandEnemy(game, enemySprite, position, objectManager, spriteLoader);
                case "dragonmov":
                    return new DragonEnemy(game, enemySprite, position, objectManager, spriteLoader);
            }



        }




    }
}
