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
        GameState game;

        public Quit(GameState newGame)
        {
            this.game = newGame;
        }

        public void Execute()
        {
            this.game.Quit();
        }
    }
}
