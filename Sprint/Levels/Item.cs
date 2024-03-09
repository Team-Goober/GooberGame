
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Characters;
using Sprint.Collision;
using Sprint.Interfaces;

namespace Sprint.Levels
{
    public class Item : IGameObject, ICollidable
    {
        ISprite sprite;
        Vector2 position;
        private Rectangle bounds;
        private ItemType itemType;

        public Item(ISprite sprite, Vector2 position, ItemType itemType)
        {
            this.itemType = itemType;
            this.sprite = sprite;
            this.position = position;
            bounds = new Rectangle((int)position.X-24, (int)position.Y-24, 48, 48);
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
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        /// <summary>
        /// Get the current Item's ItemType
        /// </summary>
        /// <returns> The Item's ItemType</returns>
        public ItemType GetItemType()
        {
            return itemType;
        }
    }
}
