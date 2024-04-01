using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Loader;
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

        public List<HUDNumber> MakeNumber(string level, Vector2 position, int spriteSize)
        {
            List<HUDNumber> nums = new List<HUDNumber>();
            float x = position.X;
            for(int i = 0; i < 2; i++)
            {
                ISprite sprite = spriteLoader.BuildSprite(LOCATION, "Numbers");
                nums.Add(new HUDNumber(sprite, new Vector2(x, position.Y)));
                x += spriteSize;
            }
            char[] arr = level.ToCharArray();
            nums[0].SetNumber(arr[0].ToString());
            nums[1].SetNumber(arr[1].ToString());

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
