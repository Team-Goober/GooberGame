using Microsoft.Xna.Framework;

namespace Sprint.Door
{
    internal class Stair : Door
    {
        public Stair(Vector2 position, Vector2 size, Point roomIndices, Vector2 spawnPosition, DungeonState dungeon) : 
            base(null, true, position, size, size, Directions.STILL, roomIndices, spawnPosition, dungeon) {
        
        }

        public override void SetOpen(bool open)
        {
            isOpen = open;
        }

    }
}
