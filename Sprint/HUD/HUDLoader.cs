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

        private List<HUDAnimSprite> LifeForce; // Player hearts display

        private int maxHearts; // Total number of heart icons to render

        private HUDPowerupArray bWeapon; // Weapon bound to B slot
        private HUDPowerupArray aWeapon; // Weapon bound to A slot
        private HUDPowerupArray rupeeCount; // Rupee rendered with count on HUD
        private HUDPowerupArray keyCount; // Key rendered with count on HUD

        private HUDPowerupArray slotDisplay; // Inventory ability slots
        private HUDPowerupArray listingDisplay; // Array of all owned powerups
        private HUDSelector selector; // Selector that moves across slots

        private HUDText descriptionText; // Text that shows item descriptions

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
            LifeForce = MakeLifeHeart(data.HeartNumPos, data.NumSpriteSize);
            foreach (HUDAnimSprite sprite in LifeForce)
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

            // List of all owned items
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
            List<HUDAnimSprite> hearts = hudFactory.MakeHearts(CharacterConstants.MAX_HEARTS, "Heart", position, spriteSize);

            // How many can the player use?
            for(int i = 0; i < maxHearts; i++)
            {
                hearts[i].SetSprite("FullHeart");
            }

            return hearts;
        }

        public HUDAnimSprite MakeDeathHeart()
        {
            HUDAnimSprite heart = hudFactory.MakeHearts(1, "Heart", data.MenuHeartPos, data.NumSpriteSize)[0];

            heart.SetSprite("FullHeart");

            return heart;
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

        public void OnSelectorChooseEvent(int b, IPowerup item)
        {
            // Exchange sprites for A item
            if(b == 0)
            {
                aWeapon.SetSinglePowerup(item);

            }
            // Exchange sprites for B item
            else if (b == 1)
            {
                bWeapon.SetSinglePowerup(item);
            }
        }

        public void OnListingUpdateEvent(Dictionary<string, IPowerup> newDict)
        {
            // Add rupee display once player picks up some
            if (newDict.ContainsKey(Inventory.RupeeLabel) && rupeeCount.GetPowerups()[0,0] == null)
                rupeeCount.SetSinglePowerup(newDict[Inventory.RupeeLabel]);

            // Add key display once player picks up some
            if (newDict.ContainsKey(Inventory.KeyLabel) && keyCount.GetPowerups()[0, 0] == null)
                keyCount.SetSinglePowerup(newDict[Inventory.KeyLabel]);

            IPowerup[,] pups = new IPowerup[7, 4];
            int r = 0, c = 0;
            foreach (KeyValuePair<string, IPowerup> kvp in newDict)
            {
                // Move currently assigned cell down to next row once it reaches end of row
                if (c >= pups.GetLength(1))
                {
                    c = 0;
                    r++;
                }
                // Assign powerup to cell and increment to next cell
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
