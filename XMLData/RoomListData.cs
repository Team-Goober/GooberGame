

using Microsoft.Xna.Framework;

namespace XMLData
{
    public class RoomListData
    {
        public RoomData[] Rooms; // All rooms in this level. First index is the starting room.
    }

    public class RoomData
    {
        public bool NeedWall; // Determines if walls will be loaded for a room
        public bool Hidden; // Determines whether room is hidden after receiving map
        public string TopExit; // Each exit for the room. Corresponds to one door each
        public string BottomExit;
        public string LeftExit;
        public string RightExit;
        public List<string> TileGrid; // Grid of tiles. Each string is a row of tiles separated by spaces. Usees LevelData.TileReferences keys
        public List<ItemSpawnData> Items; // Spawn data for every item in the room
        public List<EnemySpawnData> Enemies; // Spawn data for every enemy in the room
        public List<TextBoxData> TextBoxes; // Data for textboxes placed in room
        public List<StairData> Stairs; // Doors that arent in one of the four typical positions
    }

    public class ItemSpawnData
    {
        public string Type; // Type of item to construct. Used by ItemFactory
        public Vector2 TilePos; // Grid coordinates of tile to spawn on. Not in world coordinates
    }

    public class EnemySpawnData
    {
        public string Type; // Type of enemy to construct. Used by EnemyFactory
        public Vector2 TilePos; // Grid coordinates of tile to spawn on. Not in world coordinates
        public string? ItemDrop; // Item to drop on enemy death
    }

    public class TextBoxData
    {
        public string FontName; // Name of font to draw with
        public List<string> Text; // Lines of text to draw
        public Vector2 CharacterDimensions; // Average size of a single character
        public Vector2 Position; // Position of center of top line of text
        public Color Color; // Color mask for text
    }

    public class StairData
    {
        public Vector2 Position; // World boundaries of the stair
        public Vector2 Size;
        public int IDNum; // Number to link to this stair with
        public int OtherSideID; // Stair to link to
        public Vector2 SpawnPosition; // Place to spawn the player when exiting
    }
}