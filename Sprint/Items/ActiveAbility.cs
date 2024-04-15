using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
using Sprint.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Items
{
    internal class ActiveAbility : IAbility
    {

        /*
         *  Represents an ability which must be kept in a slot and can be selected and used.
         */

        private ISprite sprite;
        private IEffect onActivate;
        private string label;
        private Player player;
        private string description;
        private bool active;

        private TimeSpan lastUpdate;

        public ActiveAbility(ISprite sprite, IEffect onActivate, string label, string description)
        {
            this.sprite = sprite;
            this.label = label;
            this.onActivate = onActivate;
            this.description = description;
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
            return !IsActive();
        }

        public void Activate()
        {
            // Run behavior command
            onActivate?.Execute(player);
            if (onActivate != null)
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
            if(IsActive())
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
                sprite.Update(gameTime);
            }
            lastUpdate = gameTime.TotalGameTime;
        }
    }
}
