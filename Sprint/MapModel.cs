using Microsoft.Xna.Framework;
using Sprint.Interfaces;
using Sprint.Levels;

namespace Sprint
{
    internal class MapModel
    {

        private bool[,] visitedRooms; // 2D array of rooms. boolean value represents if player has visited them
        private bool[,] revealedRooms; // 2D array of rooms. boolean value represents if the map has revealed their locations
        private bool[,,] doors; // Array of doors. First dimension is up, right, down, left. Second and third are room array. Boolean value is openness
        private Point playerPos; // indices of player in the rooms matrix
        private Point compassPos; // indices of the compass pointer. if negative, don't show

        private DungeonState dungeon;


        public MapModel(DungeonState dungeon) {

            this.dungeon = dungeon;

            visitedRooms = new bool[dungeon.RoomRows(), dungeon.RoomColumns()];
            revealedRooms = new bool[dungeon.RoomRows(), dungeon.RoomColumns()];

            doors = new bool[4, dungeon.RoomRows(), dungeon.RoomColumns()];

            playerPos = dungeon.RoomIndex();
            compassPos = new Point(-1, -1);

        }

        // Place the compass pointer at a room index
        public void PlaceCompass()
        {
            compassPos = dungeon.GetCompassPointer();
        }

        // Move the player pointer to a room index and update room visibility
        public void MovePlayer(Point newPos)
        {
            playerPos = newPos;
            enterRoom(newPos.Y, newPos.X);
        }

        // React to a door opening by making it visible
        public void OpenDoor(IDoor door)
        {
            Point roomIdx = door.GetRoomIndices();
            Vector2 dir = door.SideOfRoom();

            // door found based on side of room it is in
            doors[Directions.GetIndex(dir), roomIdx.Y, roomIdx.X] = true;
        }

        // Reveal all non-hidden information for the level
        public void RevealAll()
        {
            for (int i = 0; i<dungeon.RoomRows(); i++)
            {
                for (int j = 0; j < dungeon.RoomColumns(); j++)
                {
                    // Only show existing and non-hidden rooms
                    Room r = dungeon.GetRoomAt(new Point(j, i));
                    if (r != null && !r.IsHidden())
                    {
                        revealRoom(i, j);

                    }

                }
            }
        }

        // Reveals a room and its doors if currently not visible
        private void enterRoom(int r, int c)
        {
            // already revealed, no need to change anything
            if (visitedRooms[r, c])
                return;

            visitedRooms[r, c] = true;

            // Reveal all open directional doors 
            Room room = dungeon.GetRoomAt(new Point(c, r));
            if(room.GetDoors().Count >= 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (room.GetDoors()[i].IsOpen())
                    {
                        doors[i, r, c] = true;
                    }
                }
            }
        }

        // Reveals a room to the map
        public void revealRoom(int r, int c)
        {
            revealedRooms[r, c] = true;
        }


        public bool[,] GetVisitedRooms()
        {
            return visitedRooms;
        }

        public bool[,] GetRevealedRooms()
        {
            return revealedRooms;
        }

        public bool[,,] GetDoors()
        {
            return doors;
        }

        public Point GetPlayerPosition()
        {
            return playerPos;
        }

        public Point GetCompassPosition()
        {
            return compassPos;
        }

    }
}
