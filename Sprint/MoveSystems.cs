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
        public List<string> directionList;

        //gets position of the sprite
        public MoveSystems(Game1 game, Vector2 posChar, List<string> directionList)
        {
            this.spriteLocation = posChar;

            this.directionList = directionList;
            

        }

        public void MoveLeft()
        {
            //Moves the sprite to the left
            this.spriteLocation.X -= 5;
            this.directionList[0] = "left";
            
            
            
        }

        public void MoveRight()
        {
            //Moves the sprite to the right
            this.spriteLocation.X += 5;
            this.directionList[0] = "right";

        }

        public void MoveUp()
        {
            //Moves the sprite up
            this.spriteLocation.Y -= 5;
            this.directionList[0] = "up";

        }

        public void MoveDown()
        {
            //Moves the sprite down
            this.spriteLocation.Y += 5;
            this.directionList[0] = "down";

        }
    }
}

