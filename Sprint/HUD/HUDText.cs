using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.HUD
{
    internal class HUDText : IHUD
    {

        private Vector2 position;
        private ZeldaText text;

        public HUDText(ZeldaText text, Vector2 position)
        {
            this.text = text;
            this.position = position;
        }

        public void Update(GameTime gameTime)
        {
            // no update
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            text.Draw(spriteBatch, position, gameTime);
        }

    }
}
