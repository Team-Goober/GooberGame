

using Microsoft.Xna.Framework;

namespace XMLData
{
    public class LevelData
    {
        public Vector2 FloorGridPos; // World position of where the floor starts
        public int Rows, Columns; // Dimensions of the tile grid
        public Vector2 TileSize; // World dimensions of each tile on the floor
        public Dictionary<string, TileReference> TileReferences; // Mapping of labels to tile data so rooms can reuse the same tile types

        public Vector2 DoorSize; // World dimensions of each door
        public Vector2[] DoorPositions; // Positions of each door in every room, clockwise from top
        public Dictionary<string, DoorReference> DoorReferences; // Mapping of labels to door data so rooms can reuse the same door types

        public string SpriteFile; // Path to file holding all room sprites for this level
        public List<RoomData> Rooms; // All rooms in this level. First index is the starting room.

    }

    public class TileReference
    {
        public string SpriteName; // Name of the sprite to draw for this tile. Name is used in the Level's SpriteFile
        public string Type; // Tile type to construct. Used by TileFactory
    }

    public class DoorReference
    {
        public string OpenSprite; // Name of sprite to draw when open. Name is used in the Level's SpriteFile. May be empty
        public string ClosedSprite; // Name of sprite to draw when closed. Name is used in the Level's SpriteFile. May be empty
        public string Type; // Door type to construct. Used by DoorFactory
    }

    public class RoomData
    {
        public ExitData[] Exits; // Each exit for the room. Corresponds to one door each. Clockwise from top
        public List<string> TileGrid; // Grid of tiles. Each string is a row of tiles separated by spaces. Usees LevelData.TileReferences keys
        public List<ItemSpawnData> Items; // Spawn data for every item in the room
        public List<EnemySpawnData> Enemies; // Spawn data for every enemy in the room
    }

    public class ExitData
    {
        public string Door; // The type of door to place. Uses LevelData.DoorReferences keys
        public int AdjacentRoom; // Index in LevelData.Rooms of the Room on the other side of this exit. -1 if none
    }

    public class ItemSpawnData
    {
        public string Type; // Type of item to construct. Used by ItemFactory
        public int Row, Column; // Grid coordinates of tile to spawn on
    }

    public class EnemySpawnData
    {
        public string Type; // Type of enemy to construct. Used by EnemyFactory
        public int Row, Column; // Grid coordinates of tile to spawn on
    }

}
