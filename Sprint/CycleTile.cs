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

        public CycleTile(Goober game, Vector2 pos)
        {
            this.position = pos;
            // Load textures and set up animations for enemies
            // Add enemies to the 'enemies' list

            CreateTile(game, "tiles", 0, 0, 16, 16, new Vector2(8, 8), 2);   // Tile 1
            CreateTile(game, "tiles", 17, 0, 16, 16, new Vector2(8, 8), 2);  // Tile 2
            CreateTile(game, "tiles", 34, 0, 16, 16, new Vector2(8, 8), 2);  // Tile 3
            CreateTile(game, "tiles", 52, 0, 16, 16, new Vector2(8, 8), 2);  // Tile 4

            CreateTile(game, "tiles", 0, 17, 16, 16, new Vector2(8, 8), 2);   // Tile 5
            CreateTile(game, "tiles", 17, 17, 16, 16, new Vector2(8, 8), 2);  // Tile 6
            CreateTile(game, "tiles", 34, 17, 16, 16, new Vector2(8, 8), 2);  // Tile 7
            CreateTile(game, "tiles", 52, 17, 16, 16, new Vector2(8, 8), 2);  // Tile 8

            CreateTile(game, "tiles", 0, 34, 16, 16, new Vector2(8, 8), 2);   // Tile 9
            CreateTile(game, "tiles", 17, 34, 16, 16, new Vector2(8, 8), 2);  // Tile 10

        }

        private void CreateTile(Goober game, string textureName, int x, int y, int width, int height, Vector2 center, int scale)
        {
            Texture2D tileTexture = game.Content.Load<Texture2D>(textureName);
            ISprite tileSprite = new AnimatedSprite(tileTexture);
            IAtlas tileAtlas = new SingleAtlas(new Rectangle(x, y, width, height), center);
            tileSprite.RegisterAnimation("default", tileAtlas);
            tileSprite.SetAnimation("default");
            tileSprite.SetScale(scale);

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

