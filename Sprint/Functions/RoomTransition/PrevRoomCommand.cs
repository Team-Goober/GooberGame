using Sprint.Interfaces;
using Sprint.Levels;
using Microsoft.Xna.Framework;

namespace Sprint.Functions.RoomTransition
{
    internal class PrevRoomCommand : ICommand
    {

        private DungeonState receiver;

        public PrevRoomCommand(DungeonState receiver)
        {
            this.receiver = receiver;
        }

        public void Execute()
        {
            receiver.SwitchToPrevious();
        }

    }
}
