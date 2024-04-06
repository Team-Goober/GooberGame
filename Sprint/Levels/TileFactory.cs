using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;

namespace Sprint.Levels
{
    internal class TileFactory
    {

        private SpriteLoader spriteLoader;

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
            // TODO: Store this in a better way

            ISprite sprite = spriteLoader.BuildSprite(spriteFile, spriteLabel);

            if (type.Equals("floor"))
            {
                return new FloorTile(sprite, position, size);
            }
            else if (type.Equals("wall"))
            {
                return new WallTile(sprite, position, size);
            }
            else if (type.Equals("gap"))
            {
                return new GapTile(sprite, position, size);
            }
            else if (type.Equals("moveWall"))
            {
                return new MoveWallTile(sprite, position, size);
            }
            else
            {
                return null;
            }
        }

    }
}
