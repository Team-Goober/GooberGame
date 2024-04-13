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

namespace Sprint.Items
{
    internal class ResourcePowerup : IStackedPowerup
    {

        /*
         *  Represents a powerup with a quantity that can be added or reduced by outside objects
         */

        private ISprite sprite;
        private string label;
        private int quantity;
        private ZeldaText number;

        public ResourcePowerup(ISprite sprite, IEffect command, string label)
        {
            this.sprite = sprite;
            this.label = label;
            quantity = 0;
            number = new ZeldaText("nintendo", new() { "0" }, new Vector2(16, 16), 0.5f, Color.Red, Goober.content);
        }


        public void AddAmount(int amount)
        {
            quantity += amount;
            Debug.Assert(quantity >= 0);
            number.SetText(new() { ""+quantity });
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
                ((ResourcePowerup)inv.GetPowerup(label)).AddAmount(quantity);
            }   
            else
            {
                inv.AddPowerup(this);
            }
        }

        public bool CanPickup(Inventory inventory)
        {
            return true;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
            number.Draw(spriteBatch, position, gameTime);
        }

        public string GetLabel()
        {
            return label;
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }
    }
}
