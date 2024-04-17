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
            // TODO: replace this with XML loading
            Item it = null;
            switch (name)
            {
                case "heart":
                    it = (new Item(position,
                        new InstantPowerup(
                            spriteLoader.BuildSprite(ANIM_FILE, "heart"),
                            new HealPlayerEffect(1),
                            "heart",
                            "HEART|heals one heart"), 
                        0));
                    break;
                case "heartPiece":
                    it = (new Item(position,
                        new InstantPowerup(
                            spriteLoader.BuildSprite(ANIM_FILE, "heartPiece"),
                            new AddHeartEffect(),
                            "heartPiece",
                            "HEART PIECE|increase max health"),
                        0));
                    break;
                case "compass":
                    it = (new Item(position,
                        new PassivePowerup(
                            spriteLoader.BuildSprite(ANIM_FILE, "compass"),
                            new CompassEffect(),
                            "compass",
                            "COMPASS|reveal triforce|room"),
                        0));
                    break;
                case "map":
                    it = (new Item(position,
                        new PassivePowerup(
                            spriteLoader.BuildSprite(ANIM_FILE, "map"),
                            new MapEffect(),
                            "map",
                            "MAP|reveal dungeon|layout"),
                        0));
                    break;
                case "redRing":
                    it = (new Item(position,
                        new PassivePowerup(
                            spriteLoader.BuildSprite(ANIM_FILE, "redRing"),
                            new ChangeSpeedEffect(CharacterConstants.PLAYER_SPEED),
                            "redRing",
                            "RING|doubles run speed"),
                        0));
                    break;
                case "rupee":
                    IStackedPowerup gem = new ResourcePowerup(
                            spriteLoader.BuildSprite(ANIM_FILE, "rupee"),
                            null,
                            "rupee",
                            "RUPEE|can be traded");
                    gem.AddAmount(5);
                    it = (new Item(position, gem, 0));
                    break;
                case "key":
                    IStackedPowerup key = new ResourcePowerup(
                            spriteLoader.BuildSprite(ANIM_FILE, "key"),
                            null,
                            "key",
                            "KEY|unlocks doors");
                    key.AddAmount(1);
                    it = (new Item(position, key, 0));
                    break;
                case "triforce":
                    IStackedPowerup triforce = new ResourcePowerup(
                            spriteLoader.BuildSprite(ANIM_FILE, "triforce"),
                            new WinEffect(),
                            "triforce",
                            "TRIFORCE|saves hyrule");
                    triforce.AddAmount(1);
                    it = (new Item(position, triforce, 0));
                    break;
                case "shield":
                    IAbility shield = new ActiveAbility(
                            spriteLoader.BuildSprite(ANIM_FILE, "shield"),
                            new ShieldEffect(),
                            "shield",
                            "SHIELD|protects player");
                    it = (new Item(position, shield, 0));
                    break;
                case "sword":
                    ICooldownPowerup sword = new CooldownAbility(
                            spriteLoader.BuildSprite(ANIM_FILE, "sword"),
                            new MeleeEffect(0.5f),
                            "sword",
                            "SWORD|melee attack");
                    sword.SetDuration(0.75);
                    it = (new Item(position, sword, 0));
                    break;
                case "masterSword":
                    ICooldownPowerup masterSword = new CooldownAbility(
                            spriteLoader.BuildSprite(ANIM_FILE, "masterSword"),
                            new MeleeEffect(3),
                            "masterSword",
                            "MASTER SWORD|strong melee attack");
                    masterSword.SetDuration(0.5);
                    it = (new Item(position, masterSword, 20));
                    break;
                case "bow":
                    ICooldownPowerup bow = new CooldownAbility(
                            spriteLoader.BuildSprite(ANIM_FILE, "bow"),
                            new SpawnProjectileEffect("arrow"),
                            "bow",
                            "BOW|shoots arrows");
                    bow.SetDuration(1);
                    it = (new Item(position, bow, 0));
                    break;
                case "boomerang":
                    ICooldownPowerup boomerang = new CooldownAbility(
                            spriteLoader.BuildSprite(ANIM_FILE, "boomerang"),
                            new SpawnProjectileEffect("boomerang"),
                            "boomerang",
                            "BOOMERANG|throw boomerang");
                    boomerang.SetDuration(1);
                    it = (new Item(position, boomerang, 0));
                    break;
                case "bomb":
                    IStackedPowerup bomb = (new ConsumableAbility(
                            spriteLoader.BuildSprite(ANIM_FILE, "bomb"),
                            new SpawnProjectileEffect("bomb"),
                            "bomb",
                            "BOMB|drops an explosive"));
                    bomb.AddAmount(3);
                    it = (new Item(position, bomb, 0));
                    break;
                case "meat":
                    IStackedPowerup meat = new ConsumableAbility(
                            spriteLoader.BuildSprite(ANIM_FILE, "meat"),
                            new HealPlayerEffect(2),
                            "meat",
                            "MEAT|heals 2 hearts");
                    meat.AddAmount(2);
                    it = (new Item(position, meat, 5));
                    break;
                case "redCandle":
                    IAbility candle = (new PerRoomAbility(
                            spriteLoader.BuildSprite(ANIM_FILE, "redCandle"),
                            new SpawnProjectileEffect("fireBall"),
                            "candle",
                            "CANDLE|make fire|once per room"));
                    it = (new Item(position, candle, 5));
                    break;
                case "greenBadge":
                    IUpgradePowerup greenUpgrade = new UpgradeAbility(
                            spriteLoader.BuildSprite(ANIM_FILE, "greenBadge"),
                            new DualShotUpgrade(),
                            "greenBadge",
                            "- dual shot");
                    greenUpgrade.SetUpgradeOptions(new() { "bow", "bomb", "candle", "boomerang" });
                    it = (new Item(position, greenUpgrade, 2));
                    break;
                case "blueBadge":
                    IUpgradePowerup blueUpgrade = new UpgradeAbility(
                            spriteLoader.BuildSprite(ANIM_FILE, "blueBadge"),
                            new TripleShotUpgrade(),
                            "blueBadge",
                            "- triple shot");
                    blueUpgrade.SetUpgradeOptions(new() { "bow", "bomb", "candle", "boomerang"});
                    it = (new Item(position, blueUpgrade, 2));
                    break;
                case "yellowBadge":
                    IUpgradePowerup yellowUpgrade = new UpgradeAbility(
                            spriteLoader.BuildSprite(ANIM_FILE, "yellowBadge"),
                            new QuickCooldownUpgrade(),
                            "yellowBadge",
                            "- quick reload");
                    yellowUpgrade.SetUpgradeOptions(new() { "bow", "sword", "masterSword", "boomerang" });
                    it = (new Item(position, yellowUpgrade, 2));
                    break;
                case "pinkBadge":
                    IUpgradePowerup pinkUpgrade = new UpgradeAbility(
                            spriteLoader.BuildSprite(ANIM_FILE, "pinkBadge"),
                            new InfiniteAmmoUpgrade(),
                            "pinkBadge",
                            "- infinite ammo");
                    pinkUpgrade.SetUpgradeOptions(new() { "bomb", "meat" });
                    it = (new Item(position, pinkUpgrade, 2));
                    break;
                case "blueArrow":
                    IUpgradePowerup blueArrow = new UpgradeAbility(
                            spriteLoader.BuildSprite(ANIM_FILE, "blueArrow"),
                            new UpgradeProjectileUpgrade("arrow"),
                            "blueArrow",
                            "- silver tip");
                    blueArrow.SetUpgradeOptions(new() { "bow" });
                    it = (new Item(position, blueArrow, 5));
                    break;
                case "blueBoomerang":
                    IUpgradePowerup blueBoomerang = new UpgradeAbility(
                            spriteLoader.BuildSprite(ANIM_FILE, "blueBoomerang"),
                            new UpgradeProjectileUpgrade("boomerang"),
                            "blueBoomerang",
                            "- silver tip");
                    blueBoomerang.SetUpgradeOptions(new() { "boomerang" });
                    it = (new Item(position, blueBoomerang, 5));
                    break;
            }
            return it;
        }
    }
}