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

        private List<HUDNumber> GemNumber;
        private List<HUDNumber> KeyNumber;
        private List<HUDNumber> BombNumber;

        private int maxHearts;
        private double heartLeft;

        private string bWeapon;
        private string aWeapon;

        HUDData data;

        public HUDLoader(ContentManager newContent, SpriteLoader spriteLoader)
        { 
            this.content = newContent;
            this.som = new SceneObjectManager();

            this.hudFactory = new(spriteLoader);

            //Default Item Amount
            UpdateHeartAmount(2.5);
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
            List<IHUD> heartList = new List<IHUD>();

            heartList.AddRange(hudFactory.MakeHearts(maxHearts, "EmptyHeart", position, spriteSize));

            int fullHeart = (int)Math.Floor(heartLeft);
            heartList.AddRange(hudFactory.MakeHearts(fullHeart, "FullHeart", position, spriteSize));

            double half = heartLeft - Math.Truncate(heartLeft);

            if(half !=  0)
            {
                float newX = position.X + (fullHeart * spriteSize);
                heartList.AddRange(hudFactory.MakeHearts(1, "HalfHeart", new Vector2(newX, position.Y), spriteSize));
            }

            foreach (IHUD h in heartList)
            {
                som.Add(h);
            }
        }

        public void MakeLevelNumber(string level, Vector2 position, int spriteSize)
        {
            List<HUDNumber> levelNum = hudFactory.MakeNumber(level + "B", position, spriteSize);
            foreach(HUDNumber h in levelNum)
            {
                som.Add(h);
            }
        }

        public List<HUDNumber> MakeNumber(string num, Vector2 position, int spriteSize)
        {
            som.Add(hudFactory.MakeHUDSprite("X", position));
            List<HUDNumber> levelNum = hudFactory.MakeNumber(num, new Vector2(position.X + spriteSize, position.Y), spriteSize);

            foreach (HUDNumber h in levelNum)
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
                GemNumber[pos].SetNumber(arr[pos].ToString());
                pos--;
            }
        }

        // TEST EVENT
        public void UpdateKeyAmount(int nums)
        {
            String strNum = nums.ToString() + "B";
            char[] arr = strNum.ToCharArray();

            int pos = 1;
            while (pos > -1)
            {
                KeyNumber[pos].SetNumber(arr[pos].ToString());
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
                BombNumber[pos].SetNumber(arr[pos].ToString());
                pos--;
            }
        }

        public void UpdateHeartAmount(double newHeartAmount)
        {

            heartLeft = newHeartAmount;
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
            //Health
            MakeLifeHeart(data.HeartNumPos, data.NumSpriteSize);

            // Weapons
            MakeHUDSprite(bWeapon, data.BWeapon);
            MakeHUDSprite(aWeapon, data.AWeapon);
        }
        //Load Minimap
    }
}
