using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System.Collections.Generic;
using Sprint.Characters;

namespace Sprint
{
    internal class CycleEnemy
    {
        private List<Enemy> enemies = new List<Enemy>();
        private int currentEnemyIndex;
        private Vector2 position;
        private GameObjectManager objManager;

        // Constructor
        public CycleEnemy(Goober game, Vector2 pos, GameObjectManager objManager)
        {
            this.position = pos;
            this.objManager = objManager;

            // Load textures and set up animations for enemies
            // Add enemies to the 'enemies' list
            enemies.Add(JellyfishEnemy.CreateJellyfishEnemy(game, position));
            enemies.Add(BluebubbleEnemy.CreateBluebubbleEnemy(game, position, objManager));
            enemies.Add(SkeletonEnemy.CreateSkeletonEnemy(game, position, objManager));

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
                objManager.Remove(oldE);
            if (newE != null)
                objManager.Add(newE);
        }

    }
}
