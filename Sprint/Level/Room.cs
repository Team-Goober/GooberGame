using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;
using System.Collections.Generic;
using Sprint.Loader;
using Microsoft.Xna.Framework.Content;

namespace Sprint.Level
{
    internal class Room
    {
        private List<IGameObject> tiles = new List<IGameObject>();

        RoomTileLoader pl;

        private const string ANIM_FILE = "XML/LevelOne";
        private const string POS_FILE = "XML/LevelOnePos";

        public Room(ContentManager content, SpriteLoader spriteLoader) 
        {
            pl = new RoomTileLoader(content);
            pl.LoadXML(POS_FILE);

            createRoomPart("roomOneExterior", spriteLoader);
            createRoomPart("roomOneTopDoor", spriteLoader);
            createRoomPart("roomOneLeftDoor", spriteLoader);
            createRoomPart("roomOneRightDoor", spriteLoader);
            createRoomPart("roomOneDownDoor", spriteLoader);
            createFloorTiles(spriteLoader);

        }

        /// <summary>
        /// Loads this room into the game world
        /// </summary>
        /// <param name="objectManager">GameObjectManager to add objects to</param>
        public void LoadIn(GameObjectManager objectManager)
        {
            foreach (IGameObject tile in tiles)
            {
                objectManager.Add(tile);
            }
        }

        /// <summary>
        /// Removes this room from the game world
        /// </summary>
        /// <param name="objectManager">GameObjectManager to remove objects from</param>
        public void Unload(GameObjectManager objectManager)
        {
            foreach (IGameObject tile in tiles)
            {
                objectManager.Remove(tile);
            }
        }

        /// <summary>
        /// Creates tile for an outer wall of a room
        /// </summary>
        /// <param name="game"></param>
        /// <param name="roomName">Name of the room in XML file</param>
        /// <param name="spriteLoader"></param>
        private void createRoomPart(string partName, SpriteLoader spriteLoader)
        {
            ISprite roomSprite = spriteLoader.BuildSprite(ANIM_FILE, partName);

            Tiles roomPart = new(roomSprite, pl.GetPosition(partName));

            tiles.Add(roomPart);
        }

        /// <summary>
        /// Creates tiles for each cell on the floor
        /// </summary>
        /// <param name="game"></param>
        /// <param name="spriteLoader"></param>
        private void createFloorTiles(SpriteLoader spriteLoader)
        {
            foreach (var floor in pl.GetFloor())
            {
                ISprite floorSprite = spriteLoader.BuildSprite(ANIM_FILE, floor.tile);

                Tiles floorPart = new(floorSprite, floor.position);

                tiles.Add(floorPart);

            }
        }

    }
}
