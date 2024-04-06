using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Items;
using Sprint.Levels;
using Sprint.Loader;
using Sprint.Sprite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using XMLData;


namespace Sprint.HUD
{
    internal class HUDLoader
    {
        private ContentManager content;
        private HUDFactory hudFactory;
        private SceneObjectManager topDisplay;
        private SceneObjectManager inventoryScreen;

        private List<HUDAnimSprite> RupeeNumber;
        private List<HUDAnimSprite> KeyNumber;
        private List<HUDAnimSprite> BombNumber;

        private List<HUDAnimSprite> LifeForce;

        private int maxHearts;

        private HUDInterchangeableSprite bWeapon;
        private HUDInterchangeableSprite aWeapon;
        private HUDInterchangeableSprite bSelection;

        private Dictionary<ItemType, HUDInterchangeableSprite> itemDisplays;
        private HUDSelector selector;

        HUDData data;

        public HUDLoader(ContentManager newContent, SpriteLoader spriteLoader)
        {
            content = newContent;
            topDisplay = new SceneObjectManager();
            inventoryScreen = new SceneObjectManager();

            hudFactory = new(spriteLoader);

            //Default Heart Amount
            maxHearts = 3;
        }

        public void LoadHUD(string path, int levelNum, MapModel map)
        {
            data = content.Load<HUDData>(path);

            //Frame
            topDisplay.Add(MakeHUDSprite("HUDFrame", data.HUDFrame));
            topDisplay.Add(MakeHUDSprite("LevelFrame", data.LevelFrame));

            //Numbers
            List<HUDAnimSprite> nums = MakeLevelNumber(levelNum.ToString(), data.LevelNumPos, data.NumSpriteSize);
            RupeeNumber = MakeNumber("0B", data.GemNumPos, data.NumSpriteSize);
            KeyNumber = MakeNumber("0B", data.KeyNumPos, data.NumSpriteSize);
            BombNumber = MakeNumber("0B", data.BombNumPos, data.NumSpriteSize);
            nums.AddRange(RupeeNumber);
            nums.AddRange(KeyNumber);
            nums.AddRange(BombNumber);
            foreach (HUDAnimSprite sprite in nums)
            {
                topDisplay.Add(sprite);
            }

            //Health
            List<HUDAnimSprite> hearts = MakeLifeHeart(data.HeartNumPos, data.NumSpriteSize);
            foreach (HUDAnimSprite sprite in hearts)
            {
                topDisplay.Add(sprite);
            }
            SetHearts(maxHearts);

            // Weapons
            bWeapon = MakeItemSprite(null, data.BWeapon);
            aWeapon = MakeItemSprite(ItemFactory.GetSpriteName(ItemType.Sword), data.AWeapon);
            topDisplay.Add(bWeapon);
            topDisplay.Add(aWeapon);

            // Minimap
            topDisplay.Add(MakeMinimap(map, data.MinimapPos, data.MinimapRoomSize, data.MinimapPadding, data.MinimapBackgroundSize));
            
            // Inventory Backgrounds
            inventoryScreen.Add(MakeHUDSprite("Inventory", data.InventoryFramePos));
            inventoryScreen.Add(MakeHUDSprite("DungeonMap", data.MapFramePos));

            // Visual displays for receiving items. Don't display until acquired

            itemDisplays = new()
            {
                { ItemType.Map, MakeItemSprite(ItemFactory.GetSpriteName(ItemType.Map), data.MapItemPos) },
                { ItemType.Compass, MakeItemSprite(ItemFactory.GetSpriteName(ItemType.Compass), data.CompassItemPos) },

                { ItemType.Raft, MakeItemSprite(ItemFactory.GetSpriteName(ItemType.Raft), data.RaftItemPos) },
                { ItemType.Book, MakeItemSprite(ItemFactory.GetSpriteName(ItemType.Book), data.BookItemPos) },
                { ItemType.RedRing, MakeItemSprite(ItemFactory.GetSpriteName(ItemType.RedRing), data.RingItemPos) },
                { ItemType.Ladder, MakeItemSprite(ItemFactory.GetSpriteName(ItemType.Ladder), data.LadderItemPos) },
                { ItemType.SpecialKey, MakeItemSprite(ItemFactory.GetSpriteName(ItemType.SpecialKey), data.SpecialKeyItemPos) },
                { ItemType.Bracelet, MakeItemSprite(ItemFactory.GetSpriteName(ItemType.Bracelet), data.BraceletItemPos) },
                { ItemType.Bow, MakeItemSprite(ItemFactory.GetSpriteName(ItemType.Bow), data.BowItemPos) }
            };

            // At the slot item displays
            for(int i=0; i<Inventory.Slots.GetLength(0); i++)
            {
                for (int j = 0; j < Inventory.Slots.GetLength(1); j++)
                {
                    ItemType slot = Inventory.Slots[i, j];
                    List<ItemType> upgrades = new() { slot };
                    if(Inventory.UpgradePaths.ContainsKey(slot))
                        upgrades = Inventory.UpgradePaths[slot];

                    // Calculate position of slot and place sprite there
                    Vector2 slotPos = (data.InventorySlotSize + data.InventoryPadding) * new Vector2(j, i) + data.FirstInventoryCell + data.InventorySlotSize / 2;

                    // Arrow slots are offset by a quarter
                    if (slot == ItemType.Arrow)
                        slotPos.X -= data.InventorySlotSize.X / 4;

                    // Add one sprite for every item in upgrade path
                    for (int k = 0; k < upgrades.Count; k++)
                    {
                        itemDisplays.Add(upgrades[k], MakeItemSprite(ItemFactory.GetSpriteName(upgrades[k]), slotPos));

                    }
                }
            }

            // Selector for slots
            selector = MakeSelector("selector", data.FirstInventoryCell, data.InventoryPadding + data.InventorySlotSize);
            inventoryScreen.Add(selector);

            bSelection = MakeItemSprite(null, data.BSelection);
            inventoryScreen.Add(bSelection);

            inventoryScreen.Add(MakeFullMap(map, data.FullMapPos, data.FullMapRoomSize, data.FullMapPadding, data.FullMapBackgroundSize));
            inventoryScreen.EndCycle();
        }

        public SceneObjectManager GetTopDisplay()
        {
            return topDisplay;
        }
        public SceneObjectManager GetInventoryScreen()
        {
            return inventoryScreen;
        }

        public List<HUDAnimSprite> MakeLifeHeart(Vector2 position, int spriteSize)
        {
            // Make the hearts
            LifeForce = hudFactory.MakeHearts(maxHearts, "Heart", position, spriteSize);

            // How many can the player use?
            for(int i = 0; i < maxHearts; i++)
            {
                LifeForce[i].SetSprite("FullHeart");
            }

            return LifeForce;
        }

        public HUDAnimSprite MakeDeathHeart()
        {
            LifeForce = hudFactory.MakeHearts(maxHearts, "Heart", data.MenuHeartPos, data.NumSpriteSize);

            LifeForce[0].SetSprite("FullHeart");

            return LifeForce[0];
        }

        public List<HUDAnimSprite> MakeLevelNumber(string level, Vector2 position, int spriteSize)
        {
            List<HUDAnimSprite> levelNum = hudFactory.MakeNumber(level + "B", position, spriteSize);
            return levelNum;
        }

        public List<HUDAnimSprite> MakeNumber(string num, Vector2 position, int spriteSize)
        {
            topDisplay.Add(hudFactory.MakeHUDSprite("X", position));
            List<HUDAnimSprite> levelNum = hudFactory.MakeNumber(num, new Vector2(position.X + spriteSize, position.Y), spriteSize);
            
            return levelNum;
        }

        public IHUD MakeMinimap(MapModel map, Vector2 position, Vector2 roomSize, int padding, Vector2 bgSize)
        {
            return new HUDMiniMap(map, position, roomSize, padding, bgSize);
        }

        public IHUD MakeFullMap(MapModel map, Vector2 position, Vector2 roomSize, int padding, Vector2 bgSize)
        {
            return new HUDFullMap(map, position, roomSize, padding, bgSize);
        }

        public IHUD MakeHUDSprite(string spriteLabel, Vector2 position)
        {
            return hudFactory.MakeHUDSprite(spriteLabel, position);
        }

        public HUDInterchangeableSprite MakeItemSprite(string spriteLabel, Vector2 position)
        {
            return hudFactory.MakeItemSprite(spriteLabel, position);
        }

        public HUDSelector MakeSelector(string spriteLabel, Vector2 position, Vector2 padding)
        {
            return hudFactory.MakeSelector(spriteLabel, position, padding);
        }

        public IHUD MakeGameOver()
        {
            IHUD sprite = hudFactory.MakeHUDSprite("GameOver", data.GameOverPos);
            return sprite;
        }

        public IHUD MakeDeathMenu()
        {
            IHUD sprite = hudFactory.MakeHUDSprite("DeathScreenMenu", data.DeathMenuPos);
            return sprite;
        }

        public void OnInventoryEvent(ItemType it, int prev, int next, List<ItemType> ownedUpgrades)
        {
            // Update UI numbers for specific items
            switch (it)
            {
                case ItemType.Rupee:
                    UpdateItemAmount(RupeeNumber, next);
                    break;
                case ItemType.Key:
                    UpdateItemAmount(KeyNumber, next);
                    break;
                case ItemType.Bomb:
                    UpdateItemAmount(BombNumber, next);
                    break;
                default:
                    break;
            }
            if (itemDisplays.ContainsKey(it))
            {
                // Add or remove displays for items that are gained or lost
                int k = ownedUpgrades.IndexOf(it);
                // Only update UI if this item is the highest owned upgrade
                if(k == ownedUpgrades.Count - 1)
                {
                    // Ran out
                    if (prev > 0 && next == 0)
                    {
                        inventoryScreen.Remove(itemDisplays[it]);
                        // Add in the next highest upgrade if it exists
                        if(k > 0 && itemDisplays.ContainsKey(ownedUpgrades[k - 1]))
                        {
                            inventoryScreen.Add(itemDisplays[ownedUpgrades[k - 1]]);
                        }
                    }
                    // Acquired
                    else if (prev == 0 && next > 0)
                    {
                        inventoryScreen.Add(itemDisplays[it]);
                        // Remove the next highest upgrade if it exists
                        if (k > 0 && itemDisplays.ContainsKey(ownedUpgrades[k - 1]))
                        {
                            inventoryScreen.Remove(itemDisplays[ownedUpgrades[k - 1]]);
                        }
                    }

                    // process changes
                    inventoryScreen.EndCycle();
                }
            }


        }

        public void OnSelectorMoveEvent(int r, int c) {
            // Move selector sprite
            selector.SetLocation(r, c);
        }

        public void OnSelectorChooseEvent(ItemType item)
        {
            // Exchange sprites for B item
            bWeapon.GiveSprite(itemDisplays[item].GetSprite());
            bSelection.GiveSprite(itemDisplays[item].GetSprite());
        }

        public void UpdateItemAmount(List<HUDAnimSprite> numSprites, int number)
        {
            String strNum = number.ToString() + "B";
            char[] arr = strNum.ToCharArray();

            int pos = 1;
            while (pos > -1)
            {
                numSprites[pos].SetSprite(arr[pos].ToString());
                pos--;
            }
        }

        public void UpdateHeartAmount(double prevHeart, double newHeart)
        {
            SetHearts(newHeart);
        }

        public void UpdateMaxHeartAmount(int prevMax, int newMax, double health)
        {
            maxHearts = newMax;
            SetHearts(health);
        }

        public void SetHearts(double health)
        {
            for (int i = 0; i < LifeForce.Count; i++)
            {
                // Heart is full
                if (i <= health - 1)
                {
                    LifeForce[i].SetSprite("FullHeart");
                    // Heart is half (only one can be)
                }
                else if (i == (int)health && i != health)
                {
                    LifeForce[i].SetSprite("HalfHeart");
                }
                // Heart is empty container
                else if (i <= maxHearts - 1)
                {
                    LifeForce[i].SetSprite("EmptyHeart");
                }
                // Container not received, hide heart
                else
                {
                    LifeForce[i].SetSprite("B");
                }
            }
        }

    }
}
