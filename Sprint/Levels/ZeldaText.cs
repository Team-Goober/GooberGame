using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Sprint.Interfaces;
using System.Collections.Generic;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;
using System;
using System.Linq;
using System.Diagnostics;

namespace Sprint.Levels
{
    public class ZeldaText
    {
        private SpriteFont font;
        private Dictionary<string, Vector2> lines; // Dictionary of strings to write to their positions
        private Color color;
        private float textScale;
        private Vector2 charSize;

        // Constructor with font name parameter
        public ZeldaText(string fontName, List<string> text, Vector2 charSize, float textScale, Color color, ContentManager content)
        {
            // Load the font
            font = content.Load<SpriteFont>(fontName);

            this.charSize = charSize;

            SetText(text);

            this.color = color;
            this.textScale = textScale;
        }

        // Set text based on single string with | used to delineate line breaks
        public void SetText(string text)
        {
            SetText(text.Split("|").ToList());
        }

        public void SetText(List<string> text)
        {
            // Calculate relative positions for each line
            lines = new();
            for (int i = 0; i < text.Count; i++)
            {
                lines.Add(text[i], new Vector2(text[i].Length * - charSize.X / 2,
                    (i - 0.5f) * charSize.Y));
            }
        }

        // Draw method
        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            // Draw each line at its own position
            foreach(KeyValuePair<string, Vector2> kvp in lines)
            {
                spriteBatch.DrawString(font, kvp.Key, position + kvp.Value, color, 0, Vector2.Zero, textScale, SpriteEffects.None, 0f);
            }
            
        }

    }
}

