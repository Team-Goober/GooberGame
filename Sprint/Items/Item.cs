
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Collision;
using Sprint.Interfaces;

namespace Sprint.Items
{
    public class Item : IGameObject, ICollidable
    {
        ISprite sprite;
        Vector2 position;
        private Rectangle bounds;
        private ItemType itemType;
        private bool isColliable;

        public Item(ISprite sprite, Vector2 position, ItemType itemType)
        {
            isColliable = true;
            this.itemType = itemType;
            this.sprite = sprite;
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

        public bool GetColliable()
        {
            return isColliable;
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

        public void SetPosition(Vector2 pos)
        {
            position = pos;
            bounds.X = (int)position.X - 24;
            bounds.Y = (int)position.Y - 24;
        }
    }
}
