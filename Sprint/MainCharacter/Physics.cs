using Microsoft.Xna.Framework;
using Sprint.Commands;
using System.Collections.Generic;


namespace Sprint
{

    public class Physics
    {

        public List<string> directionList;

        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }

        //gets position of the sprite
        public Physics(Game1 game, Vector2 posChar)
        { 
            Position = posChar;
            Velocity = Vector2.Zero;
        }

        public void SetPosition(Vector2 newPosition)
        {
            Position = newPosition;
        }

        public void SetVelocity(Vector2 newVelocity)
        {
            Velocity = newVelocity;
        }

        public void Update(GameTime gameTime)
        {
            // Move position according to current velocity
            Position = Position + Velocity * (float)(gameTime.ElapsedGameTime.TotalSeconds);
        }

    }
}

