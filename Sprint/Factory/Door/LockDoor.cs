using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Collision;
using Sprint.Levels;

namespace Sprint.Factory.Door
{
    internal class LockDoor: Door
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

        public LockDoor(ISprite sprite, Vector2 position, Vector2 size, Vector2 openSize, int otherSide, Vector2 spawnPosition, DungeonState dungeon) :
            base(sprite, false, position, size, openSize, otherSide, spawnPosition, dungeon)
        {

        }
    }
}
