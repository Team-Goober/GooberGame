using Microsoft.Xna.Framework.Content;
using Sprint.Levels;
using Sprint.Sprite;
using System.Runtime.Serialization;
using XMLData;

namespace Sprint.Loader
{
    internal class LevelLoader
    {
        private ContentManager content;
        private GameObjectManager objectManager;
        private SpriteLoader spriteLoader;

        public LevelLoader(ContentManager newContent, GameObjectManager objectManager, SpriteLoader spriteLoader)
        {
            this.content = newContent;
            this.objectManager = objectManager;
            this.spriteLoader = spriteLoader;

        }

        /* Loads Level data from given file
        * 
        * @param path      Path to the XML file
        */
        public Level LoadXML(string path)
        {
            LevelData data = content.Load<LevelData>(path);

            Level level = new Level(objectManager);

            RoomLoader rLoader = new RoomLoader(objectManager, content, spriteLoader);

            // Load all rooms by index using RoomLoader
            for (int i = 0; i < data.Rooms.Count; i++) {
                level.AddRoom(rLoader.LoadFromData(data, i));
            }

            return level;
        }



    }
}
