using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using System;

namespace Sprint.Sprite
{
    internal class AutoAtlas : IAtlas
    {
        private Rectangle sheetArea;
        private int rows;
        private int cols;
        private int padding;

        private int frame;
        private int frameCount;

        private bool looping;
        private float fps;
        private TimeSpan timeAtLastFrame;

        public AutoAtlas(Rectangle sheetArea, int rows, int cols, int padding, bool loop, float framerate)
        {
            this.sheetArea = sheetArea;
            this.rows = rows;
            this.cols = cols;
            this.padding = padding;

            frame = 0;
            frameCount = rows * cols;

            looping = loop;
            fps = framerate;
        }

        // Move to next frame
        private void advance()
        {
            // Increment once linearly through frames
            if (frame < frameCount - 1)
            {
                frame++;
            }
            // Loop around if looping enabled
            else if (looping)
            {
                frame = 0;
            }
        }

        public void SetLooping(bool loop)
        {
            looping = loop;
        }

        public void SetFramerate(int fps)
        {
            this.fps = fps;
        }

        public Vector2 CurrentCenterPoint()
        {
            // Centerpoint is top-left corner
            return Vector2.Zero;
        }
        public float CurrentDuration()
        {
            // Frames have constant timing
            return 1.0f;
        }

        public Rectangle CurrentFrame()
        {
            // Calculate the row and column indices of the current frame
            int paddedWidth = (sheetArea.Width + padding) / cols;
            int paddedHeight = (sheetArea.Height + padding) / rows;
            int row = frame / cols;
            int column = frame % cols;

            // Calculate bounds of the frame as Rectangle
            Rectangle sourceRectangle = new Rectangle(sheetArea.X + paddedWidth * column, sheetArea.Y + paddedHeight * row, paddedWidth - padding, paddedHeight - padding);
            return sourceRectangle;
        }

        public void Update(GameTime gameTime)
        {
            // Switch frames if needed and based on framerate
            if (CurrentDuration() > 0 && fps > 0
               && (gameTime.TotalGameTime - timeAtLastFrame).TotalSeconds > CurrentDuration() / fps)
            {
                advance();
                timeAtLastFrame = gameTime.TotalGameTime;
            }
        }

        public void Reset()
        {
            frame = 0;
        }
    }
}
