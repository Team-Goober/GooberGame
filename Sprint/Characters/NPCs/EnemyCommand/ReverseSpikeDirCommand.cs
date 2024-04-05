


using Sprint.Interfaces;
using Sprint.Characters;
using Microsoft.Xna.Framework;
using System;

namespace Sprint.Characters
{
    //Command used for reversing direction of MoveVert when it collides with the Wall
    internal class ReverseSpikeDirCommand : ICommand
    {

        private MoveSpike moveSpike;
        private Vector2 overlap;

        public ReverseSpikeDirCommand(ICollidable receiver, ICollidable effector, Vector2 overlap)
        {
            this.moveSpike = (MoveSpike)receiver;
            this.overlap = overlap;
        }

        public void Execute()
        {
            if (Math.Abs(overlap.X) > Math.Abs(overlap.Y))
            {
                moveSpike.ReverseHorDir();
            }
            else
            {
                moveSpike.ReverseVerDir();
            }
        }
    }
}
