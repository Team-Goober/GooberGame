using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
using Sprint.Levels;
using System;
using System.Diagnostics;

namespace Sprint.Items
{
    internal class ConsumableAbility : IAbility, IStackedPowerup
    {

        /*
         *  Represents an ability which must be kept in a slot and can be selected and used.
         */

        private ISprite sprite;
        private IEffect onActivate;
        private string label;
        private int quantity;
        private ZeldaText number;
        private Player player;
        private string description;

        private TimeSpan lastUpdate;

        public ConsumableAbility(ISprite sprite, IEffect onActivate, string label, string description)
        {
            this.sprite = sprite;
            this.onActivate = onActivate;
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
            this.player = player;
            Inventory inv = player.GetInventory();
            if (inv.HasPowerup(label))
            {
                ((IStackedPowerup)inv.GetPowerup(label)).AddAmount(quantity);
            }
            else
            {
                inv.AddToSlots(this);
                inv.AddPowerup(this);
            }
        }

        public bool ReadyUp()
        {
            if (quantity > 0)
            {
                AddAmount(-1);
                return true;
            }
            return false;
        }

        public void Activate()
        {
            onActivate.Execute(player);
        }

        public bool CanPickup(Inventory inventory)
        {
            return inventory.HasPowerup(label) || inventory.SlotsAvailable();
        }

        public string GetLabel()
        {
            return label;
        }

        public string GetDescription()
        {
            return description + "|amt: " + quantity;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
            if(quantity == 0)
            {
                Texture2D overlayColor;
                overlayColor = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                overlayColor.SetData(new Color[] { new Color(Color.Black, CharacterConstants.DISABLED_OPACITY) });
                int side = CharacterConstants.POWERUP_SIDE_LENGTH;
                spriteBatch.Draw(overlayColor, new Rectangle((int)(position.X - side / 2.0f), (int)(position.Y - side / 2.0f), side, side), Color.White);
            }
            number.Draw(spriteBatch, position, gameTime);
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
