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

        public Room(Goober game, ContentManager content, SpriteLoader spriteLoader) 
        {
            pl = new RoomTileLoader(content);
            pl.LoadXML(POS_FILE);

            CreateRoomPart(game, "roomOneExterior", spriteLoader);
            CreateRoomPart(game, "roomOneTopDoor", spriteLoader);
            CreateRoomPart(game, "roomOneLeftDoor", spriteLoader);
            CreateRoomPart(game, "roomOneRightDoor", spriteLoader);
            CreateRoomPart(game, "roomOneDownDoor", spriteLoader);
            CreatFloorTiles(game, spriteLoader);

        }

        /// <summary>
        /// Creates tile for an outer wall of a room
        /// </summary>
        /// <param name="game"></param>
        /// <param name="roomName">Name of the room in XML file</param>
        /// <param name="spriteLoader"></param>
        private void CreateRoomPart(Goober game, string partName, SpriteLoader spriteLoader)
        {
            ISprite roomSprite = spriteLoader.BuildSprite(ANIM_FILE, partName);

            Tiles roomPart = new(game, roomSprite, pl.GetPosition(partName));

            tiles.Add(roomPart);
        }

        /// <summary>
        /// Creates tiles for each cell on the floor
        /// </summary>
        /// <param name="game"></param>
        /// <param name="spriteLoader"></param>
        private void CreatFloorTiles(Goober game, SpriteLoader spriteLoader)
        {
            foreach (var floor in pl.GetFloor())
            {
                ISprite floorSprite = spriteLoader.BuildSprite(ANIM_FILE, floor.tile);

                Tiles floorPart = new(game, floorSprite, floor.position);

                tiles.Add(floorPart);

            }
        }

        public void Update(GameTime gameTime)
        {
            // None For Now
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for(int i = 0; i < tiles.Count; i++)
            {
                tiles[i].Draw(spriteBatch, gameTime);
            }

        }
    }
}
