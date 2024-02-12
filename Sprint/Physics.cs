using Microsoft.Xna.Framework;
using Sprint.Commands;
using System.Collections.Generic;


namespace Sprint
{

    public class Physics
    {

        public List<string> directionList;

        public Vector2 Position { get; private set; }
        public string Direction { get; private set; }

        //gets position of the sprite
        public Physics(Game1 game, Vector2 posChar)
        { 
            Position = posChar;
        }
        
        public void Move(Vector2 diff) {
            DirectionUpdate(diff);
            Position += diff;
        }

        public void DirectionUpdate(Vector2 diff)
        {
            if(diff.X > 0)
            {
                Direction = "right";
            }

            if(diff.X < 0)
            {
                Direction = "left";
            }

            if(diff.Y >  0)
            {
                Direction = "down";
            }

            if(diff.Y < 0)
            {
                Direction = "up";
            }
        }

        public void Update(GameTime gameTime)
        {
            
        }

    }
}

