using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System;

namespace Sprint.Factory.HUD
{
    internal class HUDMap : IHUD
    {

        private Vector2 position;
        private MapModel model;
        private Vector2 roomRects = new Vector2(14 * 2, 6 * 2);
        private int padding = 2 * 2;

        public HUDMap(MapModel model, Vector2 position)
        {
            this.position = position;
            this.model = model;
            model.RevealAll();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            Texture2D backingColor;
            backingColor = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            backingColor.SetData(new Color[] { Color.Black });
            spriteBatch.Draw(backingColor, new Rectangle((int)position.X, (int)position.Y, 64 * 4, 40 * 4), Color.White);

            Texture2D roomFill;
            roomFill = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            roomFill.SetData(new Color[] { Color.Blue });

            bool[,] rooms = model.GetRooms();
            for (int r = 0; r < rooms.GetLength(0); r++)
            {
                for (int c = 0; c < rooms.GetLength(1); c++)
                {
                    if (rooms[r, c])
                    {
                        spriteBatch.Draw(roomFill, new Rectangle((int)(position.X + (roomRects.X + padding) * c),
                            (int)(position.Y + (roomRects.Y + padding) * r), 
                            (int)roomRects.X, (int)roomRects.Y), Color.White);
                    }
                }
            }


        }

        public void Update(GameTime gameTime)
        {
            // throw new NotImplementedException();
        }
    }
}
