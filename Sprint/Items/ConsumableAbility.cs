using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
using Sprint.Levels;
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

        public ConsumableAbility(ISprite sprite, IEffect onActivate, string label)
        {
            this.sprite = sprite;
            this.onActivate = onActivate;
            this.label = label;
            quantity = 0;
            number = new ZeldaText("nintendo", new() { "0" }, new Vector2(16, 16), 0.5f, Color.White, Goober.content);
        }

        public void AddAmount(int amount)
        {
            quantity += amount;
            Debug.Assert(quantity >= 0);
            number.SetText(new() { "" + quantity });
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

        public void ActivateItem()
        {
            if(quantity > 0)
            {
                onActivate.Execute(player);
                AddAmount(-1);
            }
        }

        public bool CanPickup(Inventory inventory)
        {
            return inventory.HasPowerup(label) || inventory.SlotsAvailable();
        }

        public string GetLabel()
        {
            return label;
        }

        public IEffect GetEffect()
        {
            return onActivate;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
            number.Draw(spriteBatch, position, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }
    }
}
