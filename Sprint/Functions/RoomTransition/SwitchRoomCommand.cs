using Sprint.Interfaces;
using Sprint.Levels;
using Microsoft.Xna.Framework;
using Sprint.Characters;
using System.Diagnostics;

namespace Sprint.Functions.RoomTransition
{
    internal class SwitchRoomCommand : ICommand
    {

        private IDoor receiver;
        private Player effector;

        public SwitchRoomCommand(ICollidable receiver, ICollidable effector, Vector2 overlap)
        {
            this.receiver = (IDoor)receiver;
            this.effector = (Player)effector;
        }

        public void Execute()
        {
            receiver.SwitchRoom();
        }

    }
}
