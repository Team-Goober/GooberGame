using Sprint.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint.Commands.SecondaryItem;
using Sprint.Input;
using Sprint.Sprite;
using Sprint.Levels;

namespace Sprint.Projectile
{
    internal class ProjectileSystem
    {

        public SimpleProjectileFactory ProjectileFactory;

        private const float spawnDistance = 40;

        public ProjectileSystem(Vector2 startPos, SpriteLoader spriteLoader)
        {

            this.ProjectileFactory = new SimpleProjectileFactory(spriteLoader, spawnDistance, false, null);
            ProjectileFactory.SetDirection(new Vector2(1, 90));
            ProjectileFactory.SetStartPosition(startPos);

        }

        // Sets the room that factory places projectiles into
        public void SetRoom(Room room)
        {
            ProjectileFactory.SetRoom(room);
        }

        public double DamageAmount()
        {
            // Plce holder
            return 0;
        }

        // Update direction of shot projectiles
        public void UpdateDirection(Vector2 dir)
        {
            ProjectileFactory.SetDirection(dir); 
        }

        // Updates location to place projectiles at
        public void UpdatePostion(Vector2 pos)
        {
            ProjectileFactory.SetStartPosition(pos);
        }

    }
}
