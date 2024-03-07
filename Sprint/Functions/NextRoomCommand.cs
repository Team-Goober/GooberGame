using Sprint.Interfaces;
using Sprint.Levels;

namespace Sprint.Functions
{
    internal class NextRoomCommand : ICommand
    {

        private GameObjectManager receiver;

        public NextRoomCommand(GameObjectManager receiver) { 
            this.receiver = receiver;
        }

        public void Execute()
        {
            receiver.SwitchRoom(( receiver.RoomIndex() + 1) % receiver.NumRooms());
        }

    }
}
