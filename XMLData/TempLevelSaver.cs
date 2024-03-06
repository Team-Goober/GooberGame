using Microsoft.Xna.Framework;
using System.IO;
using System.Xml;

namespace XMLData
{
    public class TempLevelSaver
    {

        public TempLevelSaver(string path)
        {

            LevelData lvl = new LevelData();

            // set up LevelDat params
            lvl.FloorGridPos = new Vector2(128, 128);
            lvl.Rows = 7;
            lvl.Columns = 12;
            lvl.TileSize = new Vector2(64, 64);
            lvl.TileReferences = new();

            lvl.DoorSize = new Vector2(128, 128);
            lvl.DoorPositions = new Vector2[] { new Vector2(448, 0), new Vector2(896, 288), new Vector2(448, 576), new Vector2(0, 288) };
            lvl.DoorReferences = new();

            lvl.SpriteFile = "LevelOne/Level1";
            lvl.Rooms = new();

            //set up TileReferences
            TileReference floorTile = new TileReference();
            floorTile.SpriteName = "flatTile";
            floorTile.Type = "floor";
            lvl.TileReferences.Add("0", floorTile);

            TileReference blockTile = new TileReference();
            blockTile.SpriteName = "bevelTile";
            blockTile.Type = "wall";
            lvl.TileReferences.Add("1", blockTile);


            //set up DoorReferences
            DoorReference openExit = new DoorReference();
            openExit.OpenSprite = "openDoor";
            openExit.ClosedSprite = "";
            openExit.Type = "open";
            lvl.DoorReferences.Add("open", openExit);

            DoorReference noExit = new DoorReference();
            noExit.OpenSprite = "noneDoor";
            noExit.ClosedSprite = "";
            noExit.Type = "none";
            lvl.DoorReferences.Add("none", noExit);


            //set up Rooms
            RoomData emptyRoom = new RoomData();
            emptyRoom.Exits = new ExitData[] { new ExitData(), new ExitData(), new ExitData(), new ExitData() };
            emptyRoom.Exits[0].Door = "open";
            emptyRoom.Exits[0].AdjacentRoom = 1;
            emptyRoom.Exits[1].Door = "none";
            emptyRoom.Exits[1].AdjacentRoom = -1;
            emptyRoom.Exits[2].Door = "none";
            emptyRoom.Exits[2].AdjacentRoom = -1;
            emptyRoom.Exits[3].Door = "none";
            emptyRoom.Exits[3].AdjacentRoom = -1;
            emptyRoom.TileGrid = new()
            {
                "0 0 0 0 0 0 0 0 0 0 0 0",
                "0 0 0 0 0 0 0 0 0 0 0 0",
                "0 0 0 0 0 0 0 0 0 0 0 0",
                "0 0 0 0 0 0 0 0 0 0 0 0",
                "0 0 0 0 0 0 0 0 0 0 0 0",
                "0 0 0 0 0 0 0 0 0 0 0 0",
                "0 0 0 0 0 0 0 0 0 0 0 0"
            };
            emptyRoom.Items = new();
            emptyRoom.Enemies = new();
            lvl.Rooms.Add(emptyRoom);

            RoomData betterRoom = new RoomData();
            betterRoom.Exits = new ExitData[] { new ExitData(), new ExitData(), new ExitData(), new ExitData() };
            betterRoom.Exits[0].Door = "none";
            betterRoom.Exits[0].AdjacentRoom = -1;
            betterRoom.Exits[1].Door = "none";
            betterRoom.Exits[1].AdjacentRoom = -1;
            betterRoom.Exits[2].Door = "open";
            betterRoom.Exits[2].AdjacentRoom = 0;
            betterRoom.Exits[3].Door = "none";
            betterRoom.Exits[3].AdjacentRoom = -1;
            betterRoom.TileGrid = new()
            {
                "0 0 0 0 0 0 0 0 0 0 0 0",
                "0 0 0 1 0 0 0 0 0 0 0 0",
                "0 0 0 0 0 0 0 0 1 0 0 0",
                "0 0 1 0 0 0 0 0 1 1 0 0",
                "0 0 0 0 0 0 0 0 1 0 0 0",
                "0 0 0 1 0 0 0 0 0 0 0 0",
                "0 0 0 0 0 0 0 0 0 0 0 0"
            };
            betterRoom.Items = new();
            ItemSpawnData item = new();
            item.Type = "triforce";
            item.Row = 6;
            item.Column = 3;
            betterRoom.Items.Add(item);
            betterRoom.Enemies = new();
            EnemySpawnData enemy = new();
            enemy.Type = "skeleton";
            enemy.Row = 5;
            enemy.Column = 3;
            betterRoom.Enemies.Add(enemy);
            lvl.Rooms.Add(betterRoom);


            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {
                Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate.
                IntermediateSerializer.Serialize(writer, lvl, null);
            }

        }

    }
}
