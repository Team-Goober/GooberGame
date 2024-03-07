using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Sprint.Characters;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Sprite;
using System.Collections.Generic;
using System.Diagnostics;
using XMLData;

namespace Sprint.Loader
{
    internal class RoomLoader
    {
        private ContentManager content;
        private SpriteLoader spriteLoader;
        private GameObjectManager objectManager;

        private TileFactory tileFactory;
        private DoorFactory doorFactory;
        private ItemFactory itemFactory;
        private EnemyFactory enemyFactory;

        public RoomLoader(ContentManager newContent, SpriteLoader spriteLoader)
        {
            this.content = newContent;
            this.spriteLoader = spriteLoader;

            tileFactory = new(spriteLoader);
            doorFactory = new(spriteLoader);
            itemFactory = new();
            enemyFactory = new(objectManager, spriteLoader);
        }

        /* Loads Position from the given XML file
        * 
        * @param path      Path to the XML file
        */
        public Room LoadFromData(LevelData lvl, int roomIndex)
        {
            RoomData rd = lvl.Rooms[roomIndex];
            Room room = new Room();

            //Load Wall texture
            ISprite bgSprite = spriteLoader.BuildSprite(lvl.SpriteFile, lvl.BackgroundSprite);
            BackgroundTexture bg = new BackgroundTexture(bgSprite, Vector2.Zero);
            room.SetBackground(bg);

            //Load Wall colliders
            foreach (Rectangle rect in lvl.OuterWalls)
            {
                InvisibleWall ow = new InvisibleWall(rect);
                room.AddInvisibleWall(ow);
            }

            /*
            //Load the four door position
            room.PutDoor(Character.Directions.UP, MakeDoor(lvl, rd.TopExit, lvl.TopDoorPos));
            room.PutDoor(Character.Directions.DOWN, MakeDoor(lvl, rd.BottomExit, lvl.BottomDoorPos));
            room.PutDoor(Character.Directions.LEFT, MakeDoor(lvl, rd.LeftExit, lvl.LeftDoorPos));
            room.PutDoor(Character.Directions.RIGHT, MakeDoor(lvl, rd.RightExit, lvl.RightDoorPos));
            */

            //Load Floor tile 
            float x = lvl.FloorGridPos.X; float y = lvl.FloorGridPos.Y;
            foreach (string row in rd.TileGrid)
            { 
                string[] str = row.Split(' ');
                foreach (string tile in str)
                {
                    room.AddTile(MakeTile(lvl, tile, new Vector2(x, y)));
                    x += lvl.TileSize.X;
                }
                x = lvl.FloorGridPos.X;
                y += lvl.TileSize.Y;
            }


            //Load enemies
            //foreach (EnemySpawnData spawn in rd.Enemies)
            //{
            //    float xP = lvl.FloorGridPos.X + spawn.Column * lvl.TileSize.X;
            //    float yP = lvl.FloorGridPos.Y + spawn.Row * lvl.TileSize.Y;
            //    room.AddEnemy(enemyFactory.MakeEnemy(spawn.Type, new System.Numerics.Vector2(xP, yP)));
            //}

            ////Load items
            //foreach (ItemSpawnData spawn in rd.Items)
            //{
            //    float xP = lvl.FloorGridPos.X + spawn.Column * lvl.TileSize.X;
            //    float yP = lvl.FloorGridPos.Y + spawn.Row * lvl.TileSize.Y;
            //    room.AddItem(itemFactory.MakeItem(spawn.Type, new System.Numerics.Vector2(xP, yP)));
            //}

            return room;
        }

        public Door MakeDoor(LevelData lvl, ExitData exit, Vector2 position)
        {
            DoorReference doorRef = lvl.DoorReferences[exit.Door];
            Door door = doorFactory.MakeDoor(doorRef.Type, lvl.SpriteFile, doorRef.SpriteName, position, lvl.DoorSize);
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
