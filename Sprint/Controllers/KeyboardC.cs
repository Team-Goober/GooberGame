using Microsoft.Xna.Framework.Input;
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

        public KeyboardC(KeyboardState newState) 
        { 
            this.oldState = newState;

            keyActions = new Dictionary<Keys, ICommand>();
        }

        public void UpdateInput()
        {
            
        }
    }
}
