
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Levels;
using System;
using System.Diagnostics;

namespace Sprint.Characters.Companions
{
    internal class SwordCompanion : Companion
    {
        SwordCollision sword;
        Timer spinTimer;

        public SwordCompanion(ISprite sprite, Player player, bool flipAxisDir, bool flipLoopDir) : base(sprite, player, flipAxisDir, flipLoopDir)
        {
            spinTimer = new Timer(1.7);
            spinTimer.SetLooping(true);
            loopTimer.SetDuration(3.5);
            axisTimer.SetDuration(23);

            stride = new Vector2(150, 75);
            // Width is used isntead of length because the sword is rotating, so we need a square
            sword = new SwordCollision(new(0, 0, CharacterConstants.SWORD_WIDTH, CharacterConstants.SWORD_WIDTH),
                player, CharacterConstants.TINY_DMG);
        }

        public void SetDamage(float dmg)
        {
            sword.SetDamage(dmg);
            // Make the spin go faster for higher damage
            // The -1 /2 +1 should shrink the values towards 1
            float timeDivider = ((dmg / CharacterConstants.TINY_DMG) - 1) / 2 + 1;
            spinTimer.SetDuration(1.7 / timeDivider);
            spinTimer.SetTimeLeft(spinTimer.TimeLeft.TotalSeconds / timeDivider);
        }

        // Sets whether this object should be in a room
        public override void SetDisable(bool d)
        {
            base.SetDisable(d);
            if (d)
            {
                spinTimer.End();
                room?.GetScene().Remove(sword);
            }
            else
            {
                spinTimer.Start();
                room?.GetScene().Add(sword);
            }
        }

        // Moves to scene object manager of new room
        public override void MoveToRoom(Room r)
        {
            // Move sword
            room?.GetScene().Remove(sword);
            base.MoveToRoom(r);
            room?.GetScene().Add(sword);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Draw a little bit offset from player with rotation according to spin
            sprite.Draw(spriteBatch, player.GetPosition() + offset, gameTime, (float)(2 * Math.PI * spinTimer.TimeLeft / spinTimer.Duration));
        }

        public override void Update(GameTime gameTime)
        {
            spinTimer.Update(gameTime);

            // Update sword position
            sword.SetPosition(player.GetPosition() + offset - new Vector2(CharacterConstants.SWORD_WIDTH / 2));

            base.Update(gameTime);
        }

    }
}
