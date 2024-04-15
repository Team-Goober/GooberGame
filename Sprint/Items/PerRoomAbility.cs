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
    internal class PerRoomAbility : IAbility
    {

        /*
         *  Represents an ability which can only be used once before changing rooms
         */

        private ISprite sprite;
        private IEffect onActivate;
        private string label;
        private Player player;
        private string description;
        private Room lastRoom; // The last room that it was used in

        private TimeSpan lastUpdate;

        public PerRoomAbility(ISprite sprite, IEffect onActivate, string label, string description)
        {
            this.sprite = sprite;
            this.label = label;
            this.onActivate = onActivate;
            this.description = description;
        }


        public bool ReadyUp()
        {
            Room newRoom = player.GetCurrentRoom();
            bool same = newRoom == lastRoom;
            lastRoom = newRoom;
            // Only allow if this wasn't already used in the room
            return !same;
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
            // Draw overlay to darken sprite if already used in room
            if (player != null && lastRoom == player.GetCurrentRoom())
            {
                Texture2D overlayColor;
                overlayColor = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                overlayColor.SetData(new Color[] { new Color(Color.Black, CharacterConstants.DISABLED_OPACITY) });
                int side = CharacterConstants.POWERUP_SIDE_LENGTH;
                spriteBatch.Draw(overlayColor, new Rectangle((int)(position.X - side / 2.0f), (int)(position.Y - side / 2.0f), side, side), Color.White);
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
                sprite.Update(gameTime);
            }
            lastUpdate = gameTime.TotalGameTime;
        }
    }
}
