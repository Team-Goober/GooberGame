using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using System.Diagnostics;

namespace Sprint.Door
{
    internal class OpenDoor : Door
    {

        public OpenDoor(ISprite sprite, Vector2 position, Vector2 size, Vector2 openSize, Vector2 sideOfRoom, Point roomIndices, Vector2 spawnPosition, DungeonState dungeon) :
            base(sprite, true, position, size, openSize, sideOfRoom, roomIndices, spawnPosition, dungeon)
        {

        }
        public override void SetOpen(bool open)
        {
            Debug.Assert(open); // cant close this door
            isOpen = open;
        }
    }
}