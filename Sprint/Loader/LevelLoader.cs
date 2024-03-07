using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Level;
using Sprint.Sprite;
using System.Collections.Generic;
using XMLData;

namespace Sprint.Loader
{
    internal class LevelLoader
    {
        private ContentManager content;
        private LevelData data;

        private List<Room> rooms;

        private ItemFactory itemFactory;
        private EnemyFactory enemyFactory;
        private DoorFactory doorFactory;
        private TileFactory tileFactory;

        public LevelLoader(ContentManager newContent)
        {
            this.content = newContent;

            itemFactory = new();
            enemyFactory = new();
            doorFactory = new();
            tileFactory = new();
        }

        /* Loads Level data from given file
        * 
        * @param path      Path to the XML file
        */
        public void LoadXML(string path)
        {
            data = content.Load<LevelData>(path);

            //tileDictionary = new();


        }

        public Tiles MakeTile(string charLabel, int row, int col)
        {
            TileReference tRef = data.TileReferences[charLabel];
            Vector2 pos = new Vector2(data.FloorGridPos.X + col * data.TileSize.X, data.FloorGridPos.Y + row * data.TileSize.Y);
            return tileFactory.MakeTile(tRef.Type, data.SpriteFile, tRef.SpriteName, pos);
        }


    }
}
