using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Collision;

namespace Sprint.Factory.Door
{
    internal class OpenDoor : IDoor, ICollidable
    {
        ISprite sprite;
        Vector2 position;
        Rectangle bounds;
        int otherSide;

        public Rectangle BoundingBox => bounds;

        public CollisionTypes[] CollisionType => new CollisionTypes[] { CollisionTypes.OPEN_DOOR, CollisionTypes.DOOR };


        public OpenDoor(ISprite sprite, Vector2 position, Vector2 size, int otherSide)
        {
            this.sprite = sprite;
            this.position = position;
            bounds = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            this.otherSide = otherSide;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public Rectangle GetBoundingBox()
        {
            return bounds;
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        // Returns index in Level's room array of the Room this leads to
        public int GetAdjacentRoomIndex()
        {
            return otherSide;
        }

    }
}