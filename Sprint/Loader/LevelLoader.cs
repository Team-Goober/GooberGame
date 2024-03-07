using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Sprint.Level;
using System.Collections.Generic;
using XMLData;

namespace Sprint.Loader
{
    internal class LevelLoader
    {
        private ContentManager content;
        private List<Room> rooms;

        public LevelLoader(ContentManager newContent)
        {
            this.content = newContent;
        }

        /* Loads Level data from given file
        * 
        * @param path      Path to the XML file
        */
        public void LoadXML(string path)
        {
            LevelData ld = content.Load<LevelData>(path);

            //tileDictionary = new();


        }

        public Tiles ReadTileLabel(string label)
        {
            return null;
        }


    }
}
