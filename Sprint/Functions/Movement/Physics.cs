using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Sprint.Characters
{
    public class Physics
    {
        public List<string> directionList;

        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        public Vector2 Acceleration { get; private set; }

        public Physics(Vector2 posChar)
        {
            Position = posChar;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero; // Initialize acceleration
        }

        public void SetPosition(Vector2 newPosition)
        {
            Position = newPosition;
        }

        public void SetVelocity(Vector2 newVelocity)
        {
            Velocity = newVelocity;
        }

        public void SetAcceleration(Vector2 newAcceleration)
        {
            Acceleration = newAcceleration;
        }

        public void Update(GameTime gameTime)
        {
            // Update velocity based on acceleration
            Velocity += Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move position according to current velocity
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
