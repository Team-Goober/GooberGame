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
        private SfxFactory sfxFactory;

        // Array to generate click-through-door commands
        private Rectangle[] doorBounds;
        private IDoor[,,] doorsPerSide;
        // Dictionaries to link doors that aren't on cardinal dirs
        private Dictionary<int, IDoor> stairs;
        private Dictionary<int, int> stairLinks;

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
            songHandler = SongHandler.GetInstance();
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
            stairs = new();
            stairLinks = new();

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

            // Link together stairs
            foreach(KeyValuePair<int, IDoor> st in stairs)
            {
                st.Value.SetOtherFace(stairs[stairLinks[st.Key]]);
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

            //Load Song
            songHandler.PlaySong(data.Song);

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

            List<ITile> normTiles = new List<ITile>();
            List<ITile> moveTiles = new List<ITile>();
            
            foreach (string row in rd.TileGrid)
            {
                string[] str = row.Split(' ');
                foreach (string tile in str)
                {
                    if (tile.Equals("X"))
                    {
                        moveTiles.Add(MakeTile(lvl, tile, new Vector2(x, y)));
                        normTiles.Add(MakeTile(lvl, "0", new Vector2(x, y)));
                    }
                    else
                    {
                        normTiles.Add(MakeTile(lvl, tile, new Vector2(x, y)));
                    }
                    x += lvl.TileSize.X;
                }
                x = xChange;
                y += lvl.TileSize.Y;
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
                Vector2 position = lvl.FloorGridPos + (spawn.TilePos + new Vector2(0.5f)) * lvl.TileSize;
                Enemy en = enemyFactory.MakeEnemy(spawn.Type, position, room);
                // Give item drop
                if(spawn.ItemDrop != null)
                {
                    //en.GiveDrop(itemFactory.MakeItem(spawn.ItemDrop, position));
                }
                roomNpcs.Add(en);
            }

            //Load items
            foreach (ItemSpawnData spawn in rd.Items)
            {
                Vector2 position = lvl.FloorGridPos + (spawn.TilePos + new Vector2(0.5f)) * lvl.TileSize;
                //roomItems.Add(itemFactory.MakeItem(spawn.Type, position));
                switch(spawn.Type)
                {
                    case "heart":
                        roomItems.Add(new Item(position,
                            new InstantPowerup(
                                spriteLoader.BuildSprite("itemAnims", "heart"), 
                                new HealPlayerEffect(1),
                                "heart",
                                "HEART|heals one heart")));
                        break;
                    case "redRing":
                        roomItems.Add(new Item(position,
                            new PassivePowerup(
                                spriteLoader.BuildSprite("itemAnims", "redRing"),
                                new ChangeSpeedEffect(CharacterConstants.PLAYER_SPEED * 2),
                                "redRing",
                                "RING|doubles run speed")));
                        break;
                    case "rupee":
                        IStackedPowerup gem = new ResourcePowerup(
                                spriteLoader.BuildSprite("itemAnims", "rupee"),
                                null,
                                "rupee",
                                "RUPEE|can be traded");
                        gem.AddAmount(5);
                        roomItems.Add(new Item(position, gem));
                        break;
                    case "key":
                        IStackedPowerup key = new ResourcePowerup(
                                spriteLoader.BuildSprite("itemAnims", "key"),
                                null,
                                "key",
                                "KEY|unlocks doors");
                        key.AddAmount(1);
                        roomItems.Add(new Item(position, key));
                        break;
                    case "bow":
                        roomItems.Add(new Item(position, 
                            new ActiveAbility(
                                spriteLoader.BuildSprite("itemAnims", "bow"),
                                new SpawnProjectileEffect("arrow"),
                                "bow",
                                "BOW|shoots arrows")));
                        break;
                    case "meat":
                        IStackedPowerup meat = new ConsumableAbility(
                                spriteLoader.BuildSprite("itemAnims", "meat"),
                                new HealPlayerEffect(2),
                                "meat",
                                "MEAT|heals 2 hearts");
                        meat.AddAmount(2);
                        roomItems.Add(new Item(position, meat));
                        break;
                    case "greenBadge":
                        IUpgradePowerup greenUpgrade = new UpgradeAbility(
                                spriteLoader.BuildSprite("itemAnims", "greenBadge"),
                                new DualShotUpgrade(),
                                "greenBadge",
                                "- dual shot");
                        greenUpgrade.SetUpgradeOptions(new() { "bow" });
                        roomItems.Add(new Item(position, greenUpgrade));
                        break;
                    case "blueBadge":
                        IUpgradePowerup blueUpgrade = new UpgradeAbility(
                                spriteLoader.BuildSprite("itemAnims", "blueBadge"),
                                new TripleShotUpgrade(),
                                "blueBadge",
                                "- triple shot");
                        blueUpgrade.SetUpgradeOptions(new() { "bow" });
                        roomItems.Add(new Item(position, blueUpgrade));
                        break;
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
