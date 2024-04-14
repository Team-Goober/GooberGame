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
            quantity += amount;
            Debug.Assert(quantity >= 0);
            number.SetText( "" + quantity );
        }

        public int Quantity()
        {
            return quantity;
        }

        public void Apply(Player player)
        {
            Inventory inv = player.GetInventory();
            if (inv.HasPowerup(label))
            {
                ((IStackedPowerup)inv.GetPowerup(label)).AddAmount(quantity);
            }   
            else
            {
                inv.AddPowerup(this);
            }
            onApply?.Execute(player);
        }

        public bool CanPickup(Inventory inventory)
        {
            return true;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
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
            return description + "|amt: " + quantity;
        }

        public void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime != lastUpdate)
            {
                sprite.Update(gameTime);
            }
            lastUpdate = gameTime.TotalGameTime;
        }
    }
}
