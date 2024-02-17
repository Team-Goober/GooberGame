// CycleEnemy.cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Sprite;
using System.Collections.Generic;
using Sprint.Projectile;
using Sprint.Characters;

namespace Sprint
{
    internal class CycleEnemy
    {
        private List<IGameObject> enemies = new List<IGameObject>();
        private int currentEnemyIndex = 0;
        private Vector2 position;

        // Constructor
        public CycleEnemy(Goober game, Vector2 pos, GameObjectManager objectManager)
        {
            this.position = pos;

            // Load textures and set up animations for enemies
            // Add enemies to the 'enemies' list
            enemies.Add(JellyfishEnemy.CreateJellyfishEnemy(game, position));
            enemies.Add(BluebubbleEnemy.CreateBluebubbleEnemy(game, position, objectManager));
            enemies.Add(SkeletonEnemy.CreateSkeletonEnemy(game, position, objectManager));

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
