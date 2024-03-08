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
    internal class HiddenDoor: IDoor, ICollidable
    {
        ISprite sprite;
        Vector2 position;
        Rectangle bounds;
        int otherSide;
        bool isOpen;

        GameObjectManager objManager;

        public Rectangle BoundingBox => bounds;

        public CollisionTypes[] CollisionType {
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

        public HiddenDoor(ISprite sprite, Vector2 position, Vector2 size, int otherSide, GameObjectManager objManager)
        {
            this.sprite = sprite;
            this.position = position;
            bounds = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            this.otherSide = otherSide;
            isOpen = false;
            this.objManager = objManager;
        }

        public void SwitchRoom()
        {
            if(otherSide >= 0)
                objManager.SwitchRoom(otherSide);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public Rectangle GetBoundingBox()
        {
            return bounds;
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        // Returns index in Level's room array of the Room this leads to
        public int GetAdjacentRoomIndex()
        {
            return otherSide;
        }
    }
}
