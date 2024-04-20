using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Sprint.Levels;

public class LevelGeneration
{
    public const int Rows = 9;
    public const int Columns = 8;
    private int totalRooms = 41;
    private int numOfStepsToTake = 20;
    private (int, int) mapCoordinates = (4, 4); //Also start room for algorithm
    public int[,] mapGrid = new int[Rows, Columns];
    private Random randomObject = new Random();
    private int randomDirection;
    private int randomRoomID;
    private Dictionary<int, Delegate> functionMap;
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
    /// Create the procedurally generated map, guaranteeing win condition and shop spawn
    /// </summary>
    public void CreateRoomGrid()
    {
        //Initialize levelIndexSet
        for (var index = 0; index <= totalRooms; index++)
        {
            levelIndexSet.Add(index);
        }
        //Remove unwanted rooms
        levelIndexSet.Remove(0);
        levelIndexSet.Remove(1);
        levelIndexSet.Remove(4);
        levelIndexSet.Remove(5);
        levelIndexSet.Remove(18);
        levelIndexSet.Remove(19);
        levelIndexSet.Remove(20);


        //Loop until max rooms are made
        int stepsTaken = 0;
        while (stepsTaken < numOfStepsToTake)
        {
            //If space is empty, add room
            if (mapGrid[mapCoordinates.Item2, mapCoordinates.Item1] == 0)
            {
                //Generate random room
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
            }

            //Move to next room
            randomDirection = randomObject.Next(1, 5);
            mapCoordinates = ((int, int))functionMap[randomDirection].DynamicInvoke();
            stepsTaken++;
        }
        //Ensure win condition
        var loc = (0, 0);
        for(int index = 0; index < (Rows*Columns); index++)
        {
            var roomIndex = mapGrid[loc.Item2, loc.Item1];
            if (roomIndex != 0 && loc != (4,4) )
            {
                mapGrid[loc.Item2, loc.Item1] = 4;
                break;
            }
            //increment coordinates
            loc.Item1++;
            if (loc.Item1 == Columns)
            {
                loc.Item1 = 0;
                loc.Item2++;
            }
        }

        //Ensure shop spawns
        var locOfShop = (0, 0);
        for(int index = 0; index < (Rows*Columns); index++)
        {
            var roomIndex = mapGrid[locOfShop.Item2, locOfShop.Item1];
            if (roomIndex != 0 && locOfShop != (4,4) && roomIndex != 4)
            {
                mapGrid[locOfShop.Item2, locOfShop.Item1] = 1;
                break;
            }
            //increment coordinates
            locOfShop.Item1++;
            if (locOfShop.Item1 == Columns)
            {
                locOfShop.Item1 = 0;
                locOfShop.Item2++;
            }
        }

        //Add Gaunted Rooms to hidden row
        mapGrid[Rows-1,Columns-1] = 5;
        mapGrid[Rows-1,0] = 20;
        mapGrid[Rows - 1, 3] = 19;

        displayMap();
    }

    /// <summary>
    /// Display the map in the console for debugging
    /// </summary>
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

    /// <summary>
    /// Move the room creator up one position
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Move the room creator down one position
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Move the room creator left one position
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Move the room creator right one position
    /// </summary>
    /// <returns></returns>
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