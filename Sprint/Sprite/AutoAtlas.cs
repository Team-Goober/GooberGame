using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using System;

namespace Sprint.Sprite
{
    internal class AutoAtlas : TimedAtlas, IAtlas
    {
        private Rectangle sheetArea;
        private int rows;
        private int cols;
        private int padding;
        private Vector2 centerPoint;

        public AutoAtlas(Rectangle sheetArea, int rows, int cols, int padding, Vector2 centerPoint, bool loop, float framerate): base(rows*cols, loop, framerate)
        {
            this.sheetArea = sheetArea;
            this.rows = rows;
            this.cols = cols;
            this.padding = padding;
            this.centerPoint = centerPoint;
        }

        override public Vector2 CurrentCenterPoint()
        {
            // Centerpoint is constant between frames
            return centerPoint;
        }

        override public float CurrentDuration()
        {
            // Frames have constant timing
            return 1.0f;
        }

        override public Rectangle CurrentFrame()
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
    }
}
