using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Collision;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Functions.SecondaryItem;
using System.Threading.Tasks.Dataflow;

namespace Sprint.Characters
{
    internal class SwordCollision : IGameObject, IMovingCollidable
    {

        private Rectangle bounds;
        private Player player;
        private float damage;

        public Rectangle BoundingBox => bounds;

        public CollisionTypes[] CollisionType => new CollisionTypes[] { CollisionTypes.SWORD };

        public SwordCollision(Rectangle boundBox, Player player, float damage)
        {
            bounds = boundBox;
            this.player = player;
            this.damage = damage;
        }

        public void Update(GameTime gameTime)
        {
            //no updated needed
        }

        // Get damage amount
        public float Damage()
        {
            return damage;
        }
        
        public void SetDamage(float dmg)
        {
            damage = dmg;
        }

        public void Move(Vector2 distance)
        {
            player.Move(distance);
        }

        public void SetPosition(Vector2 pos)
        {
            bounds = new((int)pos.X, (int)pos.Y, bounds.Width, bounds.Height);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //no draw needed
        }

    }
}
