
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Collision;
using Sprint.Interfaces;
using System;

namespace Sprint.Levels
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
            this.isColliable = true;
            if(itemType == ItemType.FireBall || itemType == ItemType.OldmManText)
            {
                this.isColliable = false;
            }
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

        public bool GetColliable()
        {
            return this.isColliable;
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
