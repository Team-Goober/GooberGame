using Microsoft.Xna.Framework;
using Sprint.Door;
using Sprint.HUD;
using Sprint.Interfaces;
using Sprint.Sprite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.HUD
{
    internal class HUDFactory
    {
        private SpriteLoader spriteLoader;

        public HUDFactory(SpriteLoader spriteLoader)
        {
            this.spriteLoader = spriteLoader;
        }

        public IHUD MakeHUD(string type, string spriteFile, string spriteLabel, Vector2 position)
        {
            ISprite sprite = spriteLoader.BuildSprite(spriteFile, spriteLabel);


            switch (type)
            {
                case "HUDFrame":
                    return new HUDFrame(sprite, position);
                default:
                    break;
            }

            return null;
        }
    }
}
