using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Collision;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Functions.SecondaryItem;

namespace Sprint.Characters
{
    internal class SwordCollision : IGameObject, IMovingCollidable
    {

        private Rectangle bounds;
        private Player player;
        private float moveScale = 0.07f;
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


        public float Damage()
        {
            return damage;
        }


        public void Move(Vector2 distance)
        {
            player.Move(distance * moveScale);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //no draw needed
        }

    }
}
