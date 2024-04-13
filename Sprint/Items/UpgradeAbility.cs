using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Characters;
using Sprint.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Items
{
    internal class UpgradeAbility : IAbility, IUpgradePowerup
    {
        private ISprite sprite;
        private IUpgradeEffect onActivate;
        private string label;
        private IAbility baseAbility;
        private List<string> baseOptions;
        private Player player;

        public UpgradeAbility(ISprite sprite, IUpgradeEffect onActivate, string label)
        {
            this.sprite = sprite;
            this.onActivate = onActivate;
            this.label = label;
        }

        public void ActivateItem()
        {
            onActivate.Execute(player);
        }

        public void Apply(Player player)
        {
            this.player = player;
            Inventory inv = player.GetInventory();
            Debug.Assert(CanPickup(inv));

            baseAbility = inv.GetSelectionB();
            inv.ReplaceWithDecorator(baseAbility.GetLabel(), this);
            onActivate.SetBase(baseAbility.GetEffect());
        }

        public bool CanPickup(Inventory inventory)
        {
            return baseOptions != null && inventory.GetSelectionB() != null && baseOptions.Contains(inventory.GetSelectionB().GetLabel());
        }

        public string GetLabel()
        {
            return (baseAbility == null) ? label : baseAbility.GetLabel();
        }

        public IEffect GetEffect()
        {
            return onActivate;
        }

        public void SetUpgradeOptions(List<string> bases)
        {
            baseOptions = bases;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            baseAbility?.Draw(spriteBatch, position, gameTime);
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
            baseAbility?.Update(gameTime);
        }
    }
}
