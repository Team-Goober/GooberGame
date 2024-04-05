using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint.Sprite;

namespace Sprint.Items
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
                { "heartPiece", ItemType.HeartPiece },
                { "specialKey", ItemType.SpecialKey },
                { "rupee", ItemType.Rupee },
                { "triforce", ItemType.Triforce },
                { "shield", ItemType.Shield },
                { "clock", ItemType.Clock },
                { "bow", ItemType.Bow },
                { "raft", ItemType.Raft },
                { "ladder", ItemType.Ladder },
                { "blueArrow", ItemType.BlueArrow },
                { "fairy", ItemType.Fairy },
                { "bomb", ItemType.Bomb },
                { "blueRing", ItemType.BlueRing },
                { "redRing", ItemType.RedRing },
                { "blueBoomerang", ItemType.BlueBoomerang },
                { "boomerang", ItemType.Boomerang },
                { "heart", ItemType.Heart },
                { "blueWand", ItemType.BlueWand },
                { "sword", ItemType.Sword },
                { "bracelet", ItemType.Bracelet },
                { "book", ItemType.Book },
                { "bluePotion", ItemType.BluePotion },
                { "redPotion", ItemType.RedPotion },
                { "key", ItemType.Key },
                { "map", ItemType.Map },
                { "compass", ItemType.Compass },
                { "candle", ItemType.Candle },
                { "flute", ItemType.Flute },
                { "meat", ItemType.Meat },
                { "fireball", ItemType.FireBall },
                { "oldmanText", ItemType.OldManText }
            };
        }

        /// <summary>
        /// Builds item from string name
        /// </summary>
        /// <param name="name">Name of item to make</param>
        /// <param name="position">World position to spawn at</param>
        /// <returns></returns>
        public Item MakeItem(string name, Vector2 position)
        {
            return new Item(spriteLoader.BuildSprite(ANIM_FILE, name), position, itemTypeConverter[name]);
        }
    }
}