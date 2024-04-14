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

        public Timer(double seconds)
        {
            Duration = TimeSpan.FromSeconds(seconds);
            Ended = true;
            JustEnded = false;
        }

        public void Start()
        {
            TimeLeft = Duration;
            Ended = false;
        }

        public void End()
        {
            Ended = true;
            JustEnded = true;
        }

        public void SetDuration(double seconds)
        {
            Duration = TimeSpan.FromSeconds(seconds);
        }

        public void SubtractTime(double seconds)
        {
            TimeLeft -= TimeSpan.FromSeconds(seconds);
            if (TimeLeft < TimeSpan.Zero)
            {
                End();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (JustEnded)
            {
                JustEnded = false;
            }

            if (!Ended)
            {
                SubtractTime(gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

    }
}
