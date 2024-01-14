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
    /*
     * Invoker
     */
    public class KeyboardC : IController
    {
        private KeyboardState oldState;
        private Dictionary<Keys, ICommand> keyActions;

        public KeyboardC(KeyboardState newState, GameState game) 
        { 
            this.oldState = newState;

            keyActions = new Dictionary<Keys, ICommand>() 
            {
                {Keys.D0, new Quit(game)}
            };
        }

        public void UpdateInput()
        {
            Keys[] keys = this.oldState.GetPressedKeys();

            //Maybe Change
            foreach (Keys key in keys)
            {
                keyActions[key].Execute();
            }
        }
    }
}
