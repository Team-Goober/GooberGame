using System;
using System.Collections.Generic;
using XMLData;

namespace Sprint.Levels;

public class LevelGeneration
{
    public const int Rows = 9;
    public const int Columns = 8;
    private int totalRooms = 41;
    private int numOfRoomsToGenerate = 16;
    private (int, int) mapCoordinates = (4, 4); //Also start room for algorithm
    public int[,] mapGrid = new int[Rows, Columns];
    private Random randomObject = new Random();
    private int randomDirection;
    private int randomRoomID;
    private Dictionary<int, Delegate> functionMap;
    private int roomsCreated = 0;
    private static LevelGeneration instance;
    private HashSet<int> levelIndexSet;



    public LevelGeneration()
    {
        functionMap = new Dictionary<int, Delegate>();
        functionMap[1] = new Func<(int, int)>(MoveUp);
        functionMap[2] = new Func<(int, int)>(MoveDown);
        functionMap[3] = new Func<(int, int)>(MoveLeft);
        functionMap[4] = new Func<(int, int)>(MoveRight);
        levelIndexSet = new HashSet<int>();
    }

    /// <summary>
    /// Get an instance of the SfxFactory, create a new one on first run
    /// </summary>
    /// <returns>The instance of SfxFactory</returns>
    public static LevelGeneration GetInstance()
    {
        return instance ??= new LevelGeneration();
    }

    public void CreateRoomGrid()
    {
        //Initialize levelIndexSet
        for (var index = 0; index <= totalRooms; index++)
        {
            levelIndexSet.Add(index);
        }
        //Remove unwanted rooms
        levelIndexSet.Remove(0);
        levelIndexSet.Remove(5);
        levelIndexSet.Remove(18);
        levelIndexSet.Remove(19);
        levelIndexSet.Remove(20);


        //Loop until max rooms are made
        while (roomsCreated < numOfRoomsToGenerate)
        {
            //If space is empty, add room
            if (mapGrid[mapCoordinates.Item2, mapCoordinates.Item1] == 0)
            {
                randomRoomID = randomObject.Next(1,totalRooms);
                while (!levelIndexSet.Contains(randomRoomID))
                {
                    if (randomRoomID == totalRooms)
                    {
                        randomRoomID = 0;
                    }
                    else
                    {
                        randomRoomID++;
                    }
                }
                mapGrid[mapCoordinates.Item2, mapCoordinates.Item1] = randomRoomID;
                levelIndexSet.Remove(randomRoomID);
                roomsCreated++;
            }

            //Move to next room
            randomDirection = randomObject.Next(1, 5);
            mapCoordinates = ((int, int))functionMap[randomDirection].DynamicInvoke();
        }
        //Ensure win condition
        while (levelIndexSet.Contains(4))
        {
            if (mapGrid[mapCoordinates.Item2, mapCoordinates.Item1] == 0)
            {
                mapGrid[mapCoordinates.Item2, mapCoordinates.Item1] = 4;
                levelIndexSet.Remove(4);
            }
            randomDirection = randomObject.Next(1, 5);
            mapCoordinates = ((int, int))functionMap[randomDirection].DynamicInvoke();
        }


        //Add Rooms to hidden row
        mapGrid[Rows-1,Columns-1] = 5;
        if (!levelIndexSet.Contains(1))
        {
            mapGrid[Rows-1,0] = 20;
        }
        displayMap();
    }

    private void displayMap()
    {
        int index = 0;
        foreach (var i in mapGrid)
        {
            if (index % Columns == 0)
            {
                Console.WriteLine();
            }

            Console.Write(" " + i + " ");
            index++;
        }
    }

    private (int, int) MoveUp()
    {
        //Return if it is impossible to move up, OR if a room is already there
        if (mapCoordinates.Item2 == (0) || mapGrid[mapCoordinates.Item2 - 1, mapCoordinates.Item1] != 0)
        {
            return mapCoordinates;
        }

        mapCoordinates.Item2--;
        return mapCoordinates;
    }

    private (int, int) MoveDown()
    {
        //Return if it is impossible to move down, OR if a room is already there
        if (mapCoordinates.Item2 == Rows-2 || mapGrid[mapCoordinates.Item2+1, mapCoordinates.Item1] != 0 )
        {
            return mapCoordinates;
        }

        mapCoordinates.Item2++;
        return mapCoordinates;
    }


    private (int, int) MoveLeft()
    {
        //Return if it is impossible to move left, OR if a room is already there
        if (mapCoordinates.Item1 == 0 || mapGrid[mapCoordinates.Item2, mapCoordinates.Item1-1] != 0 )
        {
            return mapCoordinates;
        }

        mapCoordinates.Item1--;
        return mapCoordinates;
    }

    private (int, int) MoveRight()
    {
        //Return if it is impossible to move left, OR if a room is already there
        if ( mapCoordinates.Item1 == (Columns -1) || mapGrid[mapCoordinates.Item2, mapCoordinates.Item1 + 1] != 0 )
        {
            return mapCoordinates;
        }

        mapCoordinates.Item1++;
        return mapCoordinates;
    }
}