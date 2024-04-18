using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
using Sprint.Items.Effects;
using Sprint.Sprite;
using XMLData;

namespace Sprint.Items
{
    internal class ItemFactory
    {
        private const string ANIM_FILE = "itemAnims"; // File of item sprites
        private const string POWERUP_FILE = "powerups"; // File of powerup data
        private SpriteLoader spriteLoader;
        private readonly Type[] args = new Type[] { typeof(ISprite), typeof(IEffect), typeof(string), typeof(string) };
        private Dictionary<string, PowerupData> catalog; // Data loaded for each powerup
        public ItemFactory(SpriteLoader spriteLoader)
        {
            this.spriteLoader = spriteLoader;
        }

        public void LoadPowerupData()
        {
            catalog = Goober.content.Load<Dictionary<string, PowerupData>>(POWERUP_FILE);
        }


        /// <summary>
        /// Builds powerup from string name
        /// </summary>
        /// <param name="name">Name of powerup for powerup to make</param>
        /// <returns>Newly created powerup</returns>
        public IPowerup MakePowerup(string name)
        {
            // Temporarily handle unimplemented items
            if (!catalog.ContainsKey(name))
            {
                return null;
            }

            // Get data object
            PowerupData pd = catalog[name];
            // Get constructor for base type
            ConstructorInfo constructor = Type.GetType(pd.Type).GetConstructor(args);
            // Create powerup using sprite, cloned effect, and strings
            IPowerup pup = constructor?.Invoke(new object[] { spriteLoader.BuildSprite(ANIM_FILE, pd.Sprite),
                        ((IEffect)pd.Effect)?.Clone(), pd.Label, pd.Description}) as IPowerup;

            // Add quantity if stacked type
            if (pd is StackedPowerupData)
                ((IStackedPowerup)pup).AddAmount(((StackedPowerupData)pd).Quantity);
            // Set duration if cooldown type
            if (pd is CooldownPowerupData)
                ((ICooldownPowerup)pup).SetDuration(((CooldownPowerupData)pd).Duration);
            // Set bases if upgrade type
            if (pd is UpgradePowerupData)
                ((IUpgradePowerup)pup).SetUpgradeOptions(((UpgradePowerupData)pd).Bases.ToList());

            return pup;
        }

        /// <summary>
        /// Builds item from string name
        /// </summary>
        /// <param name="name">Name of powerup for item to make</param>
        /// <param name="position">World position to spawn at</param>
        /// <param name="price">Cost of item when picked up</param>
        /// <returns>Newly created Item</returns>
        public Item MakeItem(string name, Vector2 position, int price)
        {
            IPowerup pup = MakePowerup(name);
            // If powerup doesn't exist, don't make an item
            if(pup == null)
            {
                return null;
            }
            // Create final item using powerup
            Item it = new(position, pup, price);
            return it;
        }
                
    }
}