using Sprint.GameStates;
using Sprint.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Functions.WinStateCommands
{
    internal class CloseWin : ICommand
    {
        public WinState receiver;

        public CloseWin(WinState receiver) 
        {
            this.receiver = receiver;
        }

        public void Execute()
        {
            receiver.CloseWin();
        }
    }
}
