using Sprint.Interfaces;

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
