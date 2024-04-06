using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;

namespace Sprint.Loader
{
    internal class HUDInterchangeableSprite : IHUD
    {
        private ISprite sprite;
        private Vector2 position;

        public HUDInterchangeableSprite(ISprite sprite, Vector2 position)
        {
            this.sprite = sprite;
            this.position = position;
        }

        public void GiveSprite(ISprite sprite)
        {
            this.sprite = sprite;
        }

        public ISprite GetSprite()
        {
            return sprite;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (sprite!= null)
                sprite.Draw(spriteBatch, position, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            // Nothing Here
        }
    }
}
