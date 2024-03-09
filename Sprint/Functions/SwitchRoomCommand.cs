using Sprint.Interfaces;
using Sprint.Levels;
using Microsoft.Xna.Framework;

namespace Sprint.Functions
{
    internal class SwitchRoomCommand : ICommand
    {

        private IDoor receiver;

        public SwitchRoomCommand(ICollidable receiver, ICollidable effector, Vector2 overlap) {
            this.receiver = (IDoor)receiver;
        }

        public void Execute()
        {
            receiver.SwitchRoom();
        }

    }
}
