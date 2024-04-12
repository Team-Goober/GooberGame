using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;

namespace Sprint.HUD
{
    internal class HUDSprite : IHUD
    {
        private ISprite sprite;
        private Vector2 position;

        public HUDSprite(ISprite sprite, Vector2 position)
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
            //Nothing
        }
    }
}
