using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint.Commands;
using Sprint.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Controllers
{
    public class MouseC : IController
    {
        private MouseState oldState;
        private Game1 game;
        private Dictionary<Keys, ICommand> mouseActions;

        public MouseC(Game1 newGame) 
        {
            this.oldState = Mouse.GetState();
            this.game = newGame;
        }

        public void UpdateInput(GameTime gameTime)
        {
            if(oldState.RightButton == ButtonState.Pressed)
            {
                ICommand quit = new Quit(game);
                quit.Execute();
            }
            this.oldState = Mouse.GetState();
        }
    }
}
