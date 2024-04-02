using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Collision;
using Sprint.Levels;

namespace Sprint.Door
{
    internal class LockDoor : Door
    {

        public override CollisionTypes[] CollisionType
        {
            get
            {
                if (isOpen)
                {
                    return new CollisionTypes[] { CollisionTypes.OPEN_DOOR, CollisionTypes.DOOR };
                }
                else
                {
                    return new CollisionTypes[] { CollisionTypes.LOCKED_DOOR, CollisionTypes.CLOSED_DOOR, CollisionTypes.DOOR };
                }
            }
        }

        public LockDoor(ISprite sprite, Vector2 position, Vector2 size, Vector2 openSize, Vector2 sideOfRoom, Point roomIndices, Vector2 spawnPosition, DungeonState dungeon) :
            base(sprite, false, position, size, openSize, sideOfRoom, roomIndices, spawnPosition, dungeon)
        {

        }
    }
}
