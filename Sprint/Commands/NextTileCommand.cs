using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint.Interfaces;

namespace Sprint.Commands
{
    internal class NextTileCommand : ICommand
    {
        private TileManager tileManager;

        public NextTileCommand(TileManager enemyManager)
        {
            this.tileManager = enemyManager;
        }

        public void Execute()
        {

            tileManager.NextTile();
        }
    }
}
