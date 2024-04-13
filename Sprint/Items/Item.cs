
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Collision;
using Sprint.Interfaces;

namespace Sprint.Items
{
    internal class Item : IGameObject, ICollidable
    {
        Vector2 position;
        private Rectangle bounds;
        private IPowerup powerup;
        private bool isColliable;

        public Item(Vector2 position, IPowerup powerup)
        {
            isColliable = true;
            this.powerup = powerup;
            this.position = position;
            bounds = new Rectangle((int)position.X - 24, (int)position.Y - 24, 48, 48);
        }

        public Rectangle BoundingBox => bounds;

        public virtual CollisionTypes[] CollisionType
        {
            get
            {
                {
                    return new CollisionTypes[] { CollisionTypes.ITEM };
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            powerup.Draw(spriteBatch, position, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            powerup.Update(gameTime);
        }

        /// <summary>
        /// Get the current Item's ItemType
        /// </summary>
        /// <returns> The Item's ItemType</returns>
        public IPowerup GetPowerup()
        {
            return powerup;
        }

        public void SetPosition(Vector2 pos)
        {
            position = pos;
            bounds.X = (int)(position.X - bounds.Width / 2);
            bounds.Y = (int)(position.Y - bounds.Height / 2);
        }

        public bool CanPickup(Inventory inventory)
        {
            return isColliable && powerup.CanPickup(inventory);
        }

    }
}
