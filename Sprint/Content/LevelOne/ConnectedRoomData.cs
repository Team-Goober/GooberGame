using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint.Levels;
using Sprint.Loader;
using XMLData;

namespace Sprint.Content.LevelOne;

public class ConnectedRoomData
{
    private RoomListData roomListData;
    public List<XMLData.RoomData> Room;
    private int[,] generatedGrid;

    public ConnectedRoomData(int[,] generatedGrid)
    {
        roomListData = Goober.content.Load<RoomListData>("LevelOne/RoomsList");
        Room = roomListData.Room;
        this.generatedGrid = generatedGrid;
    }

    /// <summary>
    /// Adds data to Rooms to know their neighbors and where doors need to be
    /// </summary>
    public void ConnectRoomData()
    {
        // Load all rooms by index using RoomLoader
        var loc = new Point(0, 0);
        foreach (var roomIndex in generatedGrid)
        {
            if (roomIndex != 0)
            {
                checkForNeighborRoom(roomIndex, loc);
            }

            //increment coordinates
            loc.X++;
            if (loc.X == LevelGeneration.Columns)
            {
                loc.X = 0;
                loc.Y++;
            }
        }
    }

    /// <summary>
    /// Check for neighbors on either side
    /// </summary>
    /// <param name="roomIndex">Index of the current room to connect</param>
    /// <param name="loc">Location of the current room</param>
    void checkForNeighborRoom(int roomIndex, Point loc)
    {
        if ( (loc.Y !=0)  && (generatedGrid[loc.Y-1,loc.X] != 0))
        {
            connectRoomUp(roomIndex, (loc.X,loc.Y) );
        }

        if ( (loc.Y != LevelGeneration.Rows-1)  && (generatedGrid[loc.Y+1,loc.X] != 0))
        {
            connectRoomDown(roomIndex, (loc.X,loc.Y + 1) );
        }

        if ( (loc.X !=0)  && (generatedGrid[loc.Y,loc.X-1] != 0))
        {
            connectRoomLeft(roomIndex, (loc.X - 1, loc.Y));
        }

        if ( (loc.X != LevelGeneration.Columns-1)  && (generatedGrid[loc.Y,loc.X+1] != 0))
        {
            connectRoomRight(roomIndex, (loc.X+1,loc.Y));
        }
    }

    /// <summary>
    /// Connect room to a room above
    /// </summary>
    /// <param name="roomIndex">Room to add exit to</param>
    void connectRoomUp(int roomIndex, (int,int) currentRoomPosition)
    {
        if (currentRoomPosition.Item2 != LevelGeneration.Rows - 1)
        {
            Room[roomIndex].TopExit = "Open";
        }

    }

    /// <summary>
    /// Connect room to a room below, check to ensure not connected to hidden rooms
    /// </summary>
    /// <param name="roomIndex">Room to add exit to</param>
    /// <param name="nextRoomID">Next room to connect to</param>
    void connectRoomDown(int roomIndex, (int,int) nextRoomPosition)
    {
        if (nextRoomPosition.Item2 != LevelGeneration.Rows - 1)
        {
            Room[roomIndex].BottomExit = "Open";
        }
    }

    /// <summary>
    /// Connect room to a room to the left, check to ensure not connected to hidden rooms
    /// </summary>
    /// <param name="roomIndex">Room to add exit to</param>
    /// <param name="nextRoomID">Next room to connect to</param>
    void connectRoomLeft(int roomIndex, (int,int) nextRoomPosition)
    {
        if (nextRoomPosition.Item2 != LevelGeneration.Rows - 1)
        {
            Room[roomIndex].LeftExit = "Open";
        }
    }

    /// <summary>
    /// Connect room to a room to the right
    /// </summary>
    /// <param name="roomIndex">Room to add exit to</param>
    void connectRoomRight(int roomIndex, (int,int) nextRoomPosition)
    {
        if (nextRoomPosition.Item2 != LevelGeneration.Rows - 1)
        {
            Room[roomIndex].RightExit = "Open";
        }
    }

}