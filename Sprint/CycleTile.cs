using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;
using System.Collections.Generic;


namespace Sprint
{
    //Later be refactored into level loader
    internal class CycleTile
    {
        private List<IGameObject> tiles = new();
        private int currentTileIndex = 0;
        private Vector2 position;

        private const string ANIM_FILE = "tileAnims";

        public CycleTile(Goober game, Vector2 pos, SpriteLoader spriteLoader)
        {
            this.position = pos;
            // Load textures and set up animations for enemies
            // Add enemies to the 'enemies' list

            CreateTile(game, "flat", spriteLoader);   // Tile 1
            CreateTile(game, "bevel", spriteLoader);  // Tile 2
            CreateTile(game, "fish", spriteLoader);  // Tile 3
            CreateTile(game, "dragon", spriteLoader);  // Tile 4

            CreateTile(game, "dark", spriteLoader);   // Tile 5
            CreateTile(game, "sand", spriteLoader);  // Tile 6
            CreateTile(game, "light", spriteLoader);  // Tile 7
            CreateTile(game, "stairs", spriteLoader);  // Tile 8

            CreateTile(game, "bricks", spriteLoader);   // Tile 9
            CreateTile(game, "slats", spriteLoader);  // Tile 10

        }

        private void CreateTile(Goober game, string blockName, SpriteLoader spriteLoader)
        {
            ISprite tileSprite = spriteLoader.BuildSprite(ANIM_FILE, blockName);

            Tiles tile = new(game, tileSprite, position);

            tiles.Add(tile);
        }

        public void NextTile()
        {
            currentTileIndex = (currentTileIndex + 1) % tiles.Count;
        }

        public void PreviousTile()
        {
            currentTileIndex = (currentTileIndex - 1 + tiles.Count) % tiles.Count;
        }

        public void Update(GameTime gameTime)
        {
            tiles[currentTileIndex].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            tiles[currentTileIndex].Draw(spriteBatch, gameTime);
        }
    }

}

