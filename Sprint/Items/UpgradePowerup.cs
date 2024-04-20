using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Win32;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint.Items
{
    internal class UpgradePowerup : IUpgradePowerup, IStackedPowerup
    {

        /*
         *  Represents a powerup that acts as a decorator on another powerup to alter its functionality
         */


        private ISprite sprite;
        private IUpgradeEffect onApply;
        private string label;
        private IPowerup basePowerup; // Base to decorate
        private List<string> baseOptions; // Bases that upgrade can apply to
        private Player player;
        private string description;

        private TimeSpan lastUpdate;


        public UpgradePowerup(ISprite sprite, IEffect onApply, string label, string description)
        {
            this.sprite = sprite;
            this.onApply = (IUpgradeEffect)onApply;
            this.label = label;
            this.description = description;
        }

        public bool CanPickup(Inventory inventory)
        {
            // Can only pick up if an ability box can take it
            return getApplicablePowerup(inventory) != null;
        }

        public void Apply(Player player)
        {
            this.player = player;
            if(basePowerup != null)
            {
                if (onApply != null)
                {
                    // Run command if able to
                    onApply.Execute(player);
                }
                else
                {
                    // If no command, just do base directly
                    basePowerup.Apply(player);
                }
            }
            else
            {
                // Must apply this to an existing powerup
                // Shouldn't run the apply of base, just add a decorator
                Inventory inv = player.GetInventory();
                // Find the first box that can have this upgrade applied
                basePowerup = getApplicablePowerup(inv);

                // Set sprite to show a badge frame instead of the item frame
                sprite.SetAnimation("badge");

                // Replace the box's ability with this upgrade as a decorator
                inv.ReplaceWithDecorator(basePowerup.GetLabel(), this);
                // Set base ability that effect applies to
                onApply.SetBase(basePowerup);
            }

        }

        public void Undo(Player player)
        {
            // Pass to base
            basePowerup?.Undo(player);
        }

        private IPowerup getApplicablePowerup(Inventory inventory)
        {
            // Test each base option in order
            for(int i=0; i<baseOptions.Count; i++)
            {
                string label = baseOptions[i];
                // Continue if base is found in inventory
                if (inventory.HasPowerup(label))
                {
                    IPowerup potential = inventory.GetPowerup(label);
                    IUpgradePowerup upgradePotential = potential as IUpgradePowerup;
                    // Only select this one if either it isn't an upgrade, or if this upgrade isn't already in its upgrade chain
                    if(upgradePotential == null || upgradePotential.FindInChain(label) == null)
                    {
                        return potential;
                    }
                }
            }
            // No applicable base found; return null
            return null;
        }

        public string GetLabel()
        {
            // Return true label if no base, otherwise return base's label
            return (basePowerup == null) ? label : basePowerup.GetLabel();
        }

        public string GetTrueLabel()
        {
            return label;
        }

        public string GetDescription()
        {
            // Append modification text to end of base's description
            return basePowerup?.GetDescription() + "|" + description;
        }

        public IEffect GetEffect()
        {
            return basePowerup?.GetEffect();
        }

        public IEffect GetTrueEffect()
        {
            return onApply;
        }

        public void SetUpgradeOptions(List<string> bases)
        {
            baseOptions = bases;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            // Draw base
            basePowerup?.Draw(spriteBatch, position, gameTime);
            // Draw upgrade icons on top
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            // Only update if haven't already updated on this cycle
            if (gameTime.TotalGameTime != lastUpdate)
            {
                sprite.Update(gameTime);
                // Update the base
                basePowerup?.Update(gameTime);
                
            }
            lastUpdate = gameTime.TotalGameTime;
        }

        public IPowerup FindInChain(string label)
        {
            // Case where this top upgrade matches the searched label
            if (this.label == label)
            {
                return this;
            }
            // Case where this upgrade is the end of the chain and isn't the target
            else if (basePowerup == null)
            {
                return null;
            }
            // Case where base is also an upgrade and the one above it isn't the target
            else if(basePowerup is IUpgradePowerup)
            {
                return ((IUpgradePowerup)basePowerup).FindInChain(label);
            }
            // Case where base is the last possible location of the target
            else
            {
                return (basePowerup.GetLabel() == label) ? basePowerup : null;
            }
        }

        public void AddAmount(int amount)
        {
            // Defer to base if it is stackable
            IStackedPowerup stackBase = basePowerup as IStackedPowerup;
            stackBase?.AddAmount(amount);
        }

        public int Quantity()
        {
            // Defer to base if it is stackable
            IStackedPowerup stackBase = basePowerup as IStackedPowerup;
            return (stackBase == null) ? 1 : stackBase.Quantity();
        }

        public bool ReadyConsume(int amount)
        {
            // Defer to base if it is stackable
            IStackedPowerup stackBase = basePowerup as IStackedPowerup;
            return (stackBase == null) ? false : stackBase.ReadyConsume(amount);
        }

        public void SetUnlimited(bool unlimited)
        {
            // Defer to base if it is stackable
            IStackedPowerup stackBase = basePowerup as IStackedPowerup;
            stackBase?.SetUnlimited(unlimited);
        }

        public bool GetUnlimited()
        {
            // Defer to base if it is stackable
            IStackedPowerup stackBase = basePowerup as IStackedPowerup;
            return (stackBase == null) ? false : stackBase.GetUnlimited();
        }

    }
}
