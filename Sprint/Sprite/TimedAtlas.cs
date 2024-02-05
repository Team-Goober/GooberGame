using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using System;

namespace Sprint.Sprite
{
    internal abstract class TimedAtlas : IAtlas
    {

        protected int frame;
        private int frameCount;

        private bool looping;
        private float fps;
        private TimeSpan timeAtLastFrame;

        public TimedAtlas(int frameCount, bool loop, float framerate)
        {
            frame = 0;
            this.frameCount = frameCount;

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

        public void Reset()
        {
            frame = 0;
        }

        public abstract Vector2 CurrentCenterPoint();
        public abstract float CurrentDuration();
        public abstract Rectangle CurrentFrame();
    }
}
