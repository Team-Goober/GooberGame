using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Levels;
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
        private SceneObjectManager som;

        private string gemAmount;
        private string keyAmount;
        private string bombAmount;
        private int maxHearts;
        private double heartLeft;

        private string bWeapon;
        private string aWeapon;

        HUDData data;

        public HUDLoader(ContentManager newContent, SpriteLoader spriteLoader)
        {
            content = newContent;
            som = new SceneObjectManager();

            hudFactory = new(spriteLoader);

            //Default Item Amount
            UpdateGemAmount(0);
            UpdateKeyAmount(0);
            UpdateBombAmount(0);
            UpdateHeartAmount(2.5);
            UpdateMaxHeartAmount(3);

            UpdateBWeapon("Boomerang");
            UpdateAWeapon("Sword");
        }

        public void LoadHUD(string path, int levelNum, MapModel map)
        {
            data = content.Load<HUDData>(path);

            //Frame
            MakeHUDSprite("HUDFrame", data.HUDFrame);
            MakeHUDSprite("LevelFrame", data.LevelFrame);

            //Numbers
            MakeLevelNumber(levelNum.ToString(), data.LevelNumPos, data.NumSpriteSize);
            MakeNumber(gemAmount, data.GemNumPos, data.NumSpriteSize);
            MakeNumber(keyAmount, data.KeyNumPos, data.NumSpriteSize);
            MakeNumber(bombAmount, data.BombNumPos, data.NumSpriteSize);

            //Health
            MakeLifeHeart(data.HeartNumPos, data.NumSpriteSize);

            // Weapons
            MakeHUDSprite(bWeapon, data.BWeapon);
            MakeHUDSprite(aWeapon, data.AWeapon);


            MakeMinimap(map, data.MinimapPos, data.MinimapRoomSize, data.MinimapPadding);
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

            if (half != 0)
            {
                float newX = position.X + fullHeart * spriteSize;
                heartList.AddRange(hudFactory.MakeHearts(1, "HalfHeart", new Vector2(newX, position.Y), spriteSize));
            }

            foreach (IHUD h in heartList)
            {
                som.Add(h);
            }
        }

        public void MakeLevelNumber(string level, Vector2 position, int spriteSize)
        {
            List<IHUD> levelNum = hudFactory.MakeNumber(level, position, spriteSize);
            foreach (IHUD h in levelNum)
            {
                som.Add(h);
            }
        }

        public void MakeNumber(string num, Vector2 position, int spriteSize)
        {

            som.Add(hudFactory.MakeHUDSprite("X", position));
            List<IHUD> levelNum = hudFactory.MakeNumber(num, new Vector2(position.X + spriteSize, position.Y), spriteSize);
            foreach (IHUD h in levelNum)
            {
                som.Add(h);
            }
        }

        public void MakeMinimap(MapModel map, Vector2 position, Vector2 roomSize, int padding)
        {
            som.Add(new HUDMap(map, position, roomSize, padding));
        }

        // TEST EVENT
        public void UpdateKeyAmount(int num)
        {
            Debug.WriteLine("Here: " + num);
            keyAmount = num.ToString();
        }

        public void MakeHUDSprite(string spriteLabel, Vector2 position)
        {
            som.Add(hudFactory.MakeHUDSprite(spriteLabel, position));
        }

        public void UpdateGemAmount(int newGemAmount)
        {
            gemAmount = newGemAmount.ToString();
        }

        public void UpdateBombAmount(int newBombAmount)
        {
            bombAmount = newBombAmount.ToString();
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
            MakeNumber(gemAmount, data.GemNumPos, data.NumSpriteSize);
            MakeNumber(keyAmount, data.KeyNumPos, data.NumSpriteSize);
            MakeNumber(bombAmount, data.BombNumPos, data.NumSpriteSize);

            //Health
            MakeLifeHeart(data.HeartNumPos, data.NumSpriteSize);

            // Weapons
            MakeHUDSprite(bWeapon, data.BWeapon);
            MakeHUDSprite(aWeapon, data.AWeapon);
        }
        //Load Minimap
    }
}
