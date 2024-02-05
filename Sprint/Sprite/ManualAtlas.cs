using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using System;

namespace Sprint.Sprite
{
    internal class ManualAtlas : IAtlas
    {

        private Rectangle[] rects;
        private Vector2[] centers;
        private float[] durs;
        private int frame;

        private bool looping;
        private float fps;
        private TimeSpan timeAtLastFrame;

        public ManualAtlas(Rectangle[] rectangles, Vector2[] centerPoints, float[] durations)
        {
            this.rects = rectangles;
            this.centers = centerPoints;
            this.durs = durations;
            frame = 0;
        }


        // Move to next frame
        public void advance()
        {
            // Increment once linearly through frames
            if (frame < rects.Length - 1)
            {
                frame++;
            }
            // Loop around if looping enabled
            else if (looping)
            {
                frame = 0;
            }
        }

        public Vector2 CurrentCenterPoint()
        {
            return centers[frame];
        }

        public float CurrentDuration()
        {
            return durs[frame];
        }

        public Rectangle CurrentFrame()
        {
            return rects[frame];
        }

        public void Reset()
        {
            frame = 0;
        }

        public void SetLooping(bool loop)
        {
            looping = loop;
        }

        public void SetFramerate(float fps)
        {
            this.fps = fps;
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

    }
}
