using System;
using System.Collections.Generic;

namespace Sprint.Levels;

public class LevelGeneration
{
    public const int Rows = 10;
    public const int Columns = 10;
    private int numOfRoomsToGenerate = 10;
    private (int, int) mapCoordinates = (4, 4); //Also start room for algorithm
    public int[,] mapGrid = new int[Rows, Columns];
    private Random randomObject = new Random();
    private int randomDirection;
    private int randomRoomID;
    private Dictionary<int, Delegate> functionMap;
    private int roomsCreated = 0;
    private static LevelGeneration instance;


    public LevelGeneration()
    {
        functionMap = new Dictionary<int, Delegate>();
        functionMap[1] = new Func<(int, int)>(MoveUp);
        functionMap[2] = new Func<(int, int)>(MoveDown);
        functionMap[3] = new Func<(int, int)>(MoveLeft);
        functionMap[4] = new Func<(int, int)>(MoveRight);
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
        while (roomsCreated < 10)
        {
            if (mapGrid[mapCoordinates.Item1, mapCoordinates.Item2] == 0)
            {
                randomRoomID = randomObject.Next(1,18);
                mapGrid[mapCoordinates.Item1, mapCoordinates.Item2] = randomRoomID;
                roomsCreated++;
            }

            randomDirection = randomObject.Next(1, 5);
            mapCoordinates = ((int, int))functionMap[randomDirection].DynamicInvoke();
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
        if (mapCoordinates.Item2 == (Columns -1) || mapGrid[mapCoordinates.Item1, mapCoordinates.Item2 + 1] != 0)
        {
            return mapCoordinates;
        }

        mapCoordinates.Item2++;
        return mapCoordinates;
    }

    private (int, int) MoveDown()
    {
        if (mapCoordinates.Item2 == 0 || mapGrid[mapCoordinates.Item1, mapCoordinates.Item2 - 1] != 0 )
        {
            return mapCoordinates;
        }

        mapCoordinates.Item2--;
        return mapCoordinates;
    }

    private (int, int) MoveLeft()
    {
        if (mapCoordinates.Item1 == 0 || mapGrid[mapCoordinates.Item1 - 1, mapCoordinates.Item2] != 0 )
        {
            return mapCoordinates;
        }

        mapCoordinates.Item1--;
        return mapCoordinates;
    }

    private (int, int) MoveRight()
    {
        if ( mapCoordinates.Item1 == (Rows -1) || mapGrid[mapCoordinates.Item1 + 1, mapCoordinates.Item2] != 0 )
        {
            return mapCoordinates;
        }

        mapCoordinates.Item1++;
        return mapCoordinates;
    }
}