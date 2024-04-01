using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Sprint.Characters;
using Sprint.Events;
using Sprint.Factory.HUD;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Sprite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using XMLData;


namespace Sprint.Loader
{
    internal class HUDLoader
    {
        private ContentManager content;
        private HUDFactory hudFactory;
        private SceneObjectManager som;

        private List<HUDAnimSprite> GemNumber;
        private List<HUDAnimSprite> KeyNumber;
        private List<HUDAnimSprite> BombNumber;

        private List<HUDAnimSprite> LifeForce;

        private int maxHearts;

        private string bWeapon;
        private string aWeapon;

        HUDData data;

        public HUDLoader(ContentManager newContent, SpriteLoader spriteLoader)
        { 
            this.content = newContent;
            this.som = new SceneObjectManager();

            this.hudFactory = new(spriteLoader);

            //Default Item Amount
            UpdateMaxHeartAmount(3);

            UpdateBWeapon("Boomerang");
            UpdateAWeapon("Sword");
        }

        public void LoadHUD(string path, int levelNum)
        {
            data = content.Load<HUDData>(path);

            //Frame
            MakeHUDSprite("HUDFrame", data.HUDFrame);
            MakeHUDSprite("LevelFrame", data.LevelFrame);

            //Numbers
            MakeLevelNumber(levelNum.ToString(), data.LevelNumPos, data.NumSpriteSize);
            GemNumber = MakeNumber("0B", data.GemNumPos, data.NumSpriteSize);
            KeyNumber = MakeNumber("0B", data.KeyNumPos, data.NumSpriteSize);
            BombNumber = MakeNumber("0B", data.BombNumPos, data.NumSpriteSize);

            //Health
            MakeLifeHeart(data.HeartNumPos, data.NumSpriteSize);

            // Weapons
            MakeHUDSprite(bWeapon, data.BWeapon);
            MakeHUDSprite(aWeapon, data.AWeapon);
        }

        public SceneObjectManager GetScenes()
        {
            return som;
        }

        public void MakeLifeHeart(Vector2 position, int spriteSize)
        {
            // Make the hearts
            LifeForce = hudFactory.MakeHearts(maxHearts, "Heart", position, spriteSize);

            // How many can the player use?
            for(int i = 0; i < maxHearts; i++)
            {
                LifeForce[i].SetSprite("FullHeart");
            }

            foreach(HUDAnimSprite h in LifeForce)
            {
                som.Add(h);
            }
        }

        public void MakeLevelNumber(string level, Vector2 position, int spriteSize)
        {
            List<HUDAnimSprite> levelNum = hudFactory.MakeNumber(level + "B", position, spriteSize);
            foreach(HUDAnimSprite h in levelNum)
            {
                som.Add(h);
            }
        }

        public List<HUDAnimSprite> MakeNumber(string num, Vector2 position, int spriteSize)
        {
            som.Add(hudFactory.MakeHUDSprite("X", position));
            List<HUDAnimSprite> levelNum = hudFactory.MakeNumber(num, new Vector2(position.X + spriteSize, position.Y), spriteSize);

            foreach (HUDAnimSprite h in levelNum)
            {
                som.Add(h);
            }

            return levelNum;
        }

        public void MakeHUDSprite(string spriteLabel, Vector2 position)
        {
            som.Add(hudFactory.MakeHUDSprite(spriteLabel, position));
        }

        public void UpdateGemAmount(int nums)
        {
            String strNum = nums.ToString() + "B";
            char[] arr = strNum.ToCharArray();

            int pos = 1;
            while (pos > -1)
            {
                GemNumber[pos].SetSprite(arr[pos].ToString());
                pos--;
            }
        }

        public void UpdateKeyAmount(int nums)
        {
            String strNum = nums.ToString() + "B";
            char[] arr = strNum.ToCharArray();

            int pos = 1;
            while (pos > -1)
            {
                KeyNumber[pos].SetSprite(arr[pos].ToString());
                pos--;
            }
        }

        public void UpdateBombAmount(int nums)
        {
            String strNum = nums.ToString() + "B";
            char[] arr = strNum.ToCharArray();

            int pos = 1;
            while (pos > -1)
            {
                BombNumber[pos].SetSprite(arr[pos].ToString());
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

        public void UpdateBWeapon(string newWeapon)
        {
            bWeapon = newWeapon;
        }

        public void UpdateAWeapon(string newWeapon)
        {
            aWeapon = newWeapon;
        }

        public void Update()
        {
            // Weapons
            MakeHUDSprite(bWeapon, data.BWeapon);
            MakeHUDSprite(aWeapon, data.AWeapon);
        }
        //Load Minimap
    }
}
