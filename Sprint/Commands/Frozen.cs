using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Sprite;

namespace Sprint.Commands
{
    public class Frozen : ICommand
    {
        private Game1 game;

        public Frozen(Game1 newGame) 
        {
            this.game = newGame;
        }

        public void Execute()
        {
            Debug.WriteLine("Key 1 is Executed!");
            game.SetAnimation("frozen");
        }
    }
}
