using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    internal class UpgradeAbility : IAbility, IUpgradePowerup, IStackedPowerup, ICooldownPowerup
    {

        /*
         *  Represents an ability that acts as a decorator on another ability to alter its functionality
         */


        private ISprite sprite;
        private IUpgradeEffect onActivate;
        private string label;
        private IAbility baseAbility; // Base to decorate
        private List<string> baseOptions; // Bases that upgrade can apply to
        private Player player;
        private string description;

        private TimeSpan lastUpdate;


        public UpgradeAbility(ISprite sprite, IEffect onActivate, string label, string description)
        {
            this.sprite = sprite;
            this.onActivate = (IUpgradeEffect)onActivate;
            this.label = label;
            this.description = description;
        }

        public bool CanPickup(Inventory inventory)
        {
            // Can only pick up if an ability box can take it
            return getApplicableBox(inventory) >= 0;
        }

        public void Apply(Player player)
        {
            this.player = player;
            Inventory inv = player.GetInventory();
            // Find the first box that can have this upgrade applied
            int b = getApplicableBox(inv);
            // If applied, a box must have been found
            Debug.Assert(b >= 0);

            baseAbility = inv.GetSelection(b);
            // Replace the box's ability with this upgrade as a decorator
            inv.ReplaceWithDecorator(baseAbility.GetLabel(), this);
            // Set base ability that effect applies to
            onActivate.SetBase(baseAbility);
        }

        public bool ReadyUp()
        {
            // Defer readyup to base
            return baseAbility.ReadyUp();
        }

        public void Activate()
        {
            if(onActivate != null)
            {
                // Run command if able to
                onActivate.Execute(player);
            }else if(baseAbility != null)
            {
                // If no command, just do base directly
                baseAbility.Activate();
            }
        }

        public void Complete()
        {
            // Defer to base
            baseAbility?.Complete();
        }

        public bool IsActive()
        {
            // Defer to base
            if(baseAbility == null)
            {
                return false;
            }
            else
            {
                return baseAbility.IsActive();
            }
        }

        public void Undo(Player player)
        {
            // Defer to base
            baseAbility?.Undo(player);
        }

        private int getApplicableBox(Inventory inventory)
        {
            // Test each box in order
            for(int b = 0; b < CharacterConstants.SELECT_BOXES; b++)
            {
                IPowerup inBox = inventory.GetSelection(b);
                // First check that box is filled and that it has a base that is able to be upgraded by this
                if(inBox != null && baseOptions.Contains(inBox.GetLabel()))
                {
                    IUpgradePowerup upgradeBox = inBox as IUpgradePowerup;
                    // Check if the ability is either a base without upgrades, or if this upgrade is not already in the upgrade chain
                    if (upgradeBox == null || upgradeBox.FindInChain(label) == null)
                    {
                        // Success; choose this box
                        return b;
                    }
                }
            }
            // Not applicable to any box
            return -1;
        }

        public string GetLabel()
        {
            // Return true label if no base, otherwise return base's label
            return (baseAbility == null) ? label : baseAbility.GetLabel();
        }

        public string GetTrueLabel()
        {
            return label;
        }

        public string GetDescription()
        {
            // Append modification text to end of base's description
            return baseAbility.GetDescription() + "|" + description;
        }

        public void SetUpgradeOptions(List<string> bases)
        {
            baseOptions = bases;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            // Draw base
            baseAbility?.Draw(spriteBatch, position, gameTime);
            // Draw upgrade icons on top
            sprite.Draw(spriteBatch, position, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            // Only update if haven't already updated on this cycle
            if (gameTime.TotalGameTime != lastUpdate)
            {
                // If cooldown ended but item is still active, reactivate it before it reactivates itself
                if (baseAbility is ICooldownPowerup && GetTimeLeft() == 0 && IsActive())
                {
                    // End last activation
                    Complete();
                    // Try to prepare item; only continue if succeeds
                    if (ReadyUp())
                    {
                        // Reactivate item
                        Activate();
                    }
                }

                sprite.Update(gameTime);
                // Update the base
                baseAbility?.Update(gameTime);
                
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
            else if (baseAbility == null)
            {
                return null;
            }
            // Case where base is also an upgrade and the one above it isn't the target
            else if(baseAbility is IUpgradePowerup)
            {
                return ((IUpgradePowerup)baseAbility).FindInChain(label);
            }
            // Case where base is the last possible location of the target
            else
            {
                return (baseAbility.GetLabel() == label) ? baseAbility : null;
            }
        }

        public void AddAmount(int amount)
        {
            // Defer to base if it is stackable
            IStackedPowerup stackBase = baseAbility as IStackedPowerup;
            stackBase?.AddAmount(amount);
        }

        public int Quantity()
        {
            // Defer to base if it is stackable
            IStackedPowerup stackBase = baseAbility as IStackedPowerup;
            return (stackBase == null) ? 1 : stackBase.Quantity();
        }

        public void SetDuration(double duration)
        {
            // Defer to base if it has cooldown
            ICooldownPowerup cooldownBase = baseAbility as ICooldownPowerup;
            cooldownBase?.SetDuration(duration);
        }

        public void SetTimeLeft(double duration)
        {
            // Defer to base if it has cooldown
            ICooldownPowerup cooldownBase = baseAbility as ICooldownPowerup;
            cooldownBase?.SetTimeLeft(duration);
        }

        public float GetDuration()
        {
            // Defer to base if it has cooldown
            ICooldownPowerup cooldownBase = baseAbility as ICooldownPowerup;
            return (cooldownBase == null) ? 1 : cooldownBase.GetDuration();
        }

        public float GetTimeLeft()
        {
            // Defer to base if it has cooldown
            ICooldownPowerup cooldownBase = baseAbility as ICooldownPowerup;
            return (cooldownBase == null) ? 0 : cooldownBase.GetTimeLeft();
        }
    }
}
