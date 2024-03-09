using Sprint.Factory.Door;
using Sprint.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace Sprint.Functions
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
            receiver.SetOpen(true);
        }
    }
}
