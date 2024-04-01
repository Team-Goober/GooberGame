using Sprint.Interfaces;
using Sprint.Characters;
using Microsoft.Xna.Framework;
using System;

namespace Sprint.Characters
{
    //Command used for reversing direction of MoveVert when it collides with the Wall
    internal class ReverseDirCommand : ICommand
    {

        private MoveVert moveVert;
        private Vector2 overlap;

        public ReverseDirCommand(ICollidable receiver, ICollidable effector, Vector2 overlap)
        {
            this.moveVert = (MoveVert)receiver;
            this.overlap = overlap;
        }

        public void Execute()
        {
            if(Math.Abs(overlap.X) > Math.Abs(overlap.Y))
            {
                moveVert.ReverseHorDir();
            }
            else
            {
                moveVert.ReverseVerDir();
            }
        }
    }
}
