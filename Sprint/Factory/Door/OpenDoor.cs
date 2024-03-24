using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Collision;
using Sprint.Levels;
using System.Diagnostics;

namespace Sprint.Factory.Door
{
    internal class OpenDoor : Door
    {

        public OpenDoor(ISprite sprite, Vector2 position, Vector2 size, Vector2 openSize, int otherSide, Vector2 spawnPosition, DungeonState dungeon) :
            base(sprite, true, position, size, openSize, otherSide, spawnPosition, dungeon)
        {

        }
        public override void SetOpen(bool open)
        {
            Debug.Assert(open); // cant close this door
            isOpen = open;
        }
    }
}