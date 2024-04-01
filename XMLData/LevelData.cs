

using Microsoft.Xna.Framework;

namespace XMLData
{
    public class LevelData
    {
        public int Level;
        public int StartLevel;
        public string SpriteFile; // Path to file holding all room sprites for this level
        public string HUBFile;

        public string Song;    //Song for the level
        public List<SfxData> Sfxs; //All sound effects in the level

        public Rectangle[] OuterWalls; // Collision bounds of walls on the outside of the room
        public string BackgroundSprite; // is drawn at 0,0 and should show the background walls

        public Vector2 WallPos; //Position of where walls will spawn
        public Vector2 FloorGridPos; // World position of where the floor starts
        public Vector2 ZeroZeroPos; // If there are no walls. The floor position will be this instead.
        public Vector2 TileSize; // World dimensions of each tile on the floor
        public Dictionary<string, TileReference> TileReferences; // Mapping of labels to tile data so rooms can reuse the same tile types

        public Vector2 DoorSize; // World dimensions of each door
        public Vector2 OpenDoorSize; // Size of door collider (trigger for next room) when its open
        public Vector2 TopDoorPos; // Positions of each door in every room
        public Vector2 BottomDoorPos;
        public Vector2 LeftDoorPos;
        public Vector2 RightDoorPos;
        public Vector2 TopSpawnPos; // Positions of where each door should place player in the next room. Direction represents the side of the room
        public Vector2 BottomSpawnPos; // the player should end up on, not the side the entrance door was on
        public Vector2 LeftSpawnPos;
        public Vector2 RightSpawnPos;
        public Dictionary<string, DoorReference> DoorReferences; // Mapping of labels to door data so rooms can reuse the same door types

        public List<RoomData> Rooms; // All rooms in this level. First index is the starting room.

    }

    public class TileReference
    {
        public string SpriteName; // Name of the sprite to draw for this tile. Name is used in the Level's SpriteFile
        public string Type; // Tile type to construct. Used by TileFactory
    }

    public class DoorReference
    {
        public string SpriteName; // Name of sprite to draw. Name is used in the Level's SpriteFile. Should have animations named "open" and "closed"
        public string Type; // Door type to construct. Used by DoorFactory
    }

    public class RoomData
    {
        public bool NeedWall; // Determined if walls will be loaded for a room 
        public ExitData TopExit; // Each exit for the room. Corresponds to one door each
        public ExitData BottomExit;
        public ExitData LeftExit;
        public ExitData RightExit;
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

    public class SfxData
    {
        public string SoundName;
    }
}
