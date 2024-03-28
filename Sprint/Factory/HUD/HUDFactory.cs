using Microsoft.Xna.Framework;
using Sprint.Factory.Door;
using Sprint.Interfaces;
using Sprint.Sprite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Factory.HUD
{
    internal class HUDFactory
    {
        private SpriteLoader spriteLoader;

        public HUDFactory(SpriteLoader spriteLoader)
        {
            this.spriteLoader = spriteLoader;
        }

        public IHUD MakeHUD(string spriteFile, string spriteLabel, Vector2 position)
        {
            ISprite sprite = spriteLoader.BuildSprite(spriteFile, spriteLabel);

            return new Frame(sprite, position);
        }

        public List<IHUD> MakeNumber(string level, string spriteFile, Vector2 position)
        {
            List<IHUD> nums = new List<IHUD>();
            char[] charArr = level.ToCharArray();
            float x = position.X;
            for(int i = 0; i < charArr.Length; i++)
            {
                ISprite sprite = spriteLoader.BuildSprite(spriteFile, charArr[i].ToString());
                nums.Add(new Number(sprite, new Vector2(x, position.Y)));
                x += 32;
            }

            return nums; 
        }

        public List<IHUD> MakeFullHeart()
        {
            return null;
        }

        public List<IHUD> MakeEmptyHeart()
        {
            return null;
        }

        public List<IHUD> MakeHalfHeart()
        {
            return null;
        }
    }
}
