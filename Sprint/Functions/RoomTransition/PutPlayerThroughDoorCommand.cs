using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Functions.RoomTransition
{
    internal class PutPlayerThroughDoorCommand : ICommand
    {
        private Player player;
        private IDoor door;

        public PutPlayerThroughDoorCommand(ICollidable receiver, ICollidable effector, Vector2 overlap)
        {
            this.player = (Player)receiver;
            this.door = (IDoor)effector;
        }

        public void Execute()
        {
            player.MoveTo(door.PlayerSpawnPosition());
        }
    }
}
