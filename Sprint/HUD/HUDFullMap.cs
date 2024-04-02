using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint.Interfaces;
using System;
using System.IO;

namespace Sprint.HUD
{
    internal class HUDFullMap : IHUD
    {

        private Vector2 position;
        private MapModel model;
        private Vector2 roomRects;
        private int padding;
        private Vector2 bgSize;

        public HUDFullMap(MapModel model, Vector2 position, Vector2 roomSize, int padding, Vector2 bgSize)
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
            backingColor.SetData(new Color[] { new(252, 152, 56) });
            spriteBatch.Draw(backingColor, new Rectangle((int)position.X, (int)position.Y, (int)bgSize.X, (int)bgSize.Y), Color.White);

            // Draw blue rectangle for every visible room
            Texture2D roomFill;
            roomFill = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            roomFill.SetData(new Color[] { Color.Black });

            bool[,] rooms = model.GetVisitedRooms();
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

            // Draw hallways for visible doors
            bool[,,] doors = model.GetDoors();

            for (int r = 0; r < doors.GetLength(1); r++)
            {
                for (int c = 0; c < doors.GetLength(2); c++)
                {
                    for (int d = 0; d < doors.GetLength(0); d++)
                    {
                        Vector2 roomCenter = new((int)(position.X + (roomRects.X + padding) * c + roomRects.X/2),
                            (int)(position.Y + (roomRects.Y + padding) * r + roomRects.Y/2));
                        
                        // Draw if door is visible
                        if (doors[d, r, c])
                        {
                            // Calculate position of the hallway and draw there
                            Vector2 direction = Directions.GetDirectionFromIndex(d);
                            Vector2 dimensions = new((int)(padding * (1 - Math.Abs(direction.X) / 2)),
                                (int)(padding - (1 - Math.Abs(direction.Y) / 2)));
                            spriteBatch.Draw(roomFill, new Rectangle((int)(roomCenter.X + direction.X * (roomRects.X+dimensions.X) /2 - dimensions.X/2 ),
                                (int)(roomCenter.Y + direction.Y * (roomRects.Y+dimensions.Y) / 2 - dimensions.Y / 2),
                                (int)dimensions.X, (int)dimensions.Y),
                                Color.White);
                        }
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
