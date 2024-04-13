using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
using Sprint.Items;
using Sprint.Levels;
using Sprint.Loader;
using Sprint.Sprite;
using System;
using System.Collections.Generic;
using XMLData;


namespace Sprint.HUD
{
    internal class HUDLoader
    {
        private ContentManager content;
        private HUDFactory hudFactory;
        private SceneObjectManager topDisplay;
        private SceneObjectManager inventoryScreen;

        private List<HUDAnimSprite> LifeForce;

        private int maxHearts;

        private HUDPowerupArray bWeapon;
        private HUDPowerupArray aWeapon;
        private HUDPowerupArray rupeeCount;
        private HUDPowerupArray keyCount;

        private HUDPowerupArray slotDisplay;
        private HUDPowerupArray listingDisplay;
        private HUDSelector selector;

        private HUDText descriptionText;

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
            aWeapon = MakeItemSprite(null, data.AWeapon);
            rupeeCount = MakeItemSprite(null, data.GemNumPos + new Vector2(40, 0));
            keyCount = MakeItemSprite(null, data.KeyNumPos + new Vector2(40, 0));
            topDisplay.Add(bWeapon);
            topDisplay.Add(aWeapon);
            topDisplay.Add(rupeeCount);
            topDisplay.Add(keyCount);

            // Minimap
            topDisplay.Add(MakeMinimap(map, data.MinimapPos, data.MinimapRoomSize, data.MinimapPadding, data.MinimapBackgroundSize));
            
            // Inventory Backgrounds
            inventoryScreen.Add(MakeHUDSprite("Inventory", data.InventoryFramePos));
            inventoryScreen.Add(MakeHUDSprite("DungeonMap", data.MapFramePos));

            // Selector for slots
            selector = MakeSelector("selector", data.FirstInventoryCell, data.InventoryPadding + data.InventorySlotSize);
            inventoryScreen.Add(selector);

            // At the slot item displays
            slotDisplay = new HUDPowerupArray(data.FirstInventoryCell + data.InventorySlotSize / 2, data.InventorySlotSize + data.InventoryPadding);
            inventoryScreen.Add(slotDisplay);

            listingDisplay = new HUDPowerupArray(new Vector2(50, 350), data.InventorySlotSize + data.InventoryPadding);
            inventoryScreen.Add(listingDisplay);

            // Hovered item summary text
            descriptionText = new HUDText(new ZeldaText("nintendo", new() { "--HOVER ITEM--" }, new Vector2(24, 24), 0.75f, Color.Gray, content), new Vector2(240, 200));
            inventoryScreen.Add(descriptionText);

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

        public HUDPowerupArray MakeItemSprite(IPowerup powerup, Vector2 position)
        {
            return hudFactory.MakeItemSprite(powerup, position);
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

        public void OnSelectorMoveEvent(int r, int c) {
            // Move selector sprite
            selector.SetLocation(r, c);
        }

        public void OnSelectorChooseEvent(IPowerup item)
        {
            // Exchange sprites for B item
            bWeapon.SetSinglePowerup(item);
        }

        public void OnListingUpdateEvent(Dictionary<string, IPowerup> newDict)
        {
            if (newDict.ContainsKey(Inventory.RupeeLabel))
                rupeeCount.SetSinglePowerup(newDict[Inventory.RupeeLabel]);

            if (newDict.ContainsKey(Inventory.KeyLabel))
                keyCount.SetSinglePowerup(newDict[Inventory.KeyLabel]);

            IPowerup[,] pups = new IPowerup[7, 4];
            int r = 0, c = 0;
            foreach (KeyValuePair<string, IPowerup> kvp in newDict)
            {
                if (c >= pups.GetLength(1))
                {
                    c = 0;
                    r++;
                }
                if (r < pups.GetLength(0))
                {
                    pups[r, c] = kvp.Value;
                    c++;
                }
                else
                {
                    break;
                }
            }
            listingDisplay.SetPowerups(pups);

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

        public void SetSlotsArray(IAbility[,] slots)
        {
            slotDisplay.SetPowerups(slots);
        }

        public HUDPowerupArray GetListing()
        {
            return listingDisplay;
        }

        public HUDText GetDescriptionText()
        {
            return descriptionText;
        }

    }
}
