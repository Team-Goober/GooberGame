using Microsoft.Xna.Framework;
using Sprint.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint
{
    /*
     * Receiver
     */
    public class GameState
    {
        Game game;

        public GameState(Game newGame) 
        { 
            this.game = newGame;
        }

        public void Quit()
        {
            this.game.Exit();
        }
    }
}
