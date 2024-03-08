using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint.Collision;
using Sprint.Levels;

namespace Sprint.Factory.Door
{
    internal class HiddenDoor: Door
    {

        public override CollisionTypes[] CollisionType {
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

        public HiddenDoor(ISprite sprite, Vector2 position, Vector2 size, Vector2 openSize, int otherSide, GameObjectManager objManager) : 
            base(sprite, false, position, size, openSize, otherSide, objManager)
        {

        }
    }
}
