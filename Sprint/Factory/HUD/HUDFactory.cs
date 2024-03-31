using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;
using System.Collections.Generic;
using System.Reflection.Emit;


namespace Sprint.Factory.HUD
{
    internal class HUDFactory
    {
        private SpriteLoader spriteLoader;

        const string LOCATION = "HUD/HUDSprite";

        public HUDFactory(SpriteLoader spriteLoader)
        {
            this.spriteLoader = spriteLoader;
        }

        public IHUD MakeHUDSprite(string spriteLabel, Vector2 position)
        {
            ISprite sprite = spriteLoader.BuildSprite(LOCATION, spriteLabel);

            return new HUDSprite(sprite, position);
        }

        public List<IHUD> MakeNumber(string level, Vector2 position, int spriteSize)
        {
            List<IHUD> nums = new List<IHUD>();
            char[] charArr = level.ToCharArray();
            float x = position.X;
            for(int i = 0; i < charArr.Length; i++)
            {
                ISprite sprite = spriteLoader.BuildSprite(LOCATION, charArr[i].ToString());
                nums.Add(new HUDSprite(sprite, new Vector2(x, position.Y)));
                x += spriteSize;
            }

            return nums; 
        }

        public List<IHUD> MakeHearts(int amount, string spriteLabel, Vector2 position, int spriteSize)
        {
            List<IHUD> hearts = new List<IHUD>();

            float x = position.X;
            for (int i = 0; i < amount; i++)
            {
                ISprite sprite = spriteLoader.BuildSprite(LOCATION, spriteLabel);
                hearts.Add(new HUDSprite(sprite, new Vector2(x, position.Y)));
                x += spriteSize;
            }

            return hearts;
        }
    }
}
