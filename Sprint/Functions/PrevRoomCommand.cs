using Sprint.Interfaces;
using Sprint.Levels;
using Microsoft.Xna.Framework;

namespace Sprint.Functions
{
    internal class PrevRoomCommand : ICommand
    {

        private GameObjectManager receiver;

        public PrevRoomCommand(GameObjectManager receiver) { 
            this.receiver = receiver;
        }

        public void Execute()
        {
            receiver.SwitchRoom(new Vector2(512, 350), ( receiver.RoomIndex() - 1) % receiver.NumRooms());
        }

    }
}
