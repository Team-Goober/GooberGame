using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Sprint.Factory.HUD;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Sprite;
using System.Diagnostics;
using XMLData;

namespace Sprint.Loader
{
    internal class HUDLoader
    {
        private HUDFactory hudFactory;
        private SceneObjectManager som;

        public HUDLoader(ContentManager newContent, SpriteLoader spriteLoader)
        { 

            this.som = new SceneObjectManager();

            this.hudFactory = new(spriteLoader);

            MakeHUDframe("HUD/HUDSprite", "HUDFrame");
        }

        public void LoadLevelXML(string path)
        {
            //LevelData data = content.Load<LevelData>(path);

        }

        public SceneObjectManager GetScenes()
        {
            return som;
        }

        public void MakeHUDframe(string spriteFile, string spriteLabel)
        {
            som.Add(hudFactory.MakeHUD("HUDFrame", spriteFile, spriteLabel, new Vector2(0, 0)));
        }

        //Load Level Number
        //Load Minimap
        //Amount of Gem
        //Amount of Key
        //Amount of Bomb
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
