using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint.Characters;
using Sprint.Interfaces.Powerups;
using Sprint.Items.Effects;
using Sprint.Sprite;

namespace Sprint.Items
{
    internal class ItemFactory
    {
        private const string ANIM_FILE = "itemAnims";
        private SpriteLoader spriteLoader;
        public ItemFactory(SpriteLoader spriteLoader)
        {
            this.spriteLoader = spriteLoader;
        }

        /// <summary>
        /// Builds item from string name
        /// </summary>
        /// <param name="name">Name of item to make</param>
        /// <param name="position">World position to spawn at</param>
        /// <returns></returns>
        public Item MakeItem(string name, Vector2 position)
        {
            Item it = null;
            switch (name)
            {
                case "heart":
                    it = (new Item(position,
                        new InstantPowerup(
                            spriteLoader.BuildSprite(ANIM_FILE, "heart"),
                            new HealPlayerEffect(1),
                            "heart",
                            "HEART|heals one heart")));
                    break;
                case "redRing":
                    it = (new Item(position,
                        new PassivePowerup(
                            spriteLoader.BuildSprite(ANIM_FILE, "redRing"),
                            new ChangeSpeedEffect(CharacterConstants.PLAYER_SPEED * 2),
                            "redRing",
                            "RING|doubles run speed")));
                    break;
                case "rupee":
                    IStackedPowerup gem = new ResourcePowerup(
                            spriteLoader.BuildSprite(ANIM_FILE, "rupee"),
                            null,
                            "rupee",
                            "RUPEE|can be traded");
                    gem.AddAmount(5);
                    it = (new Item(position, gem));
                    break;
                case "key":
                    IStackedPowerup key = new ResourcePowerup(
                            spriteLoader.BuildSprite(ANIM_FILE, "key"),
                            null,
                            "key",
                            "KEY|unlocks doors");
                    key.AddAmount(1);
                    it = (new Item(position, key));
                    break;
                case "bow":
                    it = (new Item(position,
                        new CooldownAbility(
                            spriteLoader.BuildSprite(ANIM_FILE, "bow"),
                            new SpawnProjectileEffect("arrow"),
                            1,
                            "bow",
                            "BOW|shoots arrows")));
                    break;
                case "meat":
                    IStackedPowerup meat = new ConsumableAbility(
                            spriteLoader.BuildSprite(ANIM_FILE, "meat"),
                            new HealPlayerEffect(2),
                            "meat",
                            "MEAT|heals 2 hearts");
                    meat.AddAmount(2);
                    it = (new Item(position, meat));
                    break;
                case "greenBadge":
                    IUpgradePowerup greenUpgrade = new UpgradeAbility(
                            spriteLoader.BuildSprite(ANIM_FILE, "greenBadge"),
                            new DualShotUpgrade(),
                            "greenBadge",
                            "- dual shot");
                    greenUpgrade.SetUpgradeOptions(new() { "bow" });
                    it = (new Item(position, greenUpgrade));
                    break;
                case "blueBadge":
                    IUpgradePowerup blueUpgrade = new UpgradeAbility(
                            spriteLoader.BuildSprite(ANIM_FILE, "blueBadge"),
                            new TripleShotUpgrade(),
                            "blueBadge",
                            "- triple shot");
                    blueUpgrade.SetUpgradeOptions(new() { "bow" });
                    it = (new Item(position, blueUpgrade));
                    break;
            }
            return it;
        }
    }
}