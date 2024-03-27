using Sprint.Interfaces;
using Sprint.Levels;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Sprint.Functions.RoomTransition
{
    internal class SwitchRoomFromDoorsCommand : ICommand
    {

        private IDoor[] receivers;
        private DungeonState dungeon;

        public SwitchRoomFromDoorsCommand(IDoor[] doors, DungeonState dungeon)
        {
            receivers = doors;
            this.dungeon = dungeon;
        }

        public void Execute()
        {
            receivers[dungeon.RoomIndex()].SwitchRoom();
        }

    }
}
