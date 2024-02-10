using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Sprite;
using System.Collections.Generic;

namespace Sprint
{
    internal class CycleEnemy
    {
        private List<ISprite> enemies = new List<ISprite>();
        private int currentEnemyIndex = 0;

        public CycleEnemy(Game1 game)
        {
            // Load textures and set up animations for enemies
            // Add enemies to the 'enemies' list

            CreateEnemy(game, "zelda_enemies", 0, 0, 16, 16, new Vector2(8, 8), 2);  // Enemy 1
            CreateEnemy(game, "zelda_enemies", 240, 0, 16, 16, new Vector2(8, 8), 2); // Enemy 2
            CreateEnemy(game, "zelda_enemies", 0, 300, 16, 16, new Vector2(8, 8), 2); // Enemy 3

            // Add more enemies as needed
        }

        private void CreateEnemy(Game1 game, string textureName, int x, int y, int width, int height, Vector2 center, int scale)
        {
            Texture2D enemyTexture = game.Content.Load<Texture2D>(textureName);
            ISprite enemySprite = new AnimatedSprite(enemyTexture);
            IAtlas enemyAtlas = new SingleAtlas(new Rectangle(x, y, width, height), center);
            enemySprite.RegisterAnimation("default", enemyAtlas);
            enemySprite.SetAnimation("default");
            enemySprite.SetScale(scale);
            enemies.Add(enemySprite);
        }

        public void NextEnemy()
        {
            currentEnemyIndex = (currentEnemyIndex + 1) % enemies.Count;
        }

        public void PreviousEnemy()
        {
            currentEnemyIndex = (currentEnemyIndex - 1 + enemies.Count) % enemies.Count;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            enemies[currentEnemyIndex].Draw(spriteBatch, position, gameTime);
        }
    }

}
