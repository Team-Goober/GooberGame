using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;

namespace Sprint.Levels
{
    internal class BackgroundTexture : IGameObject
    {

        ISprite sprite;
        Vector2 position;

        public BackgroundTexture(ISprite sprite, Vector2 position)
        {
            this.sprite = sprite;
            this.position = position;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            // nothing to update
        }
    }
}
