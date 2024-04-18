
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using Sprint.Levels;
using System;

namespace Sprint.Characters.Companions
{
    internal class Companion : IGameObject
    {
        protected ISprite sprite;
        protected Player player;
        protected Vector2 offset; // Current position offset from the player
        protected Vector2 stride; // Max X and Y to reach during cycle
        protected Room room;
        protected bool disable;
        protected Timer loopTimer; // How far through a figure 8 the companion is
        protected Timer axisTimer; // How far through a circular rotation the figure 8 is
        protected bool flipAxisDir;
        protected bool flipLoopDir;

        public Companion(ISprite sprite, Player player, bool flipAxisDir, bool flipLoopDir)
        {
            this.sprite = sprite;
            this.player = player;
            this.flipLoopDir = flipLoopDir;
            this.flipAxisDir = flipAxisDir;
            offset = new Vector2();
            stride = new Vector2(100, 50);
            loopTimer = new Timer(6);
            loopTimer.SetLooping(true);
            axisTimer = new Timer(43);
            axisTimer.SetLooping(true);
        }

        // Sets whether this object should be in a room
        public virtual void SetDisable(bool d)
        {
            disable = d;
            if (!disable)
            {
                // Add to player room
                MoveToRoom(player.GetCurrentRoom());
                player.OnPlayerRoomChange += MoveToRoom;
                // Start moving
                loopTimer.Start();
                axisTimer.Start();
                Random random = new Random();
                axisTimer.SubtractTime(random.NextDouble() * 6);
            }
            else
            {
                // Remove from current room
                MoveToRoom(null);
                player.OnPlayerRoomChange -= MoveToRoom;
                // Stop moving
                loopTimer.End();
                axisTimer.End();
            }
        }

        // Moves to scene object manager of new room
        public virtual void MoveToRoom(Room r)
        {
            room?.GetScene().Remove(this);
            room = r;
            room?.GetScene().Add(this);
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Draw a little bit offset from player
            sprite.Draw(spriteBatch, player.GetPosition() + offset, gameTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            loopTimer.Update(gameTime);

            // Calculate how far through the cycle to be
            float loopCycle = (float)(loopTimer.TimeLeft / loopTimer.Duration);
            if (flipLoopDir)
                loopCycle = 1 - loopCycle;
            float spot = (loopCycle % 2);

            axisTimer.Update(gameTime);

            float axisCycle = (float)(axisTimer.TimeLeft / axisTimer.Duration);
            if (flipAxisDir)
                axisCycle = 1 - axisCycle;

            // Set position to follow figure 8
            Vector2 preRotate = new Vector2((float)Math.Sin(4 * Math.PI * spot) * stride.Y, (float)Math.Cos(2 * Math.PI * spot) * stride.X);

            offset = Vector2.Transform(preRotate, Matrix.CreateRotationZ((float)(Math.PI * 2 * axisCycle)));
        }
    }
}
