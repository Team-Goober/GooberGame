using Microsoft.Xna.Framework;
using System;
using Sprint.Sprite;
using Sprint.Loader;
using Sprint.Input;
using Sprint.Levels;
using Sprint.Interfaces;
using System.Diagnostics;
using System.Collections.Generic;

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

        //Dictinaries to replace switch statements
        private Dictionary<string, Enemy> EnemyDict;

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

            //Replaces the switch statements into Dict
            EnemyDict = new Dictionary<string, Enemy>
            {
                { "jellyfish", new JellyfishEnemy(enemySprite, position, objectManager, spriteLoader) },
                { "bluebubble", new BluebubbleEnemy(enemySprite, position, objectManager, spriteLoader) },
                { "skeleton", new SkeletonEnemy(enemySprite, position, objectManager, spriteLoader) },
                { "dog", new DogEnemy(enemySprite, position, objectManager, spriteLoader) },
                { "bat", new BatEnemy(enemySprite, position, objectManager, spriteLoader) },
                { "hand", new HandEnemy(enemySprite, position, objectManager, spriteLoader) },
                { "dragonmov", new DragonEnemy(enemySprite, position, objectManager, spriteLoader) },
                { "slime", new SlimeEnemy(enemySprite, position, objectManager, spriteLoader) },
                { "spike", new SpikeEnemy(enemySprite, position, objectManager, spriteLoader) },
                { "oldman", new OldMan(enemySprite, position, objectManager, spriteLoader) }
            };


            if (EnemyDict.TryGetValue(name, out Enemy returnEnemy))
            {
                return returnEnemy;
            }

            return null;



        }




    }
}
