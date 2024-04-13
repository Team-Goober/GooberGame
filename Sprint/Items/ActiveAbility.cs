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

        public ActiveAbility(ISprite sprite, IEffect onActivate, string label)
        {
            this.sprite = sprite;
            this.label = label;
            this.onActivate = onActivate;
        }

        public void ActivateItem()
        {
            onActivate.Execute(player);
        }

        public void Apply(Player player)
        {
            this.player = player;
            player.GetInventory().AddToSlots(this);
            player.GetInventory().AddPowerup(this);
        }

        public bool CanPickup(Inventory inventory)
        {
            return !inventory.HasPowerup(label) && inventory.SlotsAvailable();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public string GetLabel()
        {
            return label;
        }

        public IEffect GetEffect()
        {
            return onActivate;
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }
    }
}
