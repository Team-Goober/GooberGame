using Sprint.Interfaces;
using Sprint.Levels;
using Microsoft.Xna.Framework;

namespace Sprint.Functions.RoomTransition
{
    internal class NextRoomCommand : ICommand
    {

        private GameObjectManager receiver;

        public NextRoomCommand(GameObjectManager receiver)
        {
            this.receiver = receiver;
        }

        public void Execute()
        {
            receiver.SwitchRoom(new Vector2(512, 350), (receiver.RoomIndex() + 1) % receiver.NumRooms());
        }

    }
}
