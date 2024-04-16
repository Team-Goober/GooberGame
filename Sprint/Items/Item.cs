
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Collision;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
using Sprint.Levels;

namespace Sprint.Items
{
    internal class Item : IGameObject, ICollidable
    {
        Vector2 position;
        private Rectangle bounds;
        private IPowerup powerup;
        private bool isColliable;
        private int price;
        private ZeldaText priceDisplay;

        public Item(Vector2 position, IPowerup powerup, int price)
        {
            isColliable = true;
            this.powerup = powerup;
            this.position = position;
            bounds = new Rectangle((int)position.X - 24, (int)position.Y - 24, 48, 48);
            this.price = price;
            if(price > 0)
            {
                priceDisplay = new ZeldaText("nintendo", new() { ""+price }, new Vector2(16, 16), 0.5f, Color.Yellow, Goober.content);
            }
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
            if(priceDisplay != null)
            {
                priceDisplay.Draw(spriteBatch, position - new Vector2(bounds.Width, bounds.Height)/2, gameTime);
            }
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

        public int GetPrice()
        {
            return price;
        }

        public void SetPosition(Vector2 pos)
        {
            position = pos;
            bounds.X = (int)(position.X - bounds.Width / 2);
            bounds.Y = (int)(position.Y - bounds.Height / 2);
        }

        // True if the inventory has room for the item and price is met
        public bool CanPickup(Inventory inventory)
        {
            return isColliable && powerup.CanPickup(inventory) && inventory.StackQuantity(Inventory.RupeeLabel) >= price;
        }

    }
}
