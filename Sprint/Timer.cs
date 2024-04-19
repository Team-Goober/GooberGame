using Microsoft.Xna.Framework;
using System;

namespace Sprint
{
    internal class Timer
    {

        public TimeSpan TimeLeft { get; private set; }
        public TimeSpan Duration { get; private set; }
        public bool Ended { get; private set; }
        public bool JustEnded { get; private set; }

        public bool Looping { get; private set; }

        public Timer(double seconds)
        {
            Duration = TimeSpan.FromSeconds(seconds);
            Ended = true;
            JustEnded = false;
        }

        // Begin countdown
        public void Start()
        {
            TimeLeft = Duration;
            Ended = false;
        }

        // Force end to countdown
        public void End()
        {
            Ended = true;
            JustEnded = true;
            TimeLeft = TimeSpan.Zero;
        }

        // Set length of countdown
        public void SetDuration(double seconds)
        {
            Duration = TimeSpan.FromSeconds(seconds);
        }

        public void SetTimeLeft(double seconds)
        {
            TimeLeft = TimeSpan.FromSeconds(seconds);
        }

        // Skip forward in countdown
        public void SubtractTime(double seconds)
        {
            TimeLeft -= TimeSpan.FromSeconds(seconds);
            // Handle time running out
            if (TimeLeft < TimeSpan.Zero)
            {
                // If looping, restart
                if (Looping)
                {
                    Start();
                    // Mark that a loop finish just occurred
                    JustEnded = true;
                }
                // Otherwise, end
                else
                {
                    End();
                }
            }
        }

        public void SetLooping(bool loop)
        {
            Looping = loop;
        }

        public void Update(GameTime gameTime)
        {
            // Cycle after end, reset JustEnded
            if (JustEnded)
            {
                JustEnded = false;
            }
            // Count down
            if (!Ended)
            {
                SubtractTime(gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

    }
}
