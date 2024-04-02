using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System;

namespace Sprint.HUD
{
    internal class HUDMiniMap : IHUD
    {

        private Vector2 position;
        private MapModel model;
        private Vector2 roomRects;
        private int padding;
        private Vector2 bgSize;

        public HUDMiniMap(MapModel model, Vector2 position, Vector2 roomSize, int padding, Vector2 bgSize)
        {
            this.position = position;
            this.model = model;
            this.roomRects = roomSize;
            this.padding = padding;
            this.bgSize = bgSize;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Draw black background box for minimap
            Texture2D backingColor;
            backingColor = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            backingColor.SetData(new Color[] { Color.Black });
            spriteBatch.Draw(backingColor, new Rectangle((int)position.X, (int)position.Y, (int)bgSize.X, (int)bgSize.Y), Color.White);

            // Draw blue rectangle for every visible room
            Texture2D roomFill;
            roomFill = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            roomFill.SetData(new Color[] { Color.Blue });

            bool[,] rooms = model.GetRevealedRooms();
            for (int r = 0; r < rooms.GetLength(0); r++)
            {
                for (int c = 0; c < rooms.GetLength(1); c++)
                {
                    if (rooms[r, c])
                    {
                        spriteBatch.Draw(roomFill, new Rectangle((int)(position.X + (roomRects.X + padding) * c),
                            (int)(position.Y + (roomRects.Y + padding) * r),
                            (int)roomRects.X, (int)roomRects.Y),
                            Color.White);
                    }
                }
            }

            // Draw compass pointer in triforce room
            Point compassPos = model.GetCompassPosition();
            // Only draw if posiiton is placed (x value is positive)
            if(compassPos.X >= 0)
            {
                Texture2D compassPointer;
                compassPointer = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                compassPointer.SetData(new Color[] { Color.Red });

                spriteBatch.Draw(compassPointer, new Rectangle((int)(position.X + (roomRects.X + padding) * compassPos.X + (roomRects.X - roomRects.Y) / 2),
                                (int)(position.Y + (roomRects.Y + padding) * compassPos.Y),
                                (int)roomRects.Y, (int)roomRects.Y),
                                Color.White);
            }


            // Draw player pointer in current room
            Point playerPos = model.GetPlayerPosition();

            Texture2D playerPointer;
            playerPointer = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            playerPointer.SetData(new Color[] { Color.LimeGreen });

            spriteBatch.Draw(playerPointer, new Rectangle((int)(position.X + (roomRects.X + padding) * playerPos.X + (roomRects.X - roomRects.Y) / 2),
                            (int)(position.Y + (roomRects.Y + padding) * playerPos.Y),
                            (int)roomRects.Y, (int)roomRects.Y),
                            Color.White);

        }

        public void Update(GameTime gameTime)
        {
            // throw new NotImplementedException();
        }
    }
}
