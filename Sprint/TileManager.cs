using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint
{
    //Later be refactored into level loader
    internal class TileManager
    {
        private List<ISprite> tiles = new List<ISprite>();
        private int currentTileIndex = 0;

        public TileManager(Game1 game)
        {
            // Load textures and set up animations for enemies
            // Add enemies to the 'enemies' list

            CreateTile(game, "floor_tiles", 0, 0, 16, 16, new Vector2(8, 8), 2);   // Tile 1
            CreateTile(game, "floor_tiles", 18, 0, 16, 16, new Vector2(8, 8), 2);  // Tile 2
            CreateTile(game, "floor_tiles", 35, 0, 16, 16, new Vector2(8, 8), 2);  // Tile 3
            CreateTile(game, "floor_tiles", 52, 0, 16, 16, new Vector2(8, 8), 2);  // Tile 4

            CreateTile(game, "floor_tiles", 0, 18, 16, 16, new Vector2(8, 8), 2);   // Tile 5
            CreateTile(game, "floor_tiles", 18, 18, 16, 16, new Vector2(8, 8), 2);  // Tile 6
            CreateTile(game, "floor_tiles", 35, 18, 16, 16, new Vector2(8, 8), 2);  // Tile 7
            CreateTile(game, "floor_tiles", 52, 18, 16, 16, new Vector2(8, 8), 2);  // Tile 8

            CreateTile(game, "floor_tiles", 0, 35, 16, 16, new Vector2(8, 8), 2);   // Tile 9
            CreateTile(game, "floor_tiles", 18, 35, 16, 16, new Vector2(8, 8), 2);  // Tile 10

        }

        private void CreateTile(Game1 game, string textureName, int x, int y, int width, int height, Vector2 center, int scale)
        {
            Texture2D blockTexture = game.Content.Load<Texture2D>(textureName);
            ISprite blockSprite = new AnimatedSprite(blockTexture);
            IAtlas blockAtlas = new SingleAtlas(new Rectangle(x, y, width, height), center);
            blockSprite.RegisterAnimation("default", blockAtlas);
            blockSprite.SetAnimation("default");
            blockSprite.SetScale(scale);
            tiles.Add(blockSprite);
        }

        public void NextTile()
        {
            currentTileIndex = (currentTileIndex + 1) % tiles.Count;
        }

        public void PreviousTile()
        {
            currentTileIndex = (currentTileIndex - 1 + tiles.Count) % tiles.Count;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            tiles[currentTileIndex].Draw(spriteBatch, position, gameTime);
        }
    }

}

