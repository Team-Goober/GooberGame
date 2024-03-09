using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
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

            // Create all items and add to game object manager

            ISprite rupyS = spriteLoader.BuildSprite(ANIM_FILE, "rupy");
            Item rupy = new Item(game, rupyS, this.position);
            items.Add(rupy);

            ISprite arrowS = spriteLoader.BuildSprite(ANIM_FILE, "arrow");
            Item arrow = new Item(game, arrowS, this.position);
            items.Add(arrow);

            ISprite heartS = spriteLoader.BuildSprite(ANIM_FILE, "heart");
            Item heart = new Item(game, heartS, this.position);
            items.Add(heart);

            ISprite shieldS = spriteLoader.BuildSprite(ANIM_FILE, "shield");
            Item shield = new Item(game, shieldS, this.position);
            items.Add(shield);

            ISprite triforceS = spriteLoader.BuildSprite(ANIM_FILE, "triforce");
            Item triforce = new Item(game, triforceS, this.position);
            items.Add(triforce);

            ISprite specialKeyS = spriteLoader.BuildSprite(ANIM_FILE, "specialKey");
            Item specialKey = new Item(game, specialKeyS, this.position);
            items.Add(specialKey);

            ISprite clockS = spriteLoader.BuildSprite(ANIM_FILE, "clock");
            Item clockItem = new Item(game, clockS, this.position);
            items.Add(clockItem);

            ISprite bowS = spriteLoader.BuildSprite(ANIM_FILE, "bow");
            Item bowItem = new Item(game, bowS, this.position);
            items.Add(bowItem);

            ISprite ladderS = spriteLoader.BuildSprite(ANIM_FILE, "ladder");
            Item ladderItem = new Item(game, ladderS, this.position);
            items.Add(ladderItem);

            ISprite fenceS = spriteLoader.BuildSprite(ANIM_FILE, "fence");
            Item fenceItem = new Item(game, fenceS, this.position);
            items.Add(fenceItem);

            

            ISprite blueArrowS = spriteLoader.BuildSprite(ANIM_FILE, "blueArrow");
            Item blueArrowItem = new Item(game, blueArrowS, this.position);
            items.Add(blueArrowItem);

            ISprite fairyS = spriteLoader.BuildSprite(ANIM_FILE, "fairy");
            Item fairyItem = new Item(game, fairyS, this.position);
            items.Add(fairyItem);

            ISprite blueOrbS = spriteLoader.BuildSprite(ANIM_FILE, "blueOrb");
            Item blueOrbItem = new Item(game, blueOrbS, this.position);
            items.Add(blueOrbItem);

            ISprite blueRingS = spriteLoader.BuildSprite(ANIM_FILE, "blueRing");
            Item blueRingItem = new Item(game, blueRingS, this.position);
            items.Add(blueRingItem);

            ISprite redRingS = spriteLoader.BuildSprite(ANIM_FILE, "redRing");
            Item redRingItem = new Item(game, redRingS, this.position);
            items.Add(redRingItem);

            ISprite blueBoomerangS = spriteLoader.BuildSprite(ANIM_FILE, "blueBoomerang");
            Item blueBoomerangItem = new Item(game, blueBoomerangS, this.position);
            items.Add(blueBoomerangItem);

            ISprite boomerangS = spriteLoader.BuildSprite(ANIM_FILE, "boomerang");
            Item boomerangItem = new Item(game, boomerangS, this.position);
            items.Add(boomerangItem);

            ISprite blueHeartS = spriteLoader.BuildSprite(ANIM_FILE, "blueHeart");
            Item blueHeartItem = new Item(game, blueHeartS, this.position);
            items.Add(blueHeartItem);

            ISprite blueTorchS = spriteLoader.BuildSprite(ANIM_FILE, "blueTorch");
            Item blueTorchItem = new Item(game, blueTorchS, this.position);
            items.Add(blueTorchItem);

            ISprite swordS = spriteLoader.BuildSprite(ANIM_FILE, "sword");
            Item swordItem = new Item(game, swordS, this.position);
            items.Add(swordItem);

            ISprite citemS = spriteLoader.BuildSprite(ANIM_FILE, "citem");
            Item citemItem = new Item(game, citemS, this.position);
            items.Add(citemItem);

            ISprite shieldRedS = spriteLoader.BuildSprite(ANIM_FILE, "shield_red");
            Item shieldRedItem = new Item(game, shieldRedS, this.position);
            items.Add(shieldRedItem);

            ISprite potionS = spriteLoader.BuildSprite(ANIM_FILE, "potion");
            Item potionItem = new Item(game, potionS, this.position);
            items.Add(potionItem);

            ISprite cakeS = spriteLoader.BuildSprite(ANIM_FILE, "cake");
            Item cakeItem = new Item(game, cakeS, this.position);
            items.Add(cakeItem);



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
