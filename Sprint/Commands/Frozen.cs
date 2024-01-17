using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Sprite;

namespace Sprint.Commands
{
    public class Frozen : ICommand
    {
        ISprite sprite;

        public Frozen(ISprite newSprite) 
        {
            this.sprite = newSprite;
        }

        public void Execute()
        {
            sprite.DrawFrozen(new Vector2(300, 200));
        }
    }
}
