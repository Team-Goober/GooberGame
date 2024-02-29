using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;
using System.Collections.Generic;

namespace Sprint.Level
{
    internal class LevelOne
    {
        private List<IGameObject> tiles = new List<IGameObject>();
        private int currentRoomIndex = 0;
        private const string ANIM_FILE = "XML/LevelOne";

        public LevelOne(Goober game, SpriteLoader spriteLoader) 
        {
            CreateRoom(game, "roomOneExterior", new Vector2(0, 0), spriteLoader);
            CreateRoom(game, "roomOneTopDoor", new Vector2(447, 0), spriteLoader);
            CreateRoom(game, "roomOneLeftDoor", new Vector2(0, 288), spriteLoader);
            CreateRoom(game, "roomOneRightDoor", new Vector2(895, 288), spriteLoader);
            CreateRoom(game, "roomOneDownDoor", new Vector2(447, 576), spriteLoader);
            CreateRoom(game, "roomOneFloor", new Vector2(127, 128), spriteLoader);
        }

        private void CreateRoom(Goober game, string roomName, Vector2 position, SpriteLoader spriteLoader)
        {
            ISprite roomSprite = spriteLoader.BuildSprite(ANIM_FILE, roomName);

            Tiles roomPart = new(game, roomSprite, position);

            tiles.Add(roomPart);
        }

        public void Update(GameTime gameTime)
        {
            tiles[currentRoomIndex].Update(gameTime);
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
