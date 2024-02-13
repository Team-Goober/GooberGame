using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint.Interfaces;

namespace Sprint.Commands
{
    internal class PreviousTileCommand : ICommand
    {
        private TileManager tileManager;

        public PreviousTileCommand(TileManager tileManager)
        {
            this.tileManager = tileManager;
        }

        public void Execute()
        {

            tileManager.PreviousTile();
        }
    }
}
