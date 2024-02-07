using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Sprint.Interfaces;
using Sprint.Sprite;
using System;


namespace Sprint
{

    public class MoveSystems
    {

        public Vector2 spriteLocation;

        //gets position of the sprite
        public MoveSystems(Game1 game, Vector2 posChar)
        {

            this.spriteLocation = posChar;

        }


        public void MoveLeft()
        {
            //Moves the sprite to the left
            this.spriteLocation.X -= 5;
        }

        public void MoveRight()
        {
            //Moves the sprite to the right
            this.spriteLocation.X += 5;
        }

        public void MoveUp()
        {
            //Moves the sprite up
            this.spriteLocation.Y -= 5;
        }

        public void MoveDown()
        {
            //Moves the sprite down
            this.spriteLocation.Y += 5;
        }




    }
}

