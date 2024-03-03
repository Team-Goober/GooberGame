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
        private List<IGameObject> enemies = new List<IGameObject>();
        private int currentEnemyIndex = 0;
        private Vector2 position;

        private const string ANIM_FILE = "enemyAnims";
        // Constructor
        public CycleEnemy(Goober game, Vector2 pos, GameObjectManager objectManager, SpriteLoader spriteLoader)
        {
            this.position = pos;

            // Load textures and set up animations for enemies
            // Add enemies to the 'enemies' 
            ISprite jellyfishSprite = spriteLoader.BuildSprite(ANIM_FILE, "jellyfish");
            ISprite bluebubbleSprite = spriteLoader.BuildSprite(ANIM_FILE, "bluebubble");
            ISprite skeletonSprite = spriteLoader.BuildSprite(ANIM_FILE, "skeleton");
            ISprite dogSprite = spriteLoader.BuildSprite(ANIM_FILE, "dog");
            ISprite batSprite = spriteLoader.BuildSprite(ANIM_FILE, "bat");
            ISprite handSprite = spriteLoader.BuildSprite(ANIM_FILE, "hand");
            enemies.Add(new JellyfishEnemy(game, jellyfishSprite, position));
            enemies.Add(new BluebubbleEnemy(game, bluebubbleSprite, position, objectManager, spriteLoader));
            enemies.Add(new SkeletonEnemy(game, skeletonSprite, position, objectManager, spriteLoader));
            enemies.Add(new DogEnemy(game, dogSprite, position, objectManager, spriteLoader));
            enemies.Add(new BatEnemy(game, batSprite, position, objectManager, spriteLoader));
            enemies.Add(new HandEnemy(game, handSprite, position, objectManager, spriteLoader));

            // Add more enemies as needed
        }

        // Switch to the next enemy in the cycle
        public void NextEnemy()
        {
            currentEnemyIndex = (currentEnemyIndex + 1) % enemies.Count;
        }

        // Switch to the previous enemy in the cycle
        public void PreviousEnemy()
        {
            currentEnemyIndex = (currentEnemyIndex - 1 + enemies.Count) % enemies.Count;
        }

        // Update the current enemy
        public void Update(GameTime gameTime)
        {
            enemies[currentEnemyIndex].Update(gameTime);
        }

        // Draw the current enemy
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            enemies[currentEnemyIndex].Draw(spriteBatch, gameTime);
        }
    }
}
