using System;
using System.Collections.Generic;
using System.Numerics;
using Sprint.Levels;
using Sprint.Sprite;

namespace Sprint.Characters
{
    public class ItemFactory
    {
        private readonly Dictionary<string, ItemType> itemTypeConverter;
        private const string ANIM_FILE = "itemAnims";
        private SpriteLoader spriteLoader;
        public ItemFactory(SpriteLoader spriteLoader)
        {
            this.spriteLoader = spriteLoader;
            itemTypeConverter = new Dictionary<string, ItemType>()
            {
                { "arrow", ItemType.Arrow },
                { "heart", ItemType.Heart },
                { "specialKey", ItemType.SpecialKey },
                { "rupy", ItemType.Rupee },
                { "triforce", ItemType.Triforce },
                { "shield", ItemType.Shield },
                { "clock", ItemType.Clock },
                { "bow", ItemType.Bow },
                { "fence", ItemType.Fence },
                { "ladder", ItemType.Ladder },
                { "blueArrow", ItemType.BlueArrow },
                { "fairy", ItemType.Fairy },
                { "blueOrb", ItemType.BlueOrb },
                { "blueRing", ItemType.BlueRing },
                { "redRing", ItemType.RedRing },
                { "blueBoomerang", ItemType.BlueBoomerang },
                { "boomerang", ItemType.Boomerang },
                { "blueHeart", ItemType.BlueHeart },
                { "blueTorch", ItemType.BlueTorch },
                { "sword", ItemType.Sword },
                { "citem", ItemType.Citem },
                { "shieldRed", ItemType.ShieldRed },
                { "potion", ItemType.Potion },
                { "cake", ItemType.Cake },
                { "key", ItemType.Key },
                { "paper", ItemType.Paper },
                { "compass", ItemType.Compass },
                { "fireball", ItemType.FireBall },
                { "oldmanText", ItemType.OldmManText }
            };
        }

        /// <summary>
        /// Builds item from string name
        /// </summary>
        /// <param name="name">Name of item to make</param>
        /// <param name="position">World position to spawn at</param>
        /// <returns></returns>
        public Item MakeItem(String name, Vector2 position)
        {
            return new Item(spriteLoader.BuildSprite(ANIM_FILE, name), position, itemTypeConverter[name]);
        }
    }
}