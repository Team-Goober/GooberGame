using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Sprint
{
    public class ZeldaText
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
        }

        
        // Draw method
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, text, position, color);
        }

   
    }
}

