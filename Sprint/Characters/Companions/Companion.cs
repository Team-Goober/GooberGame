
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Levels;

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
            offset = new Vector2(100, 100);
            disable = false;
            player.OnPlayerRoomChange += moveToRoom;
        }

        // Sets whether this object should be in a room
        public void SetDisable(bool d)
        {
            disable = d;
            if (!disable)
            {
                // Add to player room
                moveToRoom(player.GetCurrentRoom());
            }
            else
            {
                // Remove from current room
                moveToRoom(null);
            }
        }

        // Moves to scene object manager of new room
        private void moveToRoom(Room r)
        {
            // Don't move if disabled
            if (disable)
                return;
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
