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
        private CycleTile tileManager;

        public PreviousTileCommand(CycleTile tileManager)
        {
            this.tileManager = tileManager;
        }

        public void Execute()
        {

            tileManager.PreviousTile();
        }
    }
}
