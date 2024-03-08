using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Collision;
using Sprint.Levels;

namespace Sprint.Factory.Door
{
    internal class OpenDoor : Door
    {

        public OpenDoor(ISprite sprite, Vector2 position, Vector2 size, Vector2 openSize, int otherSide, GameObjectManager objManager) :
            base(sprite, true, position, size, openSize, otherSide, objManager)
        {

        }

    }
}