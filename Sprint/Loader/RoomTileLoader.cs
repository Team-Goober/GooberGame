using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Diagnostics;
using XMLData;

namespace Sprint.Loader
{
    internal class RoomTileLoader
    {
        private ContentManager content;

        private Dictionary<string, Vector2> loaded;
        //Make into XML?
        private Dictionary<string, string> TileDictionary;
        private List<(string tile, Vector2 position)> tiles;

        public RoomTileLoader(ContentManager newContent)
        {
            loaded = new Dictionary<string, Vector2>();
            tiles = new List<(string tile, Vector2 position)>();
            TileDictionary = new Dictionary<string, string>
            {
                { "1", "blueTile" }
            };

            this.content = newContent;
        }

        /* Loads Position from the given XML file
        * 
        * @param path      Path to the XML file
        */
        public void LoadXML(string path)
        {
            RoomData pd = content.Load<RoomData>(path);
            //Load Wall Position
            loaded.Add("roomOneExterior", pd.Wall);

            //Load the four door position
            loaded.Add("roomOneTopDoor", pd.TopDoor);
            loaded.Add("roomOneLeftDoor", pd.LeftDoor);
            loaded.Add("roomOneRightDoor", pd.RightDoor);
            loaded.Add("roomOneDownDoor", pd.BottomDoor);

            //Load Floor position
            int x = pd.XTile; int y = pd.YTile;
            foreach (string row in pd.Tile)
            { 
                string[] str = row.Split(' ');
                foreach (string tile in str)
                {
                    tiles.Add((TileDictionary[tile], new Vector2(x, y)));
                    x += 64;
                }
                x = pd.XTile;
                y += 64;
            }
        }

        // Returns position for a given tile name
        public Vector2 GetPosition(string key)
        {
            return loaded[key];
        }

        // Returns a list of tiles and their accompanying positions for the floor of this level
        public List<(string tile, Vector2 position)> GetFloor()
        {
            return tiles;
        }
    }
}
