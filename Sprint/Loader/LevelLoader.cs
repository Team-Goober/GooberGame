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
        public LevelData Data;

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
            Data = content.Load<LevelData>(path);
        }



    }
}
