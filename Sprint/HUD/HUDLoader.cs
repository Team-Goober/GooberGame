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

        private List<HUDAnimSprite> GemNumber;
        private List<HUDAnimSprite> KeyNumber;
        private List<HUDAnimSprite> BombNumber;

        private List<HUDAnimSprite> LifeForce;

        private int maxHearts;

        private HUDAnimSprite bWeapon;
        private HUDAnimSprite aWeapon;

        HUDData data;

        public HUDLoader(ContentManager newContent, SpriteLoader spriteLoader)
        {
            content = newContent;
            topDisplay = new SceneObjectManager();
            inventoryScreen = new SceneObjectManager();

            hudFactory = new(spriteLoader);

            //Default Heart Amount
            UpdateMaxHeartAmount(3);
        }

        public void LoadHUD(string path, int levelNum, MapModel map)
        {
            data = content.Load<HUDData>(path);

            //Frame
            topDisplay.Add(MakeHUDSprite("HUDFrame", data.HUDFrame));
            topDisplay.Add(MakeHUDSprite("LevelFrame", data.LevelFrame));

            //Numbers
            List<HUDAnimSprite> nums = MakeLevelNumber(levelNum.ToString(), data.LevelNumPos, data.NumSpriteSize);
            GemNumber = MakeNumber("0B", data.GemNumPos, data.NumSpriteSize);
            KeyNumber = MakeNumber("0B", data.KeyNumPos, data.NumSpriteSize);
            BombNumber = MakeNumber("0B", data.BombNumPos, data.NumSpriteSize);
            nums.AddRange(GemNumber);
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

            // Weapons
            bWeapon = MakeSlotItem("Item", data.BWeapon);
            aWeapon = MakeSlotItem("Item", data.AWeapon);
            topDisplay.Add(bWeapon);
            topDisplay.Add(aWeapon);

            //Remove below or change later
            UpdateBWeapon("Sword");
            UpdateAWeapon("Bow");

            // Minimap
            topDisplay.Add(MakeMinimap(map, data.MinimapPos, data.MinimapRoomSize, data.MinimapPadding, data.MinimapBackgroundSize));
            
            // Inventory Backgrounds
            inventoryScreen.Add(MakeHUDSprite("Inventory", data.InventoryFramePos));
            inventoryScreen.Add(MakeHUDSprite("DungeonMap", data.MapFramePos));

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

        public HUDAnimSprite MakeSlotItem(string spriteLabel, Vector2 position)
        {
            HUDAnimSprite sprite = hudFactory.MakeHUDItem(spriteLabel, position);
            return sprite;
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

        public void OnInventoryEvent(ItemType it, int prev, int next)
        {
            switch (it)
            {
                case ItemType.Gem:
                    UpdateItemAmount(GemNumber, next);
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

        public void UpdateHeartAmount(double heartLeft)
        {
            int halfHeartPos = (int)Math.Floor(heartLeft);

            double half = heartLeft - Math.Truncate(heartLeft);

            if (half != 0)
            {
                LifeForce[halfHeartPos].SetSprite("HalfHeart");

                //Everything after will be empty hearts
                for (int i = halfHeartPos + 1; i < maxHearts; i++)
                {
                    LifeForce[i].SetSprite("EmptyHeart");
                }
            }
            else
            {
                for (int i = halfHeartPos; i < maxHearts; i++)
                {
                    LifeForce[i].SetSprite("EmptyHeart");
                }
            }
        }

        public void UpdateMaxHeartAmount(int newHeartAmount)
        {
            maxHearts = newHeartAmount;
        }

        public void UpdateBWeapon(string item)
        {
            bWeapon.SetSprite(item);
        }

        public void UpdateAWeapon(string item)
        {
            aWeapon.SetSprite(item);
        }

    }
}
