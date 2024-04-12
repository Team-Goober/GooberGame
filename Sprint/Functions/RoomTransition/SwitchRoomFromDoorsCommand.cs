using Sprint.Interfaces;

namespace Sprint.Functions.RoomTransition
{
    internal class SwitchRoomFromDoorsCommand : ICommand
    {

        private IDoor[,] receivers;
        private DungeonState dungeon;

        public SwitchRoomFromDoorsCommand(IDoor[,] doors, DungeonState dungeon)
        {
            receivers = doors;
            this.dungeon = dungeon;
        }

        public void Execute()
        {
            receivers[dungeon.RoomIndex().Y, dungeon.RoomIndex().X].SwitchRoom();
        }

    }
}
