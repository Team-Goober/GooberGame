using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Sprite;
using Sprint.Levels;

namespace Sprint
{
    internal class CycleItem
    {
        private List<Item> items = new List<Item>();
        private int currentItem;
        private Vector2 position;
        private GameObjectManager objManager;
        private const string ANIM_FILE = "itemAnims";

        public CycleItem(Goober game, Vector2 position, GameObjectManager objManager, SpriteLoader spriteLoader)
        {
            this.position = position;
            this.objManager = objManager;

            // Create all items and add to object manager

            ISprite rupyS = spriteLoader.BuildSprite(ANIM_FILE, "rupy");
            Item rupy = new Item(rupyS, this.position, ItemType.Rupee);
            items.Add(rupy);

            ISprite arrowS = spriteLoader.BuildSprite(ANIM_FILE, "arrow");
            Item arrow = new Item( arrowS, this.position, ItemType.Arrow);
            items.Add(arrow);

            ISprite heartS = spriteLoader.BuildSprite(ANIM_FILE, "heart");
            Item heart = new Item( heartS, this.position, ItemType.Heart);
            items.Add(heart);

            ISprite shieldS = spriteLoader.BuildSprite(ANIM_FILE, "shield");
            Item shield = new Item( shieldS, this.position, ItemType.Shield);
            items.Add(shield);

            ISprite triforceS = spriteLoader.BuildSprite(ANIM_FILE, "triforce");
            Item triforce = new Item( triforceS, this.position, ItemType.Triforce);
            items.Add(triforce);

            ISprite specialKeyS = spriteLoader.BuildSprite(ANIM_FILE, "specialKey");
            Item specialKey = new Item( specialKeyS, this.position, ItemType.SpecialKey);
            items.Add(specialKey);

            ISprite clockS = spriteLoader.BuildSprite(ANIM_FILE, "clock");
            Item clock = new Item( clockS, this.position, ItemType.Clock);
            items.Add(clock);

            ISprite bowS = spriteLoader.BuildSprite(ANIM_FILE, "bow");
            Item bow = new Item( bowS, this.position, ItemType.Bow);
            items.Add(bow);

            ISprite ladderS = spriteLoader.BuildSprite(ANIM_FILE, "ladder");
            Item ladder = new Item( ladderS, this.position, ItemType.Ladder);
            items.Add(ladder);

            ISprite fenceS = spriteLoader.BuildSprite(ANIM_FILE, "fence");
            Item fence = new Item( fenceS, this.position, ItemType.Fence);
            items.Add(fence);

            

            ISprite blueArrowS = spriteLoader.BuildSprite(ANIM_FILE, "blueArrow");
            Item blueArrow = new Item( blueArrowS, this.position, ItemType.BlueArrow);
            items.Add(blueArrow);

            ISprite fairyS = spriteLoader.BuildSprite(ANIM_FILE, "fairy");
            Item fairy = new Item( fairyS, this.position, ItemType.Fairy);
            items.Add(fairy);

            ISprite blueOrbS = spriteLoader.BuildSprite(ANIM_FILE, "blueOrb");
            Item blueOrb = new Item( blueOrbS, this.position, ItemType.BlueOrb);
            items.Add(blueOrb);

            ISprite blueRingS = spriteLoader.BuildSprite(ANIM_FILE, "blueRing");
            Item blueRing = new Item( blueRingS, this.position, ItemType.BlueRing);
            items.Add(blueRing);

            ISprite redRingS = spriteLoader.BuildSprite(ANIM_FILE, "redRing");
            Item redRing = new Item( redRingS, this.position, ItemType.RedRing);
            items.Add(redRing);

            ISprite blueBoomerangS = spriteLoader.BuildSprite(ANIM_FILE, "blueBoomerang");
            Item blueBoomerang = new Item( blueBoomerangS, this.position, ItemType.BlueBoomerang);
            items.Add(blueBoomerang);

            ISprite boomerangS = spriteLoader.BuildSprite(ANIM_FILE, "boomerang");
            Item boomerang = new Item( boomerangS, this.position, ItemType.Boomerang);
            items.Add(boomerang);

            ISprite blueHeartS = spriteLoader.BuildSprite(ANIM_FILE, "blueHeart");
            Item blueHeart = new Item( blueHeartS, this.position, ItemType.BlueHeart);
            items.Add(blueHeart);

            ISprite blueTorchS = spriteLoader.BuildSprite(ANIM_FILE, "blueTorch");
            Item blueTorch = new Item( blueTorchS, this.position, ItemType.BlueTorch);
            items.Add(blueTorch);

            ISprite swordS = spriteLoader.BuildSprite(ANIM_FILE, "sword");
            Item sword = new Item( swordS, this.position, ItemType.Sword);
            items.Add(sword);

            ISprite citemS = spriteLoader.BuildSprite(ANIM_FILE, "citem");
            Item citem = new Item( citemS, this.position, ItemType.Citem);
            items.Add(citem);

            ISprite shieldRedS = spriteLoader.BuildSprite(ANIM_FILE, "shield_red");
            Item shieldRed = new Item( shieldRedS, this.position, ItemType.ShieldRed);
            items.Add(shieldRed);

            ISprite potionS = spriteLoader.BuildSprite(ANIM_FILE, "potion");
            Item potion = new Item( potionS, this.position, ItemType.Potion);
            items.Add(potion);

            ISprite cakeS = spriteLoader.BuildSprite(ANIM_FILE, "cake");
            Item cake = new Item( cakeS, this.position, ItemType.Cake);
            items.Add(cake);

            // Select the first item
            SwitchItem(null, items[0]);
        }

        public void Next()
        {
            int before = currentItem;
            currentItem++;

            if(currentItem == items.Count)
            {
                currentItem = 0;
            }
            SwitchItem(items[before], items[currentItem]);
        }

        public void Back()
        {
            int before = currentItem;
            currentItem--;
            if(currentItem == -1)
            {
                currentItem = items.Count - 1;
            }
            SwitchItem(items[before], items[currentItem]);
        }

        public void SwitchItem(Item oldI, Item newI)
        {
            if (oldI != null)
                objManager.Remove(oldI);
            if (newI != null)
                objManager.Add(newI);
        }

    }
}
