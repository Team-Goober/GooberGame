using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Sprint.Characters;
using Sprint.Factory.Door;
using Sprint.Functions;
using Sprint.Functions.RoomTransition;
using Sprint.Input;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Sprite;
using XMLData;

namespace Sprint.Loader
{
    internal class LevelLoader
    {
        private ContentManager content;
        private DungeonState dungeon;
        private SpriteLoader spriteLoader;
        private IInputMap inputTable;

        private TileFactory tileFactory;
        private DoorFactory doorFactory;
        private ItemFactory itemFactory;
        private EnemyFactory enemyFactory;

        // Array to generate click-through-door commands
        private Rectangle[] doorBounds;
        private IDoor[,] doorsPerSide;

        public LevelLoader(ContentManager newContent, DungeonState dungeon, SpriteLoader spriteLoader, IInputMap inputTable)
        {
            this.content = newContent;
            this.dungeon = dungeon;
            this.spriteLoader = spriteLoader;
            this.inputTable = inputTable;

            tileFactory = new(spriteLoader);
            doorFactory = new(spriteLoader);
            itemFactory = new(spriteLoader);
            enemyFactory = new(spriteLoader);

        }

        /* Loads Level data from given file
        * 
        * @param path      Path to the XML file
        */
        public void LoadLevelXML(string path)
        {
            LevelData data = content.Load<LevelData>(path);

            dungeon.ClearRooms();


            // Make commands for moving between rooms
            // Should be a list of bounding boxes for doors on each side of the room, so that they can be clicked
            doorBounds = new Rectangle[] { new Rectangle((int)data.TopDoorPos.X, (int)data.TopDoorPos.Y, (int)data.DoorSize.X, (int)data.DoorSize.Y),
                new Rectangle((int)data.BottomDoorPos.X, (int)data.BottomDoorPos.Y, (int)data.DoorSize.X, (int)data.DoorSize.Y),
                new Rectangle((int)data.LeftDoorPos.X, (int)data.LeftDoorPos.Y, (int)data.DoorSize.X, (int)data.DoorSize.Y),
                new Rectangle((int)data.RightDoorPos.X, (int)data.RightDoorPos.Y, (int)data.DoorSize.X, (int)data.DoorSize.Y)};

            doorsPerSide = new IDoor[4, data.Rooms.Count];

            // Load all rooms by index using RoomLoader
            for (int i = 0; i < data.Rooms.Count; i++) {
                dungeon.AddRoom(BuildRoomManager(data, i));
            }

            // Make a command that checks all doors at its position for switching rooms when middle clicked
            for (int i=0; i<4; i++)
            {
                IDoor[] slice = new IDoor[data.Rooms.Count];
                for (int j=0; j < slice.Length; j++)
                {
                    slice[j] = doorsPerSide[i, j];
                }
                inputTable.RegisterMapping(new ClickInBoundsTrigger(ClickInBoundsTrigger.MouseButton.Middle, doorBounds[i]),
                    new SwitchRoomFromDoorsCommand(slice, dungeon));
            }

            dungeon.SwitchRoom(data.BottomSpawnPos, data.StartLevel);

        }

        /* Creates a room from given data
        * 
        * @param lvl        LevelData to pull info from
        * @param roomIndex  index of room in LevelData.Rooms to be made
        */
        public SceneObjectManager BuildRoomManager(LevelData lvl, int roomIndex)
        {
            RoomData rd = lvl.Rooms[roomIndex];
            SceneObjectManager rom = new SceneObjectManager();

            //If the rooms need walls. Load Wall sprite, Door sprite, and wall colliders.
            if (lvl.Rooms[roomIndex].NeedWall)
            {
                //Load Wall texture
                ISprite bgSprite = spriteLoader.BuildSprite(lvl.SpriteFile, lvl.BackgroundSprite);
                BackgroundTexture bg = new BackgroundTexture(bgSprite, lvl.WallPos);
                rom.Add(bg);

                //Load Wall colliders
                foreach (Rectangle rect in lvl.OuterWalls)
                {
                    InvisibleWall ow = new InvisibleWall(rect);
                    rom.Add(ow);
                }

                // spawn player on other side of room
                IDoor[] doors = { MakeDoor(lvl, rd.TopExit, lvl.TopDoorPos, lvl.BottomSpawnPos),
                MakeDoor(lvl, rd.BottomExit, lvl.BottomDoorPos, lvl.TopSpawnPos),
                MakeDoor(lvl, rd.LeftExit, lvl.LeftDoorPos, lvl.RightSpawnPos),
                MakeDoor(lvl, rd.RightExit, lvl.RightDoorPos, lvl.LeftSpawnPos) };

                for (int i = 0; i < doors.Length; i++)
                {
                    rom.Add(doors[i]);
                    // make commands if clicked
                    doorsPerSide[i, roomIndex] = doors[i];
                }
            }

            //Load Floor tile 
            float x, y, xChange;
            if (lvl.Rooms[roomIndex].NeedWall)
            {
                x = lvl.FloorGridPos.X;
                y = lvl.FloorGridPos.Y;
                xChange = lvl.FloorGridPos.X;
            } else
            {
                x = lvl.ZeroZeroPos.X;
                y = lvl.ZeroZeroPos.Y;
                xChange = lvl.ZeroZeroPos.X;
            }
            
            foreach (string row in rd.TileGrid)
            {
                string[] str = row.Split(' ');
                foreach (string tile in str)
                {
                    rom.Add(MakeTile(lvl, tile, new Vector2(x, y)));
                    x += lvl.TileSize.X;
                }
                x = xChange;
                y += lvl.TileSize.Y;
            }

            //Load enemies
            foreach (EnemySpawnData spawn in rd.Enemies)
            {
                float xP = lvl.FloorGridPos.X + (spawn.Column + 0.5f) * lvl.TileSize.X;
                float yP = lvl.FloorGridPos.Y + (spawn.Row + 0.5f) * lvl.TileSize.Y;
                rom.Add(enemyFactory.MakeEnemy(spawn.Type, new System.Numerics.Vector2(xP, yP), rom));
            }

            //Load items
            foreach (ItemSpawnData spawn in rd.Items)
            {
                float xP = lvl.FloorGridPos.X + (spawn.Column + 0.5f) * lvl.TileSize.X;
                float yP = lvl.FloorGridPos.Y + (spawn.Row + 0.5f) * lvl.TileSize.Y;
                rom.Add(itemFactory.MakeItem(spawn.Type, new System.Numerics.Vector2(xP, yP)));
            }

            return rom;
        }

        public IDoor MakeDoor(LevelData lvl, ExitData exit, Vector2 position, Vector2 spawnPosition)
        {
            DoorReference doorRef = lvl.DoorReferences[exit.Door];
            //Parameter list is too long
            IDoor door = doorFactory.MakeDoor(doorRef.Type, lvl.SpriteFile, doorRef.SpriteName, position, lvl.DoorSize, lvl.OpenDoorSize, spawnPosition, exit.AdjacentRoom, dungeon);
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
