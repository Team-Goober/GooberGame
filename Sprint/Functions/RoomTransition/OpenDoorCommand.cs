using Sprint.Door;
using Sprint.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace Sprint.Functions.RoomTransition
{
    internal class OpenDoorCommand : ICommand
    {

        private IDoor receiver;

        public OpenDoorCommand(ICollidable receiver, ICollidable effector, Vector2 overlap)
        {
            this.receiver = (IDoor)receiver;
        }

        public void Execute()
        {
            // Open the door
            receiver.SetOpen(true);
            // Open the other side of the door
            receiver.GetOtherFace().SetOpen(true);
        }
    }
}
