using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
