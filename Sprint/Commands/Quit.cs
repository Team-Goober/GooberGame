using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sprint.Interfaces;

namespace Sprint.Commands
{
    /*
     * https://www.newthinktank.com/2012/09/command-design-pattern-tutorial/
     * 
     * Command
     */
    public class Quit : ICommand
    {
        Game1 game;

        public Quit(Game1 newGame)
        {
            this.game = newGame;
        }

        public void Execute()
        {
            this.game.Exit();
        }
    }
}
