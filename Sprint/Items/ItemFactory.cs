using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint.Sprite;

namespace Sprint.Items
{
    public class ItemFactory
    {
        private const string ANIM_FILE = "itemAnims";
        private SpriteLoader spriteLoader;
        public ItemFactory(SpriteLoader spriteLoader)
        {
            this.spriteLoader = spriteLoader;
        }

        /// <summary>
        /// Builds item from string name
        /// </summary>
        /// <param name="name">Name of item to make</param>
        /// <param name="position">World position to spawn at</param>
        /// <returns></returns>
        /*public Item MakeItem(string name, Vector2 position)
        {
            return new Item(spriteLoader.BuildSprite(ANIM_FILE, name), position, itemTypeConverter[name]);
        }*/
    }
}