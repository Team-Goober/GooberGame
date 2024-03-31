using Sprint.Interfaces;
using Sprint.Levels;
using Microsoft.Xna.Framework;

namespace Sprint.Functions.RoomTransition
{
    internal class NextRoomCommand : ICommand
    {

        private DungeonState receiver;

        public NextRoomCommand(DungeonState receiver)
        {
            this.receiver = receiver;
        }

        public void Execute()
        {
            receiver.SwitchToNext();
        }

    }
}
