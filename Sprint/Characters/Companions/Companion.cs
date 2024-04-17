
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
        private Vector2 offset; // Current position offset from the player
        private Vector2 stride; // Max X and Y to reach during cycle
        private Room room;
        private bool disable;
        private Timer loopTimer;

        public Companion(ISprite sprite, Player player)
        {
            this.sprite = sprite;
            this.player = player;
            Random random = new Random();
            offset = new Vector2((float)(300 * (random.NextDouble() - 0.5)), (float)(300 * (random.NextDouble() - 0.5)));
            stride = new Vector2(100, 50);
            loopTimer = new Timer(6);
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
                // Start moving
                loopTimer.Start();
            }
            else
            {
                // Remove from current room
                moveToRoom(null);
                player.OnPlayerRoomChange -= moveToRoom;
                // Stop moving
                loopTimer.End();
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
            loopTimer.Update(gameTime);
            if (loopTimer.JustEnded)
            {
                loopTimer.Start();
            }

            // Calculate how far through the cycle to be
            float cycle = (float)(loopTimer.TimeLeft / loopTimer.Duration);
            float spot = (cycle % 2);
            // Set position to follow figure 8
            offset = new Vector2((float)Math.Cos(2 * Math.PI * spot) * stride.X, (float)Math.Sin(4 * Math.PI * spot) * stride.Y);
        }
    }
}
