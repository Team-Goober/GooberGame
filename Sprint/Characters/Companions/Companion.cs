
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Levels;
using System;

namespace Sprint.Characters.Companions
{
    internal class Companion : IGameObject
    {
        private ISprite sprite;
        private Player player;
        private Vector2 offset;
        private Room room;
        private bool disable;

        public Companion(ISprite sprite, Player player)
        {
            this.sprite = sprite;
            this.player = player;
            Random random = new Random();
            offset = new Vector2((float)(200 * (random.NextDouble() - 0.5)), (float)(200 * (random.NextDouble() - 0.5)));
        }

        // Sets whether this object should be in a room
        public void SetDisable(bool d)
        {
            disable = d;
            if (!disable)
            {
                // Add to player room
                moveToRoom(player.GetCurrentRoom());
                player.OnPlayerRoomChange += moveToRoom;
            }
            else
            {
                // Remove from current room
                moveToRoom(null);
                player.OnPlayerRoomChange -= moveToRoom;
            }
        }

        // Moves to scene object manager of new room
        private void moveToRoom(Room r)
        {
            room?.GetScene().Remove(this);
            room = r;
            room?.GetScene().Add(this);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Draw a little bit offset from player
            sprite.Draw(spriteBatch, player.GetPhysic().Position + offset, gameTime);
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
