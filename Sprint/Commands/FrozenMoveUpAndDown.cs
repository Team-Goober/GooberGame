using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sprint.Interfaces;

namespace Sprint.Commands
{
    public class FrozenMoveUpAndDown : ICommand
    {
        Game1 game;

        public FrozenMoveUpAndDown(Game1 newGame)
        {
            this.game = newGame;
        }
        public void Execute()
        {
            Debug.WriteLine("Key 3 is Executed");
            this.game.SetAnimation("frozenUpDown");
        }
    }
}
