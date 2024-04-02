using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Collision;
using Sprint.Levels;
using System.Diagnostics;
using Sprint.Characters;

namespace Sprint.Factory.Door
{
    internal abstract class Door: IDoor
    {
        protected ISprite sprite;
        protected Vector2 position;
        protected Rectangle bounds;
        protected Rectangle openBounds;
        protected Point roomIndices;
        protected Vector2 sideOfRoom;
        protected bool isOpen;
        protected Vector2 spawnPosition;

        protected bool queueOpen; //to prevent glitching when hitting a newly opened door

        DungeonState dungeon;
        protected IDoor otherFace;

        // Bounds depends on if this door is open
        public Rectangle BoundingBox => isOpen ? openBounds : bounds;

        public virtual CollisionTypes[] CollisionType
        {
            get
            {
                // Only treat as open door if the next room is valid
                if (isOpen && otherFace != null)
                {
                    return new CollisionTypes[] { CollisionTypes.OPEN_DOOR, CollisionTypes.DOOR };
                }
                else
                {
                    return new CollisionTypes[] { CollisionTypes.CLOSED_DOOR, CollisionTypes.DOOR };
                }
            }
        }

        public Door(ISprite sprite, bool isOpen, Vector2 position, Vector2 size, Vector2 openSize, Vector2 sideOfRoom, Point roomIndices, Vector2 spawnPosition, DungeonState dungeon)
        {
            this.sprite = sprite;
            this.position = position;
            bounds = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            openBounds = new Rectangle((int)(position.X + (size.X - openSize.X)/2), (int)(position.Y + (size.Y - openSize.Y) / 2), 
                (int)openSize.X, (int)openSize.Y);
            this.roomIndices = roomIndices;
            this.sideOfRoom = sideOfRoom;
            SetOpen(isOpen);
            this.dungeon = dungeon;
            this.spawnPosition = spawnPosition;
            queueOpen = false;
        }

        public void SwitchRoom()
        {
            if (otherFace != null)
                dungeon.SwitchRoom(PlayerSpawnPosition(), otherFace.GetRoomIndices(), sideOfRoom);
        }

        public Vector2 PlayerSpawnPosition()
        {
            return spawnPosition;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            if (queueOpen)
            {
                isOpen = true;
                queueOpen = false;
            }
            sprite.Update(gameTime);
        }

        // Returns current room index
        public Point GetRoomIndices()
        {
            return roomIndices;
        }

        public virtual void SetOpen(bool open)
        {
            
            if (open)
            {
                sprite.SetAnimation("open");
                queueOpen = true;
            }
            else
            {
                sprite.SetAnimation("close");
                isOpen = false;
            }
        }

        public IDoor GetOtherFace()
        {
            return otherFace;
        }

        public void SetOtherFace(IDoor other)
        {
            otherFace = other;
        }

        public Vector2 SideOfRoom()
        {
            return sideOfRoom;
        }
    }
}
