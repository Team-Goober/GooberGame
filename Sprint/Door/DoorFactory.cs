using Microsoft.Xna.Framework;
using Sprint.Door;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Sprite;

namespace Sprint.Door
{
    internal class DoorFactory
    {
        private SpriteLoader spriteLoader;

        public DoorFactory(SpriteLoader spriteLoader)
        {
            this.spriteLoader = spriteLoader;
        }

        /// <summary>
        /// Constructs a door with given parameters
        /// </summary>
        /// <param name="type">Name of Door subclass to use</param>
        /// <param name="spriteFile">File to find sprites in</param>
        /// <param name="spriteLabel">Label for sprite to use</param>
        /// <param name="position">Position in world space for this door</param>
        /// <returns></returns>
        public IDoor MakeDoor(string type, string spriteFile, string spriteLabel, Vector2 position, Vector2 size, Vector2 openSize, Vector2 spawnPosition, Vector2 sideOfRoom, Point roomIndices, DungeonState dungeon)
        {
            // TODO: Implement this function
            // Consider storing doors in file with reflection

            ISprite sprite = spriteLoader.BuildSprite(spriteFile, spriteLabel);

            switch (type)
            {
                case "open":
                    return new OpenDoor(sprite, position, size, openSize, sideOfRoom, roomIndices, spawnPosition, dungeon);
                case "wall":
                    return new WallDoor(sprite, position, size, sideOfRoom, roomIndices, dungeon);
                case "lock":
                    return new LockDoor(sprite, position, size, openSize, sideOfRoom, roomIndices, spawnPosition, dungeon);
                case "hidden":
                    return new HiddenDoor(sprite, position, size, openSize, sideOfRoom, roomIndices, spawnPosition, dungeon);
                case "puzzle":
                    return new PuzzleDoor(sprite, position, size, openSize, sideOfRoom, roomIndices, spawnPosition, dungeon);
                default:
                    break;
            }

            return null;

        }

    }
}
