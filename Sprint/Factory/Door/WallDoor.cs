using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Collision;
using Sprint.Levels;

namespace Sprint.Factory.Door
{
    internal class WallDoor: Door
    {
        
        public WallDoor(ISprite sprite, Vector2 position, Vector2 size, Vector2 openSize, int otherSide, GameObjectManager objManager) :
            base(sprite, false, position, size, openSize, otherSide, objManager)
        {

        }

    }
}
