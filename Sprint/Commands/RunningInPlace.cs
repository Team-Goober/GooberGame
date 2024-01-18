using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;

namespace Sprint.Commands
{
    public class RunningInPlace : ICommand
    {
        Game1 game;

        public RunningInPlace(Game1 newGame) 
        {
            this.game = newGame;
        }

        public void Execute()
        {
            Debug.WriteLine("Key 2 is Executed!");
            this.game.SetAnimation("running");
        }
    }
}
