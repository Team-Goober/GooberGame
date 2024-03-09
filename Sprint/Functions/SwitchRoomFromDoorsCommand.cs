using Sprint.Interfaces;
using Sprint.Levels;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Sprint.Functions
{
    internal class SwitchRoomFromDoorsCommand : ICommand
    {

        private IDoor[] receivers;
        private GameObjectManager objManager;

        public SwitchRoomFromDoorsCommand(IDoor[] doors, GameObjectManager objManager) { 
            this.receivers = doors;
            this.objManager = objManager;
        }

        public void Execute()
        {
            receivers[objManager.RoomIndex()].SwitchRoom();
        }

    }
}
