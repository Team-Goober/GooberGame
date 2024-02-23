using Microsoft.Xna.Framework;
using Sprint.Interfaces;

namespace Sprint.Sprite
{
    internal abstract class TimedAtlas : IAtlas
    {

        protected int frame;
        private int frameCount;

        private bool looping;
        private float fps;
        private Timer timer;

        public TimedAtlas(int frameCount, bool loop, float framerate)
        {
            frame = 0;
            this.frameCount = frameCount;

            looping = loop;
            fps = framerate;

            timer = new Timer(1.0 / framerate);
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

            // Begin timer again
            timer.SetDuration(CurrentDuration() / fps);
            timer.Start();
        }

        public void SetLooping(bool loop)
        {
            looping = loop;
        }

        public void SetFramerate(float fps)
        {
            this.fps = fps;
        }

        public void PassTime(GameTime gameTime)
        {
            // Pass time to timer until it is time to go to next frame
            timer.Update(gameTime);
            if (timer.JustEnded)
            {
                advance();
            }
        }

        public void Reset()
        {
            // Start from first frame
            timer.Start();
            frame = 0;
        }

        public abstract Vector2 CurrentCenterPoint();
        public abstract float CurrentDuration();
        public abstract Rectangle CurrentFrame();
    }
}
