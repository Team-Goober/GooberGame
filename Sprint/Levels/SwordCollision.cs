using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Characters;

namespace Sprint.Levels
{
    internal class SwordCollision : IGameObject, IMovingCollidable
    {

        private Rectangle bounds;
        private Player player;

        public SwordCollision(Rectangle boundBox, Player player)
        {
            this.bounds = boundBox;
            this.player = player;
        }


        public Rectangle GetBoundingBox()
        {
            return bounds;
        }

        public void Update(GameTime gameTime)
        {
            //no updated needed
        }


        public void Move(Vector2 distance)
        {
            player.Move(distance);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //no draw needed
        }
        
    }
}
