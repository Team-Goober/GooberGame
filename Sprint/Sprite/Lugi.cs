using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System.Diagnostics;

namespace Sprint.Sprite
{

    public class Lugi : ISprite
    {
        public Texture2D Texture;
        public Texture2D TextureRight;
        public int Rows;
        public int Columns;
        public int currentFrame;
        public int totalFrames;
        public int lastFrameTime;
        public int frameTime;

        public int posX;
        public int posY;
        public bool reverseX;
        public bool reverseY;

        public Lugi(Texture2D texture, Texture2D textureRight, int rows, int columns)
        {
            this.Texture = texture;
            this.TextureRight = textureRight;
            this.Rows = rows;
            this.Columns = columns;
            this.currentFrame = 0;
            this.totalFrames = Rows * Columns;
            this.lastFrameTime = 0;
            this.frameTime = 250;

            this.posX = 300;
            this.posY = 200;
            this.reverseX = false;
            this.reverseY = false;
        }

        /*www.youtube.com/watch?v=hm4PkqS2bqY*/
        public void Update(GameTime gameTime)
        {
            lastFrameTime += gameTime.ElapsedGameTime.Milliseconds;
            if(lastFrameTime > frameTime)
            {
                lastFrameTime -= frameTime;
                currentFrame++;
                if (currentFrame == totalFrames)
                {
                    currentFrame = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, string animation)
        {
            if(animation == "frozen")
            {
                DrawFrozen(spriteBatch, location);
            }

            if(animation == "running")
            {
                DrawRunning(spriteBatch, location);
            }

            if(animation == "frozenUpDown")
            {
                DrawFrozenMoveUpAndDown(spriteBatch, location);
            }

            if(animation == "runningLeftRight")
            {
                if(!reverseX)
                {
                    DrawRunningLeft(spriteBatch, location);
                } else
                {
                    DrawRunningRight(spriteBatch, location);
                }
            }
        }

        public void DrawFrozen(SpriteBatch spriteBatch, Vector2 location)
        {
            Debug.WriteLine("Key 1 is Pressed!");

            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = 0;
            int column = 3;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, 100, 100);

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
        }

        public void DrawRunning(SpriteBatch spriteBatch, Vector2 location)
        {
            Debug.WriteLine("Key 2 is Pressed!");

            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = currentFrame / Columns;
            int column = 3 - currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, 100, 100);


            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
        }

        public void DrawFrozenMoveUpAndDown(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = 0;
            int column = 3;

            int screenHeight = 400;

            if(!reverseY)
            {
                posY += 2;
            } else
            {
                posY -= 2;
            }

            if(screenHeight == posY)
            {
                reverseY = true;
            } else if(0 == posY)
            {
                reverseY = false;
            }
            

            Debug.WriteLine(posY);

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, posY, 100, 100);

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
        }

        public void DrawRunningLeft(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = currentFrame / Columns;
            int column = 3 - currentFrame % Columns;

            posX -= 2;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle(posX, (int)location.Y, 100, 100);

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);

            if (posX < 0)
            {
                reverseX = true;
            }
        }

        public void DrawRunningRight(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = currentFrame / Columns;
            int column = 3 - currentFrame % Columns;

            int screenWidth = 720;
            
            posX += 2;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle(posX, (int)location.Y, 100, 100);

            spriteBatch.Draw(TextureRight, destinationRectangle, sourceRectangle, Color.White);

            if (posX > screenWidth)
            {
                reverseX = false;
            }
        }

        public void RegisterAnimation(string label, IAtlas atlas)
        {
            throw new System.NotImplementedException();
        }

        public void SetAnimation(string label)
        {
            throw new System.NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, GameTime gameTime)
        {
            throw new System.NotImplementedException();
        }

        public void SetScale(float scale)
        {
            throw new System.NotImplementedException();
        }
    }
}
