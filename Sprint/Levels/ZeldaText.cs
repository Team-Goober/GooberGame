using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Sprint.Interfaces;
using System.Diagnostics;

namespace Sprint.Levels
{
    public class ZeldaText : IGameObject
    {
        private SpriteFont font;
        private string text;
        private Vector2 position;
        private Color color;

        // Constructor with font name parameter
        public ZeldaText(string fontName, string text, Vector2 position, Color color, ContentManager content)
        {
            // Load the font
            font = content.Load<SpriteFont>(fontName);

            this.text = text;
            this.position = position;
            this.color = color;
            Debug.WriteLine(color);
        }


        // Draw method
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.DrawString(font, text, position, color);
        }

        public void Update(GameTime gameTime)
        {
            // None
        }
    }
}

