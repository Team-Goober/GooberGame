using Sprint.Levels;
using Microsoft.Xna.Framework.Content;
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
        public Level LoadXML(string path)
        {
            Data = content.Load<LevelData>(path);
        }



    }
}
