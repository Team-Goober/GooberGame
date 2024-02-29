using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Diagnostics;
using XMLData;

namespace Sprint.Loader
{
    internal class PositionLoader
    {
        private ContentManager content;

        private Dictionary<string, Vector2> loaded;

        public PositionLoader(ContentManager newContent)
        {
            loaded = new Dictionary<string, Vector2>();
            this.content = newContent;
        }

        /* Loads Position from the given XML file
        * 
        * @param path      Path to the XML file
        */
        public void LoadXML(string path)
        {
            PositionData spriteDict = content.Load<PositionData>(path);
            foreach (KeyValuePair<string, Vector2> kvp in spriteDict.SpritePosition)
            {
                loaded.Add(kvp.Key, kvp.Value);
                Debug.WriteLine(kvp.Key + " " + kvp.Value);
            }
        }

        public Vector2 GetPosition(string key)
        {
            return loaded[key];
        }
    }
}
