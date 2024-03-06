using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System.Collections.Generic;
using Sprint.Characters;
using Sprint.Sprite;

namespace Sprint
{
    internal class CycleEnemy
    {
        private List<Enemy> enemies = new List<Enemy>();
        private int currentEnemyIndex;
        private Vector2 position;
        private GameObjectManager objectManager;

        private const string ANIM_FILE = "enemyAnims";
        // Constructor
        public CycleEnemy(Goober game, Vector2 pos, GameObjectManager objectManager, SpriteLoader spriteLoader)
        {
            this.position = pos;
            this.objectManager = objectManager;

            // Load textures and set up animations for enemies
            // Add enemies to the 'enemies' 
            ISprite jellyfishSprite = spriteLoader.BuildSprite(ANIM_FILE, "jellyfish");
            ISprite bluebubbleSprite = spriteLoader.BuildSprite(ANIM_FILE, "bluebubble");
            ISprite skeletonSprite = spriteLoader.BuildSprite(ANIM_FILE, "skeleton");
            ISprite dogSprite = spriteLoader.BuildSprite(ANIM_FILE, "dog");
            ISprite batSprite = spriteLoader.BuildSprite(ANIM_FILE, "bat");
            ISprite handSprite = spriteLoader.BuildSprite(ANIM_FILE, "hand");
            ISprite dragonSprite = spriteLoader.BuildSprite(ANIM_FILE, "dragonmov");
          
            enemies.Add(new JellyfishEnemy(game, jellyfishSprite, position));
            enemies.Add(new BluebubbleEnemy(game, bluebubbleSprite, position, objectManager, spriteLoader));
            enemies.Add(new SkeletonEnemy(game, skeletonSprite, position, objectManager, spriteLoader));
            enemies.Add(new DogEnemy(game, dogSprite, position, objectManager, spriteLoader));
            enemies.Add(new BatEnemy(game, batSprite, position, objectManager, spriteLoader));
            enemies.Add(new HandEnemy(game, handSprite, position, objectManager, spriteLoader));
            enemies.Add(new DragonEnemy(game, dragonSprite, position, objectManager, spriteLoader));

            // Add more enemies as needed

            SwitchEnemy(null, enemies[0]);
        }

        // Switch to the next enemy in the cycle
        public void NextEnemy()
        {
            int before = currentEnemyIndex;
            currentEnemyIndex = (currentEnemyIndex + 1) % enemies.Count;
            SwitchEnemy(enemies[before], enemies[currentEnemyIndex]);
        }

        // Switch to the previous enemy in the cycle
        public void PreviousEnemy()
        {
            int before = currentEnemyIndex;
            currentEnemyIndex = (currentEnemyIndex - 1 + enemies.Count) % enemies.Count;
            SwitchEnemy(enemies[before], enemies[currentEnemyIndex]);
        }

        public void SwitchEnemy(Enemy oldE, Enemy newE)
        {
            if (oldE != null)
                objectManager.Remove(oldE);
            if (newE != null)
                objectManager.Add(newE);
        }

    }
}
