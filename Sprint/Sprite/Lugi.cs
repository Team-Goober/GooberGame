using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System.Diagnostics;

namespace Sprint.Sprite
{

    public class Lugi : ISprite
    {
        public Texture2D Texture;
        public SpriteBatch spriteBatch;
        public int Rows;
        public int Columns;
        public int currentFrame;
        public int totalFrames;

        public Lugi(Texture2D texture, SpriteBatch newSpriteBatch, int rows, int columns)
        {
            this.Texture = texture;
            this.spriteBatch = newSpriteBatch;
            this.Rows = rows;
            this.Columns = columns;
            this.currentFrame = 0;
            this.totalFrames = Rows * Columns;
        }

        /*www.youtube.com/watch?v=hm4PkqS2bqY*/
        public void Update()
        {
            //currentFrame++;

            if (currentFrame == totalFrames)
            {
                currentFrame = 0;
            }
        }

        public void DrawFrozen (Vector2 location)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = currentFrame / Columns;

            Rectangle sourceRectangle = new Rectangle(width * 3, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, 100, 100);

            spriteBatch.Begin();
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }
    }
}
