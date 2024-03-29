using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint.Characters;
using Sprint.Collision;
using Sprint.Commands;
using Sprint.Functions;
using Sprint.Functions.RoomTransition;
using Sprint.HUD;
using Sprint.Input;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Loader;
using Sprint.Sprite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sprint
{
    internal class DungeonState : IGameState
    {

        private Goober game;
        private ContentManager contentManager;
        private SpriteLoader spriteLoader;
        
        private IInputMap inputTable;
        private CollisionDetector collisionDetector;

        private bool resetGame = false; // Set to true to queue a reset on next update

        private Vector2 arenaPosition = Vector2.Zero; // Top left corner of the playable area on the screen

        private SceneObjectManager[][] rooms; // Object managers for each room. Accessed by index
        private List<Point> hiddenRooms; // Room indices not visible on map
        private Point currentRoom; // Index of currently updated room
        private SceneObjectManager hud; // Object manager for HUD that should persist between rooms
        private Player player; // Player game object to be moved as rooms switch

        private MapModel map; // Tracks revealing of rooms for UI
        private HUDMap mapUI; // UI element displaying map
        private Point compassPointer; // Room indices for triforce location


        public DungeonState(Goober game, SpriteLoader spriteLoader, ContentManager contentManager)
        {
            this.game = game;
            this.contentManager = contentManager;
            this.spriteLoader = spriteLoader;

            inputTable = new InputTable();
            collisionDetector = new CollisionDetector();

            player = new Player(inputTable, spriteLoader, this);

            hiddenRooms = new List<Point>();

            //Load the hud
            HUDLoader hudLoader = new HUDLoader(contentManager, spriteLoader);
            hud = hudLoader.GetScenes();

            // Load all rooms in the level from XML file
            LevelLoader loader = new LevelLoader(contentManager, this, spriteLoader, inputTable);
            loader.LoadLevelXML("LevelOne/Level1");
            makeCommands();
        }

        // Generates all commands available while the player is moving in a room
        private void makeCommands()
        {
            //Uses the ICommand interface (MoveItems.cs) to execute command for the movement of the main character sprite
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.A), new MoveLeft(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D), new MoveRight(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.W), new MoveUp(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.S), new MoveDown(player));

            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Left), new MoveLeft(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Right), new MoveRight(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Up), new MoveUp(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Down), new MoveDown(player));

            Keys[] moveKeys = { Keys.A, Keys.D, Keys.W, Keys.S, Keys.Left, Keys.Right, Keys.Up, Keys.Down };
            inputTable.RegisterMapping(new MultipleKeyReleaseTrigger(moveKeys), new StopMoving(player));

            //Melee Regular Sword Attack
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Z), new Melee(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.N), new Melee(player));

            //Player uses a cast move
            // TODO: shouldnt bind separately from shoot commands
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D1), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D2), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D3), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D4), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D5), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D6), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D7), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D8), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D9), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D0), new Cast(player));

            // Reset command
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.R), new Reset(this));

            // Switching rooms
            inputTable.RegisterMapping(new SingleClickTrigger(SingleClickTrigger.MouseButton.Right), new NextRoomCommand(this));
            inputTable.RegisterMapping(new SingleClickTrigger(SingleClickTrigger.MouseButton.Left), new PrevRoomCommand(this));

            // Switching to new pause state
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Escape), new PauseCommand(game, this));

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            // Translate the game arena to the correct location on screen
            Matrix translateMat = Matrix.CreateTranslation(new Vector3(arenaPosition.X, arenaPosition.Y, 0));
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: translateMat);

            // Draw current room's objects
            SceneObjectManager currRoom = rooms[currentRoom.Y][currentRoom.X];
            foreach (IGameObject obj in currRoom.GetObjects())
                obj.Draw(spriteBatch, gameTime);


            spriteBatch.End();
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
   
            // Draw HUD
            foreach (IGameObject obj in hud.GetObjects())
                obj.Draw(spriteBatch, gameTime);

            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {

            // Resets the game when requested
            if (resetGame)
            {
                ResetGame();
                resetGame = false;
                // End cycle to complete additions and deletions
                rooms[currentRoom.Y][currentRoom.X].EndCycle();
            }

            SceneObjectManager currRoom = rooms[currentRoom.Y][currentRoom.X];

            // Detect inputs and execute commands
            inputTable.Update(gameTime);

            // Update room objects
            foreach (IGameObject obj in currRoom.GetObjects())
                obj.Update(gameTime);
            // Update HUD
            foreach (IGameObject obj in hud.GetObjects())
                obj.Update(gameTime);

            // Complete additions and deletions
            currRoom.EndCycle();
            hud.EndCycle();

            // Test for collisions in room
            collisionDetector.Update(gameTime, currRoom.GetMovers(), currRoom.GetStatics());

            // Complete additions and deletions resulting from collisions
            currRoom.EndCycle();
        }

        public void PassToState(IGameState newState)
        {
            game.GameState = newState;
            inputTable.Sleep();
        }

        //checks if the user requested a reset for game
        public void ResetReq()
        {
            resetGame = true;
        }

        //clears input dictionary and object manager
        public void ResetGame()
        {
            // delete all game objects
            hud.ClearObjects();

            hud.EndCycle();

            ClearRooms(0, 0);

            inputTable.ClearDictionary();

            // new player
            player = new Player(inputTable, spriteLoader, this);

            //reload the hud
            HUDLoader hudLoader = new HUDLoader(contentManager, spriteLoader);
            hud = hudLoader.GetScenes();

            // reload the level
            LevelLoader loader = new LevelLoader(contentManager, this, spriteLoader, inputTable);
            loader.LoadLevelXML("LevelOne/Level1");

            // remake commands
            makeCommands();
        }

        public void AddRoom(Point loc, SceneObjectManager room, bool hidden = false)
        {
            rooms[loc.Y][loc.X] = room;
            // Hide from map if requested
            if (hidden)
            {
                hiddenRooms.Add(loc);
            }
        }

        // Switches current room to a different one by index
        public void SwitchRoom(Vector2 spawn, Point idx, Vector2 dir)
        {
            // Create new GameState to scroll and then set back to this state
            TransitionState scroll = new TransitionState(game, new List<SceneObjectManager> { rooms[currentRoom.Y][currentRoom.X] }, 
                new List<SceneObjectManager> { rooms[idx.Y][idx.X] }, new List<SceneObjectManager> { hud },
                dir, 0.75f, arenaPosition, this);

            PassToState(scroll);

            // Clean up previous room changes
            rooms[idx.Y][idx.X].EndCycle();
            // Move player to new room
            player.SetScene(rooms[idx.Y][idx.X]);
            currentRoom = idx;
            player.MoveTo(spawn);

            // Update map for change
            map.MovePlayer(idx);
        }

        // Finds the next room to the right or down 
        public void SwitchToNext()
        {
            Point target = new(-1, -1);
            while(target.X < 0 || rooms[target.Y][target.X] == null)
            {
                if (target.X < 0)
                    target = currentRoom;
                target = new Point(target.X + 1, target.Y);
                if (target.X >= rooms[0].Length)
                {
                    target = new Point(0, (target.Y + 1) % rooms.Length);
                }
            }
            SwitchRoom(new Vector2(512, 572), target, Directions.STILL);
        }

        // Finds the next room to the left or up
        public void SwitchToPrevious()
        {
            Point target = new(-1, -1);
            while (target.X < 0 || rooms[target.Y][target.X] == null)
            {
                if (target.X < 0)
                    target = currentRoom;
                target = new Point(target.X - 1, target.Y);
                if (target.X < 0)
                {
                    target = new Point(rooms[0].Length - 1, (target.Y - 1 + rooms.Length) % rooms.Length);
                }
            }
            SwitchRoom(new Vector2(512, 572), target, Directions.STILL);
        }

        public Point RoomIndex()
        {
            return currentRoom;
        }

        public SceneObjectManager GetRoomAt(Point p)
        {
            return rooms[p.Y][p.X];
        }

        public bool IsHidden(Point p)
        {
            return hiddenRooms.Contains(p);
        }

        public int RoomColumns()
        {
            return rooms[0].Length;
        }

        public int RoomRows()
        {
            return rooms.Length;
        }

        public void ClearRooms(int rows, int cols)
        {
            rooms = new SceneObjectManager[rows][];
            for (int i = 0; i < rows; i++)
            {
                rooms[i] = new SceneObjectManager[cols];
            }
        }

        public void SetArenaPosition(Vector2 pos)
        {
            arenaPosition = pos;
        }

        public void CreateMap(MapModel newMap)
        {
            map = newMap;
            mapUI = new HUDMap(map, new Vector2(16 * 4, 8 * 4));
            hud.Add(mapUI);
        }

        public MapModel GetMap()
        {
            return map;
        }

        public void SetCompassPointer(Point room)
        {
            compassPointer = room;
        }

        public Point GetCompassPointer()
        {
            return compassPointer;
        }
    }
}
