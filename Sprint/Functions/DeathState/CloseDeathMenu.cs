using Sprint.GameStates;
using Sprint.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Functions.DeathState
{
    internal class CloseDeathMenu : ICommand
    {
        private GameOverState receiver;

        public CloseDeathMenu(GameOverState receiver) 
        {
            this.receiver = receiver;
        }

        public void Execute()
        {
            receiver.CloseDeathMenu();
        }
    }
}
