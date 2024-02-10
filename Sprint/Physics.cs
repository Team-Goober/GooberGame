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

        public Vector2 spriteLocation;
        public List<string> directionList;

        public Vector2 Position { get; private set; }

        //gets position of the sprite
        public Physics(Game1 game, Vector2 posChar)
        {
            this.spriteLocation = posChar;

            Position = posChar;

        }
        
        public void Move(Vector2 diff) {
            Position += diff;
        }

    }
}

