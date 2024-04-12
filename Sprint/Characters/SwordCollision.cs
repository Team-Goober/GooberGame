using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Collision;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint.Characters
{
    internal class SwordCollision : IGameObject, IMovingCollidable
    {

        private Rectangle bounds;
        private Player player;
        private float moveScale = 0.07f;

        public Rectangle BoundingBox => bounds;

        public CollisionTypes[] CollisionType => new CollisionTypes[] { CollisionTypes.SWORD };

        public SwordCollision(Rectangle boundBox, Player player)
        {
            bounds = boundBox;
            this.player = player;
        }

        public void Update(GameTime gameTime)
        {
            //no updated needed
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
