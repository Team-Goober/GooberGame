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
            lvl.SpriteFile = "LevelOne/Level1";
            lvl.BackgroundSprite = "background";

            int s = 4; //scaling up from texture file
            lvl.OuterWalls = new Rectangle[] {
                new Rectangle(0*s, 0*s, 112*s, 32*s),
                new Rectangle(0*s, 0*s, 32*s, 72*s),
                new Rectangle(144*s, 0*s, 112*s, 32*s),
                new Rectangle(224*s, 0*s, 32*s, 72*s),
                new Rectangle(144*s, 144*s, 112*s, 32*s),
                new Rectangle(224*s, 104*s, 32*s, 72*s),
                new Rectangle(0*s, 144*s, 112*s, 32*s),
                new Rectangle(0*s, 104*s, 32*s, 72*s),
            };

            lvl.FloorGridPos = new Vector2(128, 128);
            lvl.TileSize = new Vector2(64, 64);
            lvl.TileReferences = new();

            lvl.DoorSize = new Vector2(128, 128);
            lvl.TopDoorPos = new Vector2(448, 0);
            lvl.BottomDoorPos = new Vector2(896, 288);
            lvl.RightDoorPos = new Vector2(448, 576);
            lvl.LeftDoorPos = new Vector2(0, 288);

            lvl.DoorReferences = new();

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
            emptyRoom.TopExit = new ExitData();
            emptyRoom.TopExit.Door = "open";
            emptyRoom.TopExit.AdjacentRoom = 1;
            emptyRoom.BottomExit = new ExitData();
            emptyRoom.BottomExit.Door = "none";
            emptyRoom.BottomExit.AdjacentRoom = -1;
            emptyRoom.RightExit = new ExitData();
            emptyRoom.RightExit.Door = "none";
            emptyRoom.RightExit.AdjacentRoom = -1;
            emptyRoom.LeftExit = new ExitData();
            emptyRoom.LeftExit.Door = "none";
            emptyRoom.LeftExit.AdjacentRoom = -1;
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
            emptyRoom.TopExit = new ExitData();
            emptyRoom.TopExit.Door = "open";
            emptyRoom.TopExit.AdjacentRoom = -1;
            emptyRoom.BottomExit = new ExitData();
            emptyRoom.BottomExit.Door = "none";
            emptyRoom.BottomExit.AdjacentRoom = 0;
            emptyRoom.RightExit = new ExitData();
            emptyRoom.RightExit.Door = "none";
            emptyRoom.RightExit.AdjacentRoom = -1;
            emptyRoom.LeftExit = new ExitData();
            emptyRoom.LeftExit.Door = "none";
            emptyRoom.LeftExit.AdjacentRoom = -1;
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
