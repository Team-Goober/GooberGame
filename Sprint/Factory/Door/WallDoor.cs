using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Collision;
using Sprint.Levels;

namespace Sprint.Factory.Door
{
    internal class WallDoor: Door
    {
        
        public WallDoor(ISprite sprite, Vector2 position, Vector2 size, GameObjectManager objManager) :
            base(sprite, false, position, size, Vector2.Zero, -1, Vector2.Zero, objManager)
        {

        }

    }
}
