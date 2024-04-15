using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Levels;

namespace Sprint.Functions.RoomTransition
{
    internal class SwitchRoomFromDoorsCommand : ICommand
    {

        private Vector2 direction;
        private DungeonState dungeon;

        public SwitchRoomFromDoorsCommand(DungeonState dungeon, Vector2 direction)
        {
            this.direction = direction;
            this.dungeon = dungeon;
        }

        public void Execute()
        {
            // Get current room
            Room r = dungeon.GetRoomAt(dungeon.RoomIndex());

            if(r.GetDoors().Count < 4)
            {
                // Room doesn't have directional doors, so don't click through
                return;
            }
            // Get door in room by direction and switch
            r?.GetDoors()[Directions.GetIndex(direction)]?.SwitchRoom();
        }

    }
}
