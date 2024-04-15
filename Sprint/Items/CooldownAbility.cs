using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
using System;
using System.Diagnostics;
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
        private bool active;

        private TimeSpan lastUpdate;

        public CooldownAbility(ISprite sprite, IEffect onActivate, string label, string description)
        {
            this.sprite = sprite;
            this.label = label;
            this.onActivate = onActivate;
            this.description = description;
            cooldownTimer = new Timer(1.0f);
        }
        public bool CanPickup(Inventory inventory)
        {
            // Only pickupable if not already in inventory and if there's space in the ability slots
            return !inventory.HasPowerup(label) && inventory.SlotsAvailable();
        }

        public void Apply(Player player)
        {
            this.player = player;
            // Assign ability to slot
            player.GetInventory().AddToSlots(this);
            // Add to full list of items
            player.GetInventory().AddPowerup(this);
        }

        public bool ReadyUp()
        {
            // Can only use when timer is over or it was just started this cycle
            if ((cooldownTimer.Ended || cooldownTimer.TimeLeft == cooldownTimer.Duration) && !IsActive())
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
            if(onActivate != null)
            {
                active = true;
            }
        }

        public void Complete()
        {
            // Reverse whatever was done by activate
            onActivate?.Reverse(player);
            active = false;
        }

        public bool IsActive()
        {
            return active;
        }

        public void Undo(Player player)
        {
            // End execution if necessary
            if (IsActive())
                Complete();
            this.player = player;
            // Remove from assigned slot
            player.GetInventory().DeleteFromSlots(this);
            // Remove from list of items
            player.GetInventory().DeletePowerup(this);
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
                // If cooldown ended but item is still active, reactivate it
                if (GetTimeLeft() == 0 && IsActive())
                {
                    // End last activation
                    Complete();
                    // Try to prepare item; only continue if succeeds
                    if (ReadyUp())
                    {
                        // Reactivate item
                        Activate();
                    }
                }

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
            cooldownTimer.SetTimeLeft(duration);
        }
        public float GetDuration()
        {
            return (float)cooldownTimer.Duration.TotalSeconds;
        }
        public float GetTimeLeft()
        {
            return (float)cooldownTimer.TimeLeft.TotalSeconds;
        }
    }
}
