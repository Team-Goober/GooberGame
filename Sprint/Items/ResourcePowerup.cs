using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Sprint.Levels;
using Sprint.Interfaces.Powerups;

namespace Sprint.Items
{
    internal class ResourcePowerup : IStackedPowerup
    {

        /*
         *  Represents a powerup with a quantity that can be added or reduced by outside objects
         */

        private ISprite sprite;
        private IEffect onApply;
        private string label;
        private int quantity;
        private ZeldaText number;
        private string description;

        private TimeSpan lastUpdate;

        public ResourcePowerup(ISprite sprite, IEffect onApply, string label, string description)
        {
            this.sprite = sprite;
            this.onApply = onApply;
            this.label = label;
            quantity = 0;
            number = new ZeldaText("nintendo", new() { "0" }, new Vector2(16, 16), 0.5f, Color.White, Goober.content);
            this.description = description;
        }

        public void AddAmount(int amount)
        {
            // Add an amount to the stack
            quantity += amount;
            // Must not have negative stack size
            Debug.Assert(quantity >= 0);
            number.SetText( "" + quantity );
        }

        public int Quantity()
        {
            return quantity;
        }
        public bool CanPickup(Inventory inventory)
        {
            // Can always pick up a resource
            return true;
        }

        public void Apply(Player player)
        {
            Inventory inv = player.GetInventory();
            // Get the version of this resource in the inventory
            IStackedPowerup ownedVersion = inv.GetPowerup(label) as IStackedPowerup;
            if(ownedVersion == this)
            {
                // This is the version in the inventory; run apply without re-adding
                onApply?.Execute(player);
            }
            else if (ownedVersion != null)
            {
                // Add this stack to existing one if player already has this type
                ownedVersion.AddAmount(quantity);
                // Make it run its effect
                ownedVersion.Apply(player);
            }   
            else
            {
                // If player doesn't have this type, add it to their inventory
                inv.AddPowerup(this);
                onApply?.Execute(player);
            }
        }


        public void Undo(Player player)
        {
            // Remove this powerup from inventory listing
            player.GetInventory().DeletePowerup(this);
            // Undo behavior
            onApply?.Reverse(player);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
            // Draw an overlay to darken sprite if there are none left
            if (quantity == 0)
            {
                Texture2D overlayColor;
                overlayColor = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                overlayColor.SetData(new Color[] { new Color(Color.Black, CharacterConstants.DISABLED_OPACITY) });
                int side = CharacterConstants.POWERUP_SIDE_LENGTH;
                spriteBatch.Draw(overlayColor, new Rectangle((int)(position.X - side / 2.0f), (int)(position.Y - side / 2.0f), side, side), Color.White);
            }
            number.Draw(spriteBatch, position, gameTime);
        }

        public string GetLabel()
        {
            return label;
        }

        public string GetDescription()
        {
            // Add quantity to the item description
            return description + "|amt: " + quantity;
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
