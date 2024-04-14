﻿using Microsoft.Xna.Framework;
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

        public bool ReadyUp()
        {
            return baseAbility.ReadyUp();
        }

        public void Activate()
        {
            if(onActivate != null)
            {
                onActivate.Execute(player);
            }else if(baseAbility != null)
            {
                baseAbility.Activate();
            }
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
            if(baseOptions != null && inventory.GetSelectionB() != null && baseOptions.Contains(inventory.GetSelectionB().GetLabel()))
            {
                IUpgradePowerup upgradeB = inventory.GetSelectionB() as IUpgradePowerup;
                if (upgradeB != null)
                {
                    return upgradeB.FindInChain(label) == null;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        public string GetLabel()
        {
            return (baseAbility == null) ? label : baseAbility.GetLabel();
        }

        public string GetTrueLabel()
        {
            return label;
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

        public IPowerup FindInChain(string label)
        {
            if (this.label == label)
            {
                return this;
            }
            else if (baseAbility == null)
            {
                return null;
            }
            else if(baseAbility is IUpgradePowerup)
            {
                return ((IUpgradePowerup)baseAbility).FindInChain(label);
            }
            else
            {
                return (baseAbility.GetLabel() == label) ? baseAbility : null;
            }
        }

        public void AddAmount(int amount)
        {
            IStackedPowerup stackBase = baseAbility as IStackedPowerup;
            stackBase?.AddAmount(amount);
        }

        public int Quantity()
        {
            IStackedPowerup stackBase = baseAbility as IStackedPowerup;
            return (stackBase == null) ? 1 : stackBase.Quantity();
        }

        public void SetDuration(float duration)
        {
            ICooldownPowerup cooldownBase = baseAbility as ICooldownPowerup;
            cooldownBase?.SetDuration(duration);
        }

        public void SetTimeLeft(float duration)
        {
            ICooldownPowerup cooldownBase = baseAbility as ICooldownPowerup;
            cooldownBase?.SetTimeLeft(duration);
        }

        public float GetTimeLeft()
        {
            ICooldownPowerup cooldownBase = baseAbility as ICooldownPowerup;
            return (cooldownBase == null) ? 0 : cooldownBase.GetTimeLeft();
        }
    }
}
