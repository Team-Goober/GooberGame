using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Sprint.Factory.HUD;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Sprite;
using System.Collections.Generic;
using System.Reflection.Emit;
using XMLData;


namespace Sprint.Loader
{
    internal class HUDLoader
    {
        private ContentManager content;
        private HUDFactory hudFactory;
        private SceneObjectManager som;

        private string gemAmount;
        private string keyAmount;
        private string bombAmount;
        private string heartLeft;

        const string loc = "HUD/HUDSprite";

        public HUDLoader(ContentManager newContent, SpriteLoader spriteLoader)
        { 
            this.content = newContent;
            this.som = new SceneObjectManager();

            this.hudFactory = new(spriteLoader);

            //Default Item Amount
            UpdateGemAmount(0);
            UpdateKeyAmount(0);
            UpdateBombAmount(0);
        }

        public void LoadHUD(string path, int levelNum)
        {
            HUDData data = content.Load<HUDData>(path);

            MakeHUDframe(loc, "HUDFrame", data.HUDFrame);
            MakeHUDframe(loc, "LevelFrame", data.LevelFrame);
            MakeLevelNumber(levelNum.ToString(), data.LevelNumPos);
            MakeNumber(gemAmount, loc, data.GemNumPos, data.NumSpriteSize);
            MakeNumber(keyAmount, loc, data.KeyNumPos, data.NumSpriteSize);
            MakeNumber(bombAmount, loc, data.BombNumPos, data.NumSpriteSize);
        }

        public SceneObjectManager GetScenes()
        {
            return som;
        }

        public void MakeLifeHeart(string num, string)
        {

        }

        public void MakeLevelNumber(string level, Vector2 position)
        {
            List<IHUD> levelNum = hudFactory.MakeNumber(level, loc, position);
            foreach(IHUD h in levelNum)
            {
                som.Add(h);
            }
        }

        public void MakeNumber(string num, string spriteFile, Vector2 position, int spriteSize)
        {
            if(num.Equals("0"))
            {
                num = "00";
            }

            som.Add(hudFactory.MakeHUD(spriteFile, "X", position));
            List<IHUD> levelNum = hudFactory.MakeNumber(num, loc, new Vector2(position.X + spriteSize, position.Y));
            foreach (IHUD h in levelNum)
            {
                som.Add(h);
            }
        }

        public void MakeHUDframe(string spriteFile, string spriteLabel, Vector2 position)
        {
            som.Add(hudFactory.MakeHUD(spriteFile, spriteLabel, position));
        }

        public void UpdateGemAmount(int newGemAmount)
        {
            gemAmount = newGemAmount.ToString();
        }

        public void UpdateKeyAmount(int newKeyAmount)
        {
            keyAmount = newKeyAmount.ToString();
        }

        public void UpdateBombAmount(int newBombAmount)
        {
            bombAmount = newBombAmount.ToString();
        }

        public void UpdateHeartAmount(int newHeartAmount)
        {
            heartLeft = newHeartAmount.ToString();
        }

        //Load Minimap
        //B Weapon
        //A Weapon
        //Amount Life

        //Update of Gem
        //Update of Key
        //Update of Bomb
        //Update Life
        //Update B Weapon
        //Update A Weapon
    }
}
