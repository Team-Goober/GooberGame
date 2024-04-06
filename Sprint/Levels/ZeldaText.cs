using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Sprint.Interfaces;
using System.Collections.Generic;

namespace Sprint.Levels
{
    public class ZeldaText : IGameObject
    {
        private SpriteFont font;
        private Dictionary<string, Vector2> lines; // Dictionary of strings to write to their positions
        private Color color;

        // Constructor with font name parameter
        public ZeldaText(string fontName, List<string> text, Vector2 topLineCenterPos, Vector2 charSize, Color color, ContentManager content)
        {
            // Load the font
            font = content.Load<SpriteFont>(fontName);
            lines = new();
            // Calculate positions for each line
            for (int i = 0; i < text.Count; i++)
            {
                lines.Add(text[i], new Vector2(topLineCenterPos.X - (text[i].Length - 0.5f) * charSize.X / 2,
                    topLineCenterPos.Y + (i - 0.5f) * charSize.Y));
            }
            this.color = color;
        }


        // Draw method
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Draw each line at its own position
            foreach(KeyValuePair<string, Vector2> kvp in lines)
            {
                spriteBatch.DrawString(font, kvp.Key, kvp.Value, color);
            }
            
        }

        public void Update(GameTime gameTime)
        {
            // None
        }
    }
}

