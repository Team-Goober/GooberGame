using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Sprint.Interfaces;
using Sprint.Sprite;
using System;


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

        public void Update(GameTime gameTime)
        {
            
        }

    }
}

