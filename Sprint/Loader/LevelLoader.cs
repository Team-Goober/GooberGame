using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Sprint.Characters;
using Sprint.Factory.Door;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Sprite;
using System.Diagnostics;
using System.Runtime.Serialization;
using XMLData;

namespace Sprint.Loader
{
    internal class LevelLoader
    {
        private ContentManager content;
        private GameObjectManager objectManager;
        private SpriteLoader spriteLoader;

        private TileFactory tileFactory;
        private DoorFactory doorFactory;
        private ItemFactory itemFactory;
        private EnemyFactory enemyFactory;

        public LevelLoader(ContentManager newContent, GameObjectManager objectManager, SpriteLoader spriteLoader)
        {
            this.content = newContent;
            this.objectManager = objectManager;
            this.spriteLoader = spriteLoader;
            
            tileFactory = new(spriteLoader);
            doorFactory = new(spriteLoader, objectManager);
            itemFactory = new();
            enemyFactory = new(objectManager, spriteLoader);

        }

        /* Loads Level data from given file
        * 
        * @param path      Path to the XML file
        */
        public void LoadLevelXML(string path)
        {
            LevelData data = content.Load<LevelData>(path);

            objectManager.ClearRooms();

            // Load all rooms by index using RoomLoader
            for (int i = 0; i < data.Rooms.Count; i++) {
                objectManager.AddRoom(BuildRoomManager(data, i));
            }

            objectManager.SwitchRoom(data.BottomSpawnPos, 0);

        }

        /* Loads Position from the given XML file
        * 
        * @param path      Path to the XML file
        */
        public RoomObjectManager BuildRoomManager(LevelData lvl, int roomIndex)
        {
            RoomData rd = lvl.Rooms[roomIndex];
            RoomObjectManager rom = new RoomObjectManager();

            //Load Wall texture
            ISprite bgSprite = spriteLoader.BuildSprite(lvl.SpriteFile, lvl.BackgroundSprite);
            BackgroundTexture bg = new BackgroundTexture(bgSprite, Vector2.Zero);
            rom.Add(bg);

            //Load Wall colliders
            foreach (Rectangle rect in lvl.OuterWalls)
            {
                InvisibleWall ow = new InvisibleWall(rect);
                rom.Add(ow);
            }

            // spawn player on other side of room
            rom.Add(MakeDoor(lvl, rd.TopExit, lvl.TopDoorPos, lvl.BottomSpawnPos));
            rom.Add(MakeDoor(lvl, rd.BottomExit, lvl.BottomDoorPos, lvl.TopSpawnPos));
            rom.Add(MakeDoor(lvl, rd.LeftExit, lvl.LeftDoorPos, lvl.RightSpawnPos));
            rom.Add(MakeDoor(lvl, rd.RightExit, lvl.RightDoorPos, lvl.LeftSpawnPos));

            //Load Floor tile 
            float x = lvl.FloorGridPos.X; float y = lvl.FloorGridPos.Y;
            foreach (string row in rd.TileGrid)
            {
                string[] str = row.Split(' ');
                foreach (string tile in str)
                {
                    rom.Add(MakeTile(lvl, tile, new Vector2(x, y)));
                    x += lvl.TileSize.X;
                }
                x = lvl.FloorGridPos.X;
                y += lvl.TileSize.Y;
            }

            //Load enemies
            foreach (EnemySpawnData spawn in rd.Enemies)
            {
                float xP = lvl.FloorGridPos.X + spawn.Column * lvl.TileSize.X;
                float yP = lvl.FloorGridPos.Y + spawn.Row * lvl.TileSize.Y;
                rom.Add(enemyFactory.MakeEnemy(spawn.Type, new System.Numerics.Vector2(xP, yP)));
            }

            /*
            //Load items
            foreach (ItemSpawnData spawn in rd.Items)
            {
                float xP = lvl.FloorGridPos.X + spawn.Column * lvl.TileSize.X;
                float yP = lvl.FloorGridPos.Y + spawn.Row * lvl.TileSize.Y;
                rom.Add(itemFactory.MakeItem(spawn.Type, new System.Numerics.Vector2(xP, yP)));
            }
            */
            return rom;
        }

        public IDoor MakeDoor(LevelData lvl, ExitData exit, Vector2 position, Vector2 spawnPosition)
        {
            DoorReference doorRef = lvl.DoorReferences[exit.Door];
            //Parameter list is too long
            IDoor door = doorFactory.MakeDoor(doorRef.Type, lvl.SpriteFile, doorRef.SpriteName, position, lvl.DoorSize, lvl.OpenDoorSize, spawnPosition, exit.AdjacentRoom);
            return door;
        }

        public ITile MakeTile(LevelData lvl, string dictLabel, Vector2 position)
        {
            TileReference tRef = lvl.TileReferences[dictLabel];
            ITile tile = tileFactory.MakeTile(tRef.Type, lvl.SpriteFile, tRef.SpriteName, position, lvl.TileSize);
            return tile;
        }

    }
}
