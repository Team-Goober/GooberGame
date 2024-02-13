using Microsoft.Xna.Framework;
using Sprint.Commands;
using System.Collections.Generic;


namespace Sprint
{

    public class Physics
    {

        public List<string> directionList;

        public Vector2 Position { get; private set; }

        //gets position of the sprite
        public Physics(Game1 game, Vector2 posChar)
        { 
            Position = posChar;
        }
        
        public void Move(Vector2 diff) {
            Position += diff;
        }

        public void SetPosition(Vector2 newPosition)
        {
            Position = newPosition;
        }

        public void Update(GameTime gameTime)
        {
            
        }

    }
}

