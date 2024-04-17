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

        public void UpdateVelocity(float baseFriction, float speedLimit, GameTime gameTime)
        {
            // Calculate friction dynamically based on velocity
            float friction = baseFriction;
            if (Velocity.LengthSquared() > 0)
            {
                // Apply higher friction if the object is moving
                friction = CharacterConstants.MOVING_FRICTION;
            }

            // Update velocity based on acceleration
            Vector2 newVelocity = Velocity + Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Apply friction to gradually slow down the object
            newVelocity *= (1f - friction);

            // Clamp velocity to ensure it does not exceed the speed limit
            newVelocity.X = MathHelper.Clamp(newVelocity.X, -speedLimit, speedLimit);
            newVelocity.Y = MathHelper.Clamp(newVelocity.Y, -speedLimit, speedLimit);

            // Update velocity
            Velocity = newVelocity;
        }


        public void Update(GameTime gameTime)
        {
            // Move position according to current velocity
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}