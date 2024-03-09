using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Levels;

namespace Sprint.Factory.Door
{
    internal class PuzzleDoor : Door
    {
        public PuzzleDoor(ISprite sprite, Vector2 position, Vector2 size, Vector2 openSize, int otherSide, Vector2 spawnPosition, GameObjectManager objManager) :
            base(sprite, false, position, size, openSize, otherSide, spawnPosition, objManager)
        {

        }
    }
}
