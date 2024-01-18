using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Sprint.Commands;
using Sprint.Interfaces;
using Sprint.Sprite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Sprint.Controllers
{
    /*
     * Invoker
     */
    public class KeyboardC : IController
    {
        private KeyboardState oldState;
        private ISprite sprite;
        private Dictionary<Keys, ICommand> keyActions;

        public KeyboardC(Game1 game) 
        {
            this.oldState = Keyboard.GetState();

            keyActions = new Dictionary<Keys, ICommand>() 
            {
                { Keys.D0, new Quit(game) },
                { Keys.D1, new Frozen(game) },
                { Keys.D2, new RunningInPlace(game) },
                { Keys.D3, new  FrozenMoveUpAndDown(game) },
                {Keys.D4, new RunningLeftAndRight(game) }
            };

        }

        public void UpdateInput(GameTime gameTime)
        {
            Keys[] keys = this.oldState.GetPressedKeys();

            //Maybe Change
            foreach (Keys key in keys)
            {
                if (keyActions.ContainsKey(key))
                {
                    keyActions[key].Execute();
                }
            }

            this.oldState = Keyboard.GetState();
        }
    }
}
