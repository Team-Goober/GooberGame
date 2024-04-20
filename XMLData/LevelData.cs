

using Microsoft.Xna.Framework;

namespace XMLData
{
    public class LevelData
    {
        public int Level;
        public Point StartLevel; // Column, Row pair for room to start player in
        public Point CompassPoint; // Column, Row pair for room to place a compass signal in
        public string SpriteFile; // Path to file holding all room sprites for this level
        public string HUBFile;

        public string Song;    //Song for the level
        public List<SfxData> Sfxs; //All sound effects in the level

        public Rectangle[] OuterWalls; // Collision bounds of walls on the outside of the room
        public string BackgroundSprite; // is drawn at 0,0 and should show the background walls

        public Vector2 ArenaOffset; // Position of the playable area on screen. Wall and floor are relative to this position
        public Vector2 WallPos; // Position of where walls will spawn
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

        public int LayoutColumns; // Dimensions of room layout grid
        public int LayoutRows;

    }

    public class TileReference
    {
        public string SpriteName; // Name of the sprite to draw for this tile. Name is used in the Level's SpriteFile
        public string Type; // Tile type to construct. Used by TileFactory
    }

    public class DoorReference
    {
        public string TopSprite; // Name of sprite to draw. Name is used in the Level's SpriteFile. Should have animations named "open" and "closed"
        public string BottomSprite;
        public string LeftSprite;
        public string RightSprite;
        public string Type; // Door type to construct. Used by DoorFactory
    }

    public class SfxData
    {
        public string SoundName;
    }
}
