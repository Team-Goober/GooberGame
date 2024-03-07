using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Sprite;
using System.Collections.Generic;
using Sprint.Levels;


namespace Sprint
{
    //Later be refactored into level loader
    internal class CycleTile
    {
        private List<ITile> tiles = new();
        private GameObjectManager objManager;

        private int currentTileIndex;
        private Vector2 position;

        private const string ANIM_FILE = "tileAnims";

        public CycleTile(Goober game, Vector2 pos, GameObjectManager objManager, SpriteLoader spriteLoader)
        {
            this.position = pos;
            this.objManager = objManager;
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

            SwitchTile(null, tiles[0]);
        }

        private void CreateTile(Goober game, string blockName, SpriteLoader spriteLoader)
        {
            ISprite tileSprite = spriteLoader.BuildSprite(ANIM_FILE, blockName);

            ITile tile = new WallTile(tileSprite, position, new Vector2(16, 16));

            tiles.Add(tile);
        }

        public void NextTile()
        {
            int before = currentTileIndex;
            currentTileIndex = (currentTileIndex + 1) % tiles.Count;
            SwitchTile(tiles[before], tiles[currentTileIndex]);
        }

        public void PreviousTile()
        {
            int before = currentTileIndex;
            currentTileIndex = (currentTileIndex - 1 + tiles.Count) % tiles.Count;
            SwitchTile(tiles[before], tiles[currentTileIndex]);
        }

        public void SwitchTile(ITile oldT, ITile newT)
        {
            if(oldT!=null)
                objManager.Remove(oldT);
            if(newT!=null)
                objManager.Add(newT);
        }

    }

}

