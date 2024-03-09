using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Sprite;

namespace Sprint.Factory.Door
{
    internal class DoorFactory
    {
        private SpriteLoader spriteLoader;
        private GameObjectManager objManager;

        public DoorFactory(SpriteLoader spriteLoader, GameObjectManager objManager)
        {
            this.spriteLoader = spriteLoader;
            this.objManager = objManager;
        }

        /// <summary>
        /// Constructs a door with given parameters
        /// </summary>
        /// <param name="type">Name of Door subclass to use</param>
        /// <param name="spriteFile">File to find sprites in</param>
        /// <param name="spriteLabel">Label for sprite to use</param>
        /// <param name="position">Position in world space for this door</param>
        /// <returns></returns>
        public IDoor MakeDoor(string type, string spriteFile, string spriteLabel, Vector2 position, Vector2 size, Vector2 openSize, Vector2 spawnPosition, int otherSide)
        {
            // TODO: Implement this function
            // Consider storing doors in file with reflection

            ISprite sprite = spriteLoader.BuildSprite(spriteFile, spriteLabel);
            
            switch(type)
            {
                case "open":
                    return new OpenDoor(sprite, position, size, openSize, otherSide, spawnPosition, objManager);
                case "wall":
                    return new WallDoor(sprite, position, size, objManager);
                case "lock":
                    return new LockDoor(sprite, position, size, openSize, otherSide, spawnPosition, objManager);
                case "hidden":
                    return new HiddenDoor(sprite, position, size, openSize, otherSide, spawnPosition, objManager);
                case "puzzle":
                    return new PuzzleDoor(sprite, position, size, openSize, otherSide, spawnPosition, objManager);
                default:
                    break;
            }

            return null;

        }

    }
}
