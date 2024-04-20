using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Sprint.Characters;
using Sprint.Door;
using Sprint.HUD;
using Sprint.Interfaces;
using Sprint.Items;
using Sprint.Levels;
using Sprint.Music.Songs;
using Sprint.Sprite;
using System.Collections.Generic;
using Sprint.Content.LevelOne;
using Sprint.Interfaces.Powerups;
using XMLData;

namespace Sprint.Loader
{
    internal class LevelLoader
    {
        private ContentManager content;
        private DungeonState dungeon;
        private SpriteLoader spriteLoader;

        private TileFactory tileFactory;
        private DoorFactory doorFactory;
        private ItemFactory itemFactory;
        private EnemyFactory enemyFactory;
        private SongHandler songHandler;

        // Array to generate click-through-door commands
        private Rectangle[] doorBounds;
        // Dictionaries to link doors that aren't on cardinal dirs
        private Dictionary<int, IDoor> stairs;
        private Dictionary<int, int> stairLinks;
        private Player player;

        private int levelNumber;

        public LevelGeneration levelGeneration;


        public LevelLoader(ContentManager newContent, DungeonState dungeon, SpriteLoader spriteLoader, Player player)
        {
            this.content = newContent;
            this.dungeon = dungeon;
            this.spriteLoader = spriteLoader;
            this.player = player;


            tileFactory = new(spriteLoader);
            doorFactory = new(spriteLoader);
            itemFactory = new(spriteLoader);
            enemyFactory = new EnemyFactory(spriteLoader, player);
            songHandler = SongHandler.GetInstance();

        }

        /* Loads Level data from given file
        * 
        * @param path      Path to the XML file
        */
        public void LoadLevelXML(string path)
        {
            levelGeneration = new LevelGeneration();
            levelGeneration.CreateRoomGrid();
            var generatedGrid = levelGeneration.mapGrid;
            LevelData data = content.Load<LevelData>(path);
            ConnectedRoomData roomListData = new ConnectedRoomData(generatedGrid);
            roomListData.ConnectRoomData();



            itemFactory.LoadPowerupData();

            dungeon.SetArenaPosition(data.ArenaOffset);

            dungeon.ClearRooms(data.LayoutRows, data.LayoutColumns);
          
            levelNumber = data.Level;

            // Make commands for moving between rooms
            // Should be a list of bounding boxes for doors on each side of the room, so that they can be clicked
            doorBounds = new Rectangle[4];
            doorBounds[Directions.GetIndex(Directions.UP)] = new Rectangle((int)data.TopDoorPos.X, (int)data.TopDoorPos.Y, (int)data.DoorSize.X, (int)data.DoorSize.Y);
            doorBounds[Directions.GetIndex(Directions.RIGHT)] = new Rectangle((int)data.RightDoorPos.X, (int)data.RightDoorPos.Y, (int)data.DoorSize.X, (int)data.DoorSize.Y);
            doorBounds[Directions.GetIndex(Directions.DOWN)] = new Rectangle((int)data.BottomDoorPos.X, (int)data.BottomDoorPos.Y, (int)data.DoorSize.X, (int)data.DoorSize.Y);
            doorBounds[Directions.GetIndex(Directions.LEFT)] = new Rectangle((int)data.LeftDoorPos.X, (int)data.LeftDoorPos.Y, (int)data.DoorSize.X, (int)data.DoorSize.Y);

            // Offset each door bound area so they fall inside the arena on screen
            for (int i=0; i<doorBounds.Length; i++)
                doorBounds[i].Offset(data.ArenaOffset);

            stairs = new();
            stairLinks = new();

            // Load all rooms by index using RoomLoader
            var loc = new Point(0, 0);
            foreach (var roomIndex in generatedGrid)
            {
                if (roomIndex != 0)
                {
                    dungeon.AddRoom(loc, BuildRoom(roomListData, data, roomIndex, loc), roomListData.Room[roomIndex].Hidden);
                }
                loc.X++;
                if (loc.X == LevelGeneration.Columns)
                {
                    loc.X = 0;
                    loc.Y++;
                }
            }

            // Link together doors on opposing sides
            var roomLocation = new Point(0, 0);
            foreach (var roomIndex in generatedGrid)
            {
                if (generatedGrid[roomLocation.Y,roomLocation.X] != 0 && roomListData.Room[roomIndex].NeedWall)
                {
                    // Check each exit direction
                    for (int d = 0; d < 4; d++)
                    {
                        // Get direction and other room's indices
                        Vector2 dir = Directions.GetDirectionFromIndex(d);
                        int or = (int)(roomLocation.Y + dir.Y);
                        int oc = (int)(roomLocation.X + dir.X);

                        // Only link doors if the other room is in layout bounds
                        if (or >= 0 && or < data.LayoutRows && oc >= 0 && oc < data.LayoutColumns)
                        {
                            IDoor door = dungeon.GetRoomAt(roomLocation).GetDoors()[d];
                            Room otherRoom = dungeon.GetRoomAt(new Point(oc, or));
                            if(otherRoom != null && otherRoom.GetDoors().Count >= 4)
                            {
                                IDoor otherDoor = otherRoom.GetDoors()[Directions.GetIndex(Directions.Opposite(dir))];
                                door.SetOtherFace(otherDoor);
                            }
                        }
                    }
                }

                roomLocation.X++;
                if (roomLocation.X == LevelGeneration.Columns)
                {
                    roomLocation.X = 0;
                    roomLocation.Y++;
                }
            }

            // Link together stairs
            foreach(KeyValuePair<int, IDoor> st in stairs)
            {
                st.Value.SetOtherFace(stairs[stairLinks[st.Key]]);
            }

            dungeon.SetDoors(doorBounds);

            //Get compass pointer
            var compassLocation =new Point(0, 0);
            foreach (var roomIndex in levelGeneration.mapGrid)
            {
                if (roomIndex == 4)
                {
                    break;
                }
                //increment coordinates
                compassLocation.X++;
                if (compassLocation.X == LevelGeneration.Columns)
                {
                    compassLocation.X = 0;
                    compassLocation.Y++;
                }
            }
            dungeon.SetCompassPointer(compassLocation);

            dungeon.SetStart(data.BottomSpawnPos, data.StartLevel);

            //Load Song
            songHandler.PlaySong(data.Song);

            // Player needs to start with empty key and rupee powerups in their inventory
            IStackedPowerup key = itemFactory.MakePowerup(Inventory.KeyLabel) as IStackedPowerup;
            key.ReadyConsume(key.Quantity());
            key.Apply(player);
            IStackedPowerup rupee = itemFactory.MakePowerup(Inventory.RupeeLabel) as IStackedPowerup;
            rupee.ReadyConsume(rupee.Quantity());
            rupee.Apply(player);

        }

        /* Creates a room from given data
        * 
        * @param lvl        LevelData to pull info from
        * @param roomIndex  index of room in LevelData.Rooms to be made
        */
        public Room BuildRoom(ConnectedRoomData roomListData, LevelData levelData, int roomIndex, Point roomCoordinates)
        {
            RoomData rd = roomListData.Room[roomIndex];
            Point roomIndices = roomCoordinates;
            Room room = new(rd.Hidden);

            List<IDoor> roomDoors = room.GetDoors();
            List<Character> roomNpcs = room.GetNpcs();
            List<Item> roomItems = room.GetItems();
            SceneObjectManager scene = room.GetScene();

            //If the rooms need walls. Load Wall sprite, Door sprite, and wall colliders.
            if (roomListData.Room[roomIndex].NeedWall)
            {
                //Load Wall texture
                ISprite bgSprite = spriteLoader.BuildSprite(levelData.SpriteFile, levelData.BackgroundSprite);
                BackgroundTexture bg = new BackgroundTexture(bgSprite, levelData.WallPos);
                scene.Add(bg);

                //Load Wall colliders
                foreach (Rectangle rect in levelData.OuterWalls)
                {
                    InvisibleWall ow = new InvisibleWall(rect);
                    scene.Add(ow);
                }

                // spawn player on other side of room
                // parameter list is way too long
                IDoor[] doors = new IDoor[4];
                doors[Directions.GetIndex(Directions.UP)] = MakeDoor(levelData, rd.TopExit, levelData.DoorReferences[rd.TopExit].TopSprite, levelData.TopDoorPos, levelData.BottomSpawnPos, Directions.UP, roomIndices);
                doors[Directions.GetIndex(Directions.RIGHT)] = MakeDoor(levelData, rd.RightExit, levelData.DoorReferences[rd.RightExit].RightSprite, levelData.RightDoorPos, levelData.LeftSpawnPos, Directions.RIGHT, roomIndices);
                doors[Directions.GetIndex(Directions.DOWN)] = MakeDoor(levelData, rd.BottomExit, levelData.DoorReferences[rd.BottomExit].BottomSprite, levelData.BottomDoorPos, levelData.TopSpawnPos, Directions.DOWN, roomIndices);
                doors[Directions.GetIndex(Directions.LEFT)] = MakeDoor(levelData, rd.LeftExit, levelData.DoorReferences[rd.LeftExit].LeftSprite, levelData.LeftDoorPos, levelData.RightSpawnPos, Directions.LEFT, roomIndices);


                for (int i = 0; i < doors.Length; i++)
                {
                    roomDoors.Add(doors[i]);
                }
            }

            // Load extra doors that arent in normal 
            for (int i = 0; i < rd.Stairs.Count; i++)
            {
                StairData sd = rd.Stairs[i];
                IDoor stair = doorFactory.MakeStair(sd.Position, sd.Size, sd.SpawnPosition, roomIndices, dungeon);
                roomDoors.Add(stair);
                // Record in dictionaries for linkage once all doors are made
                stairs[sd.IDNum] = stair;
                stairLinks[sd.IDNum] = sd.OtherSideID;
            }

            //Load Floor tile 
            float x, y, xChange;
            if (roomListData.Room[roomIndex].NeedWall)
            {
                x = levelData.FloorGridPos.X;
                y = levelData.FloorGridPos.Y;
                xChange = levelData.FloorGridPos.X;
            } else
            {
                x = levelData.ZeroZeroPos.X;
                y = levelData.ZeroZeroPos.Y;
                xChange = levelData.ZeroZeroPos.X;
            }

            List<ITile> normTiles = new List<ITile>();
            List<ITile> moveTiles = new List<ITile>();
            
            foreach (string row in rd.TileGrid)
            {
                string[] str = row.Split(' ');
                foreach (string tile in str)
                {
                    if (tile.Equals("X"))
                    {
                        moveTiles.Add(MakeTile(levelData, tile, new Vector2(x, y)));
                        normTiles.Add(MakeTile(levelData, "0", new Vector2(x, y)));
                    }
                    else
                    {
                        normTiles.Add(MakeTile(levelData, tile, new Vector2(x, y)));
                    }
                    x += levelData.TileSize.X;
                }
                x = xChange;
                y += levelData.TileSize.Y;
            }
            
            foreach (ITile t in normTiles)
            {
                scene.Add(t);
            }

            foreach (ITile t in moveTiles)
            {
                scene.Add(t);
            }


            //Load enemies
            foreach (EnemySpawnData spawn in rd.Enemies)
            {
                Vector2 position = levelData.FloorGridPos + (spawn.TilePos + new Vector2(0.5f)) * levelData.TileSize;
                Enemy en = enemyFactory.MakeEnemy(spawn.Type, position, room);
                // Give item drop
                if(spawn.ItemDrop != null)
                {
                    Item it = itemFactory.MakeItem(spawn.ItemDrop, position, 0);
                    if (it != null)
                    {
                        en.GiveDrop(it);
                    }
                   
                }
                roomNpcs.Add(en);
            }

            //Load items
            foreach (ItemSpawnData spawn in rd.Items)
            {
                Vector2 position = levelData.FloorGridPos + (spawn.TilePos + new Vector2(0.5f)) * levelData.TileSize;
                Item it = itemFactory.MakeItem(spawn.Type, position, spawn.Price);
                if (it != null)
                {
                    roomItems.Add(it);
                }
            }

            //Load textboxes
            foreach (TextBoxData box in rd.TextBoxes)
            {
                ZeldaText text = new(box.FontName, box.Text, box.CharacterDimensions, 1.0f, box.Color, content);
                scene.Add(new HUDText(text, box.Position));
            }

            foreach (IDoor d in roomDoors)
                scene.Add(d);
            foreach (Character n in roomNpcs)
                scene.Add(n);
            foreach (Item i in roomItems)
                scene.Add(i);

            room.loadNPCEvents();

            return room;
        }

        public IDoor MakeDoor(LevelData lvl, string doorName, string spriteName, Vector2 position, Vector2 spawnPosition, Vector2 sideOfRoom, Point roomIndices)
        {
            DoorReference doorRef = lvl.DoorReferences[doorName];
            //Parameter list is too long
            IDoor door = doorFactory.MakeDoor(doorRef.Type, lvl.SpriteFile, spriteName, position, lvl.DoorSize, lvl.OpenDoorSize, spawnPosition, sideOfRoom, roomIndices, dungeon);
            return door;
        }

        public ITile MakeTile(LevelData lvl, string dictLabel, Vector2 position)
        {
            TileReference tRef = lvl.TileReferences[dictLabel];
            ITile tile = tileFactory.MakeTile(tRef.Type, lvl.SpriteFile, tRef.SpriteName, position, lvl.TileSize);
            return tile;
        }

        public int GetLevel() {

            return levelNumber;
        }
    }
}
