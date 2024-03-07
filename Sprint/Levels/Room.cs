using System.Collections.Generic;
using Sprint.Characters;

namespace Sprint.Levels
{
    internal class Room
    {

        private BackgroundTexture background;
        private List<InvisibleWall> walls;
        private List<Tiles> tiles;
        private Dictionary<Character.Directions, Door> doors;
        private List<Item> items;
        private List<Enemy> enemies;

        public Room()
        {
            walls = new();
            tiles = new();
            doors = new();
            items = new();
            enemies = new();
        }

        /// <summary>
        /// Loads this room into the game world
        /// </summary>
        /// <param name="direction">Direction of door to enter the room through</param>
        /// <param name="objectManager">GameObjectManager to add objects to</param>
        public void Enter(Character.Directions direction, GameObjectManager objectManager)
        {
            // TODO: teleport player?
            objectManager.Add(background);
            foreach (InvisibleWall wall in walls)
            {
                objectManager.Add(wall);
            }
            foreach (Tiles tile in tiles)
            {
                objectManager.Add(tile);
            }
            foreach (Door door in doors.Values)
            {
                objectManager.Add(door);
            }
            foreach (Item item in items)
            {
                objectManager.Add(item);
            }
            foreach (Enemy enemy in enemies)
            {
                objectManager.Add(enemy);
            }
        }

        /// <summary>
        /// Removes this room from the game world
        /// </summary>
        /// <param name="objectManager">GameObjectManager to remove objects from</param>
        public void Exit(GameObjectManager objectManager)
        {
            objectManager.Remove(background);
            foreach (InvisibleWall wall in walls)
            {
                objectManager.Remove(wall);
            }
            foreach (Tiles tile in tiles)
            {
                objectManager.Remove(tile);
            }
            foreach (Door door in doors.Values)
            {
                objectManager.Remove(door);
            }
            foreach (Item item in items)
            {
                objectManager.Remove(item);
            }
            foreach (Enemy enemy in enemies)
            {
                objectManager.Remove(enemy);
            }
        }


        public void SetBackground(BackgroundTexture background)
        {
            this.background = background;
        }

        public void AddInvisibleWall(InvisibleWall wall)
        {
            walls.Add(wall);
        }

        public void AddTile(Tiles tile)
        {
            tiles.Add(tile);
        }

        public void PutDoor(Character.Directions dir, Door door)
        {
            doors.Add(dir, door);
        }

        public void AddItem(Item item)
        {
            items.Add(item);
        }

        public void AddEnemy(Enemy enemy)
        {
            enemies.Add(enemy);
        }


    }
}
