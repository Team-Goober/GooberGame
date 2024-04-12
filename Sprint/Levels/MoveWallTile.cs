using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Collision;

namespace Sprint.Levels
{
    internal class MoveWallTile : ITile, ICollidable, IMovingCollidable
    {
        ISprite sprite;
        Vector2 position;
        Vector2 originalPosition;
        Rectangle bounds;

        public Rectangle BoundingBox => bounds;

        public CollisionTypes[] CollisionType => new CollisionTypes[] { CollisionTypes.MOVEWALL };

        public MoveWallTile(ISprite sprite, Vector2 position, Vector2 size)
        {
            this.sprite = sprite;
            this.position = position;
            this.originalPosition = position;
            bounds = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
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


        // TEST!! Moves the player by a set distance
        public void Move(Vector2 distance)
        {
            position = position + distance;
            bounds.X = (int)position.X;
            bounds.Y = (int)position.Y;
        }

        public (Vector2, Vector2) GetPosition()
        {
            return (originalPosition, position);
        }
    }
}
