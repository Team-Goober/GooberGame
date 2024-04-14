using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
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
        private string description;

        private TimeSpan lastUpdate;


        public UpgradeAbility(ISprite sprite, IUpgradeEffect onActivate, string label, string description)
        {
            this.sprite = sprite;
            this.onActivate = onActivate;
            this.label = label;
            this.description = description;
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
            onActivate.SetBase(baseAbility);
        }

        public bool CanPickup(Inventory inventory)
        {
            return baseOptions != null && inventory.GetSelectionB() != null && baseOptions.Contains(inventory.GetSelectionB().GetLabel());
        }

        public string GetLabel()
        {
            return (baseAbility == null) ? label : baseAbility.GetLabel();
        }

        public string GetDescription()
        {
            return baseAbility.GetDescription() + "|" + description;
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
            if (gameTime.TotalGameTime != lastUpdate)
            {
                sprite.Update(gameTime);
                baseAbility?.Update(gameTime);
            }
            lastUpdate = gameTime.TotalGameTime;
        }
    }
}
