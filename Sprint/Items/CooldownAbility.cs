using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
using System;
using System.Reflection.Metadata;

namespace Sprint.Items
{
    internal class CooldownAbility : IAbility, ICooldownPowerup
    {

        /*
         *  Represents an ability which must be kept in a slot and can be selected and used.
         */

        private ISprite sprite;
        private IEffect onActivate;
        private string label;
        private Player player;
        private string description;
        private Timer cooldownTimer; // Times duration between allowed uses

        private TimeSpan lastUpdate;

        public CooldownAbility(ISprite sprite, IEffect onActivate, string label, string description)
        {
            this.sprite = sprite;
            this.label = label;
            this.onActivate = onActivate;
            this.description = description;
            cooldownTimer = new Timer(1.0f);
        }

        public bool ReadyUp()
        {
            // Can only use when timer is over or it was just started this cycle
            if (cooldownTimer.Ended || cooldownTimer.TimeLeft == cooldownTimer.Duration)
            {
                // Start cooldown
                cooldownTimer.Start();
                return true;
            }
            return false;
        }

        public void Activate()
        {
            // Run behavior command
            onActivate?.Execute(player);
        }

        public void Apply(Player player)
        {
            this.player = player;
            // Assign ability to slot
            player.GetInventory().AddToSlots(this);
            // Add to full list of items
            player.GetInventory().AddPowerup(this);
        }

        public bool CanPickup(Inventory inventory)
        {
            // Only pickupable if not already in inventory and if there's space in the ability slots
            return !inventory.HasPowerup(label) && inventory.SlotsAvailable();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);

            // Draw dark overlay to show how much time is left in cooldown
            if (!cooldownTimer.Ended)
            {
                Texture2D overlayColor;
                overlayColor = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                overlayColor.SetData(new Color[] { new Color(Color.Black, CharacterConstants.DISABLED_OPACITY) } );
                int side = CharacterConstants.POWERUP_SIDE_LENGTH;
                float height = side * (float)(cooldownTimer.TimeLeft / cooldownTimer.Duration);
                float ypos = position.Y + side / 2.0f - height;
                spriteBatch.Draw(overlayColor, new Rectangle((int)(position.X - side / 2.0f), (int)ypos, side, (int)height), Color.White);
            }
        }
        public string GetLabel()
        {
            return label;
        }


        public string GetDescription()
        {
            return description;
        }

        public void Update(GameTime gameTime)
        {
            // Only update if haven't already updated on this cycle
            if (gameTime.TotalGameTime != lastUpdate)
            {
                cooldownTimer.Update(gameTime);
                sprite.Update(gameTime);
            }
            lastUpdate = gameTime.TotalGameTime;
        }

        public void SetDuration(double duration)
        {
            cooldownTimer.SetDuration(duration);
        }

        public void SetTimeLeft(double duration)
        {
            cooldownTimer.SubtractTime(duration);
        }

        public float GetTimeLeft()
        {
            return (float)cooldownTimer.TimeLeft.TotalSeconds;
        }
    }
}
