using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Sprint.Characters;
using Sprint.Door;
using Sprint.Functions.RoomTransition;
using Sprint.Input;
using Sprint.Interfaces;
using Sprint.Items;
using Sprint.Levels;
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

        // Array to generate click-through-door commands
        private Rectangle[] doorBounds;
        private IDoor[,,] doorsPerSide;

        private int levelNumber;

        public LevelLoader(ContentManager newContent, DungeonState dungeon, SpriteLoader spriteLoader)
        {
            this.content = newContent;
            this.dungeon = dungeon;
            this.spriteLoader = spriteLoader;

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

            doorsPerSide = new IDoor[4, data.LayoutRows, data.LayoutColumns];

            // Load all rooms by index using RoomLoader
            for (int r = 0; r < data.LayoutRows; r++) {
                for (int c = 0; c < data.LayoutColumns; c++)
                {
                    Point loc = new Point(c, r);
                    if (data.Rooms[r][c] != null)
                    {
                        dungeon.AddRoom(loc, BuildRoom(data, loc), data.Rooms[loc.Y][loc.X].Hidden);
                    }
                }
            }

            // Link together doors on opposing sides
            for (int r = 0; r < data.LayoutRows; r++)
            {
                for (int c = 0; c < data.LayoutColumns; c++)
                {
                    if (data.Rooms[r][c] != null && data.Rooms[r][c].NeedWall)
                    {
                        // Check each exit direction
                        for (int d=0; d<4; d++)
                        {
                            // Get direction and other room's indices
                            Vector2 dir = Directions.GetDirectionFromIndex(d);
                            int or = (int)(r + dir.Y);
                            int oc = (int)(c + dir.X);
                            // Only link doors if the other room is in layout bounds
                            if (or >= 0 && or < data.LayoutRows && oc >= 0 && oc < data.LayoutColumns)
                            {
                                doorsPerSide[d, r, c].SetOtherFace(doorsPerSide[Directions.GetIndex(Directions.Opposite(dir)), or, oc]);
                            }
                        }
                    }
                }
            }

            // Make a command that checks all doors at its position for switching rooms when middle clicked
            for (int i = 0; i < 4; i++)
            {
                IDoor[,] slice = new IDoor[data.LayoutColumns, data.LayoutRows];
                for (int r = 0; r < data.LayoutRows; r++)
                {
                    for (int c = 0; c < data.LayoutColumns; c++)
                    {
                        if (data.Rooms[r][c] != null && data.Rooms[r][c].NeedWall)
                        {
                            slice[r, c] = doorsPerSide[i, r, c];
                        }
                    }
                }
            }

            dungeon.SetDoors(doorsPerSide, doorBounds);

            dungeon.SetCompassPointer(data.CompassPoint);

            dungeon.SetStart(data.BottomSpawnPos, data.StartLevel);

        }

        /* Creates a room from given data
        * 
        * @param lvl        LevelData to pull info from
        * @param roomIndex  index of room in LevelData.Rooms to be made
        */
        public Room BuildRoom(LevelData lvl, Point roomIndices)
        {
            RoomData rd = lvl.Rooms[roomIndices.Y][roomIndices.X];

            Room room = new(rd.Hidden);

            List<IDoor> roomDoors = room.GetDoors();
            List<Character> roomNpcs = room.GetNpcs();
            List<Item> roomItems = room.GetItems();
            SceneObjectManager scene = room.GetScene();

            //If the rooms need walls. Load Wall sprite, Door sprite, and wall colliders.
            if (lvl.Rooms[roomIndices.Y][roomIndices.X].NeedWall)
            {
                //Load Wall texture
                ISprite bgSprite = spriteLoader.BuildSprite(lvl.SpriteFile, lvl.BackgroundSprite);
                BackgroundTexture bg = new BackgroundTexture(bgSprite, lvl.WallPos);
                scene.Add(bg);

                //Load Wall colliders
                foreach (Rectangle rect in lvl.OuterWalls)
                {
                    InvisibleWall ow = new InvisibleWall(rect);
                    scene.Add(ow);
                }

                // spawn player on other side of room
                // parameter list is way too long
                IDoor[] doors = new IDoor[4];
                doors[Directions.GetIndex(Directions.UP)] = MakeDoor(lvl, rd.TopExit, lvl.DoorReferences[rd.TopExit].TopSprite, lvl.TopDoorPos, lvl.BottomSpawnPos, Directions.UP, roomIndices);
                doors[Directions.GetIndex(Directions.RIGHT)] = MakeDoor(lvl, rd.RightExit, lvl.DoorReferences[rd.RightExit].RightSprite, lvl.RightDoorPos, lvl.LeftSpawnPos, Directions.RIGHT, roomIndices);
                doors[Directions.GetIndex(Directions.DOWN)] = MakeDoor(lvl, rd.BottomExit, lvl.DoorReferences[rd.BottomExit].BottomSprite, lvl.BottomDoorPos, lvl.TopSpawnPos, Directions.DOWN, roomIndices);
                doors[Directions.GetIndex(Directions.LEFT)] = MakeDoor(lvl, rd.LeftExit, lvl.DoorReferences[rd.LeftExit].LeftSprite, lvl.LeftDoorPos, lvl.RightSpawnPos, Directions.LEFT, roomIndices);


                for (int i = 0; i < doors.Length; i++)
                {
                    roomDoors.Add(doors[i]);
                    // make commands if clicked
                    doorsPerSide[i, roomIndices.Y, roomIndices.X] = doors[i];
                }
            }

            //Load Floor tile 
            float x, y, xChange;
            if (lvl.Rooms[roomIndices.Y][roomIndices.X].NeedWall)
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
                    scene.Add(MakeTile(lvl, tile, new Vector2(x, y)));
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
                roomNpcs.Add(enemyFactory.MakeEnemy(spawn.Type, new System.Numerics.Vector2(xP, yP), room));
            }

            //Load items
            foreach (ItemSpawnData spawn in rd.Items)
            {
                float xP = lvl.FloorGridPos.X + (spawn.Column + 0.5f) * lvl.TileSize.X;
                float yP = lvl.FloorGridPos.Y + (spawn.Row + 0.5f) * lvl.TileSize.Y;
                roomItems.Add(itemFactory.MakeItem(spawn.Type, new System.Numerics.Vector2(xP, yP)));
            }

            foreach (IDoor d in roomDoors)
                scene.Add(d);
            foreach (Character n in roomNpcs)
                scene.Add(n);
            foreach (Item i in roomItems)
                scene.Add(i);

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
