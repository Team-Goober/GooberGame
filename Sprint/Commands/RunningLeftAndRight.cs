using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sprint.Interfaces;

namespace Sprint.Commands
{
    public class RunningLeftAndRight : ICommand
    {
        Game1 game;

        public RunningLeftAndRight(Game1 newGame) 
        {
            this.game = newGame;
        }

        public void Execute()
        {
            Debug.WriteLine("Key 4 is Executed!");
            this.game.SetAnimation("runningLeftRight");
        }
    }
}
