

using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Sprint
{
    internal class MapModel
    {

        private bool[,] rooms; // 2D array of rooms. boolean value represents visibility
        private bool[,] horizDoors; // 2D array of doors facing left and right. boolean value represents visibility
        private bool[,] vertDoors; // 2D array of doors facing up and down. boolean value represents visibility
        private Point playerPos; // indices of player in the rooms matrix
        private Point compassPos; // indices of the compass pointer. if negative, don't show
        private bool allShown; // true if map was acquired and everything should be visible. hidden doors and rooms aren't revealed

        private DungeonState dungeon;
        private IDoor[,,] doorReference; // reference for doors per room side. first dimension is Up/Right/Down/Left, and other two represent room matrix 

        public MapModel(DungeonState dungeon, IDoor[,,] doorReference) {

            this.dungeon = dungeon;
            this.doorReference = doorReference;

            rooms = new bool[dungeon.RoomRows(), dungeon.RoomColumns()];
            horizDoors = new bool[dungeon.RoomRows(), dungeon.RoomColumns()+1];
            vertDoors = new bool[dungeon.RoomRows()+1, dungeon.RoomColumns()];
            playerPos = dungeon.RoomIndex();
            compassPos = new Point(-1, -1);
            allShown = false;

        }

        // Place the compass pointer at a room index
        public void PlaceCompass(Point pos)
        {
            compassPos = pos;
        }

        // Move the player pointer to a room index and update room visibility
        public void MovePlayer(Point newPos)
        {
            playerPos = newPos;
            revealRoom(newPos.Y, newPos.X);
        }

        // React to a door opening by making it visible
        public void OpenDoor(IDoor door)
        {
            Point roomIdx = door.GetRoomIndices();
            Vector2 dir = door.SideOfRoom();

            // door found based on side of room it is in
            if (dir == Directions.UP)
            {
                vertDoors[roomIdx.Y, roomIdx.X] = true;
            }
            else if (dir == Directions.RIGHT)
            {
                horizDoors[roomIdx.Y, roomIdx.X + 1] = true;
            }
            else if (dir == Directions.DOWN)
            {
                vertDoors[roomIdx.Y + 1, roomIdx.X] = true;
            }
            else if (dir == Directions.LEFT)
            {
                horizDoors[roomIdx.Y, roomIdx.X] = true;
            }
        }

        // Reveal all non-hidden information for the level
        public void RevealAll()
        {
            for (int i = 0; i<dungeon.RoomRows(); i++)
            {
                for (int j = 0; j < dungeon.RoomColumns(); j++)
                {
                    // Only show existing and non-hidden rooms
                    if (dungeon.GetRoomAt(new Point(j, i)) != null && !dungeon.IsHidden(new Point(j, i)))
                    {
                        revealRoom(i, j);

                    }

                }
            }
            
            allShown = true;

        }

        // Reveals a room and its doors if currently nto visible
        private void revealRoom(int r, int c)
        {
            // already revealed, no need to change anything
            if (rooms[r, c])
                return;

            rooms[r, c] = true;

            // Don't add doors if no door room
            if (doorReference[0, r, c] != null)
            {
                // reveal any open doors in the room
                // top
                if (doorReference[0, r, c].IsOpen())
                {
                    vertDoors[r, c] = true;
                }
                // right
                if (doorReference[1, r, c].IsOpen())
                {
                    horizDoors[r, c + 1] = true;
                }
                // bottom
                if (doorReference[2, r, c].IsOpen())
                {
                    vertDoors[r + 1, c] = true;
                }
                // left
                if (doorReference[3, r, c].IsOpen())
                {
                    horizDoors[r, c] = true;
                }
            }
        }

        /*
         *         private bool[,] rooms; // 2D array of rooms. boolean value represents visibility
        private bool[,] horizDoors; // 2D array of doors facing left and right. boolean value represents visibility
        private bool[,] vertDoors; // 2D array of doors facing up and down. boolean value represents visibility
        private Point playerPos; // indices of player in the rooms matrix
        private Point compassPos; // indices of the compass pointer. if negative, don't show
        private bool allShown; // true if map was acquired and everything should be visible. hidden doors and rooms aren't revealed */

        public bool[,] GetRooms()
        {
            return rooms;
        }

        public bool[,] GetHorizontalDoors()
        {
            return horizDoors;
        }

        public bool[,] GetVerticalDoors()
        {
            return vertDoors;
        }

        public Point GetPlayerPosition()
        {
            return playerPos;
        }

        public Point GetCompassPosition()
        {
            return compassPos;
        }

        public bool GetAllShown()
        {
            return allShown;
        }

    }
}
