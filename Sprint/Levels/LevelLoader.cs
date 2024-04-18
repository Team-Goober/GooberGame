using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Sprint.Characters;
using Sprint.Door;
using Sprint.HUD;
using Sprint.Interfaces;
using Sprint.Interfaces.Powerups;
using Sprint.Items;
using Sprint.Items.Effects;
using Sprint.Levels;
using Sprint.Music.Sfx;
using Sprint.Music.Songs;
using Sprint.Sprite;
using System.Collections.Generic;
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

        private int levelNumber;

        private LevelGeneration levelGeneration;


        public LevelLoader(ContentManager newContent, DungeonState dungeon, SpriteLoader spriteLoader)
        {
            this.content = newContent;
            this.dungeon = dungeon;
            this.spriteLoader = spriteLoader;

            tileFactory = new(spriteLoader);
            doorFactory = new(spriteLoader);
            itemFactory = new(spriteLoader);
            enemyFactory = new(spriteLoader);
            songHandler = SongHandler.GetInstance();
            levelGeneration = LevelGeneration.GetInstance();
        }

        /* Loads Level data from given file
        * 
        * @param path      Path to the XML file
        */
        public void LoadLevelXML(string path)
        {
            // Dictionary<int, (int, int)> indexConverter = new Dictionary<int, (int, int)>()
            // {
            //     {0, (2,2)}, {1, (2,3)}, {2,(3,3)}, {3,(3,5)}, {4,(3,6)}, {5,(4,1)}, {6,(4,2)}, {7,(4,3)}, {8,(4,4)},
            //     {9,(4,5)}, {10,(5,2)}, {11,(5,3)}, {12,(5,4)}, {13,(6,3)}, {14,(7,2)}, {15,(7,3)}, {16,(7,4)}
            // };
            //
            // int rowNum = 0;
            // int columnNum = 0;
            // RoomData[] empty = new[] {(RoomData)null,(RoomData)null, (RoomData)null,(RoomData)null,(RoomData)null,(RoomData)null,(RoomData)null,(RoomData)null} as RoomData[];
            // RoomData[][] mapGirdConverted = new RoomData[8][];
            // for (int i = 0; i< 8;i++)
            // {
            //     mapGirdConverted[i] = empty;
            // }
            //
            // foreach (var indexNeeded in levelGeneration.mapGrid)
            // {
            //
            //     var roomNeeded = indexConverter[indexNeeded];
            //
            //     if (indexNeeded == 0)
            //     {
            //         mapGirdConverted[rowNum][columnNum] = null;
            //     }
            //     else
            //     {
            //         mapGirdConverted[rowNum][columnNum] = data.Rooms[roomNeeded.Item1][roomNeeded.Item2];
            //     }
            //
            //     if (columnNum % 7 == 0 && columnNum != 0)
            //     {
            //         rowNum++;
            //         columnNum = 0;
            //     }
            //     else
            //     {
            //         columnNum++;
            //     }
            // }

            LevelData data = content.Load<LevelData>(path);
            RoomListData roomListData = content.Load<RoomListData>("LevelOne/RoomsList");
            var generatedGrid = levelGeneration.mapGrid;


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
                    dungeon.AddRoom(loc, BuildRoom(roomListData, data, roomIndex, loc), roomListData.Rooms[roomIndex].Hidden);
                }
                loc.Y++;
                if (loc.Y == LevelGeneration.Columns)
                {
                    loc.Y = 0;
                    loc.X++;
                }
            }

            // // Load all rooms by index using RoomLoader
            // for (int r = 0; r < data.LayoutRows; r++) {
            //     for (int c = 0; c < data.LayoutColumns; c++)
            //     {
            //         Point loc = new Point(c, r);
            //         if (data.Rooms[r][c] != null)
            //         {
            //             dungeon.AddRoom(loc, BuildRoom(data, loc), data.Rooms[loc.Y][loc.X].Hidden);
            //         }
            //     }
            // }

            // Link together stairs
            foreach(KeyValuePair<int, IDoor> st in stairs)
            {
                st.Value.SetOtherFace(stairs[stairLinks[st.Key]]);
            }
            // Link together doors on opposing sides
            var roomLocation = new Point(0, 0);
            foreach (var roomIndex in generatedGrid)
            {
                if (generatedGrid[roomLocation.X,roomLocation.Y] != 0 && roomListData.Rooms[roomIndex].NeedWall)
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

                if (roomLocation.Y == LevelGeneration.Columns)
                {
                    roomLocation.Y = 0;
                    roomLocation.X++;
                }
            }


            // // Link together doors on opposing sides
            // for (int r = 0; r < LevelGeneration.Rows; r++)
            // {
            //     for (int c = 0; c < LevelGeneration.Columns; c++)
            //     {
            //         if (data.Rooms[r][c] != null && data.Rooms[r][c].NeedWall)
            //         {
            //             // Check each exit direction
            //             for (int d=0; d<4; d++)
            //             {
            //                 // Get direction and other room's indices
            //                 Vector2 dir = Directions.GetDirectionFromIndex(d);
            //                 int or = (int)(r + dir.Y);
            //                 int oc = (int)(c + dir.X);
            //                 // Only link doors if the other room is in layout bounds
            //                 if (or >= 0 && or < data.LayoutRows && oc >= 0 && oc < data.LayoutColumns)
            //                 {
            //                     IDoor door = dungeon.GetRoomAt(new Point(c, r)).GetDoors()[d];
            //                     Room otherRoom = dungeon.GetRoomAt(new Point(oc, or));
            //                     if(otherRoom != null && otherRoom.GetDoors().Count >= 4)
            //                     {
            //                         IDoor otherDoor = otherRoom.GetDoors()[Directions.GetIndex(Directions.Opposite(dir))];
            //                         door.SetOtherFace(otherDoor);
            //                     }
            //
            //                 }
            //             }
            //         }
            //     }
            // }

            dungeon.SetDoors(doorBounds);

            dungeon.SetCompassPointer(data.CompassPoint);

            dungeon.SetStart(data.BottomSpawnPos, data.StartLevel);

            //Load Song
            songHandler.PlaySong(data.Song);

        }

        /* Creates a room from given data
        * 
        * @param lvl        LevelData to pull info from
        * @param roomIndex  index of room in LevelData.Rooms to be made
        */
        public Room BuildRoom(RoomListData roomListData, LevelData levelData, int roomIndex, Point roomCoordinates)
        {
            RoomData rd = roomListData.Rooms[roomIndex];
            Point roomIndices = roomCoordinates;
            Room room = new(rd.Hidden);

            List<IDoor> roomDoors = room.GetDoors();
            List<Character> roomNpcs = room.GetNpcs();
            List<Item> roomItems = room.GetItems();
            SceneObjectManager scene = room.GetScene();

            //If the rooms need walls. Load Wall sprite, Door sprite, and wall colliders.
            if (roomListData.Rooms[roomIndex].NeedWall)
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
            if (roomListData.Rooms[roomIndex].NeedWall)
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
                    Item it = itemFactory.MakeItem(spawn.ItemDrop, position);
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
                Item it = itemFactory.MakeItem(spawn.Type, position);
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
