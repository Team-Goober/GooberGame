using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Levels;

namespace Sprint.Door
{
    internal class PuzzleDoor : Door
    {
        public PuzzleDoor(ISprite sprite, Vector2 position, Vector2 size, Vector2 openSize, Vector2 sideOfRoom, Point roomIndices, Vector2 spawnPosition, DungeonState dungeon) :
            base(sprite, false, position, size, openSize, sideOfRoom, roomIndices, spawnPosition, dungeon)
        {

        }
    }
}
