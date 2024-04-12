using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Collision;

namespace Sprint.Door
{
    internal class HiddenDoor : Door
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
                    return new CollisionTypes[] { CollisionTypes.HIDDEN_DOOR, CollisionTypes.CLOSED_DOOR, CollisionTypes.DOOR };
                }
            }
        }

        public HiddenDoor(ISprite sprite, Vector2 position, Vector2 size, Vector2 openSize, Vector2 sideOfRoom, Point roomIndices, Vector2 spawnPosition, DungeonState dungeon) :
            base(sprite, false, position, size, openSize, sideOfRoom, roomIndices, spawnPosition, dungeon)
        {

        }
    }
}
