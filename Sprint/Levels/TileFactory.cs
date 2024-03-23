using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;
using System;
using System.Collections.Generic;

namespace Sprint.Levels
{
    internal class TileFactory
    {

        private SpriteLoader spriteLoader;

        private Dictionary<string, ITile> TileAnimDict;

        public TileFactory(SpriteLoader spriteLoader)
        {
            this.spriteLoader = spriteLoader;
        }

        /// <summary>
        /// Constructs a tile with given parameters
        /// </summary>
        /// <param name="type">Name of Tile subclass to use</param>
        /// <param name="spriteFile">File to find sprites in</param>
        /// <param name="spriteLabel">Label for sprite to use</param>
        /// <param name="position">Position in world space for this tile</param>
        /// <returns></returns>
        public ITile MakeTile(string type, string spriteFile, string spriteLabel, Vector2 position, Vector2 size)
        {


            ISprite sprite = spriteLoader.BuildSprite(spriteFile, spriteLabel);


            // TODO: Store this in a better way
            TileAnimDict = new Dictionary<string, ITile>
            {
                { "floor", new FloorTile(sprite, position, size) },
                { "wall", new WallTile(sprite, position, size) },
                { "gap", new GapTile(sprite, position, size) }
                
            };

            


            if(TileAnimDict.TryGetValue(type, out ITile tile))
            {
                return tile;
            }

            return null;


        }

    }
}
