using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint.Characters;
using Sprint.Collision;
using Sprint.Commands;
using Sprint.Commands.SecondaryItem;
using Sprint.Functions;
using Sprint.Functions.RoomTransition;
using Sprint.GameStates;
using Sprint.HUD;
using Sprint.Input;
using Sprint.Interfaces;
using Sprint.Items;
using Sprint.Levels;
using Sprint.Loader;
using Sprint.Sprite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;

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
        private Vector2 roomStartPosition; // Location in room to begin at when not going through door
        private Point firstRoom; // Room to start the level in

        private SceneObjectManager hud; // Object manager for HUD that should persist between rooms
        private Player player; // Player game object to be moved as rooms switch

        private IDoor[,,] doorReference;
        private Rectangle[] doorBounds;
        private MapModel map; // Tracks revealing of rooms for UI
        private Point compassPointer; // Room indices for triforce location
        HUDLoader hudLoader;


        public DungeonState(Goober game, SpriteLoader spriteLoader, ContentManager contentManager)
        {
            this.game = game;
            this.contentManager = contentManager;
            this.spriteLoader = spriteLoader;


            collisionDetector = new CollisionDetector();

            player = new Player(spriteLoader, this);

            hiddenRooms = new List<Point>();
            currentRoom = new(-1, -1);

            // Load all rooms in the level from XML file
            LevelLoader loader = new LevelLoader(contentManager, this, spriteLoader);
            loader.LoadLevelXML("LevelOne/Level1");

            map = new MapModel(this);

            //Load the hud
            hudLoader = new HUDLoader(contentManager, spriteLoader);
            hudLoader.LoadHUD("HUD/HUDData", loader.GetLevel(), map);
            hud = hudLoader.GetTopDisplay();

            loadDelegates();

            // enter first room
            SwitchRoom(roomStartPosition, firstRoom, Directions.STILL);
        }

        private void loadDelegates ()
        {
            Inventory inventory = player.GetInventory();
            inventory.InventoryEvent += hudLoader.OnInventoryEvent;
            inventory.InventoryEvent += this.OnInventoryEvent;

        }

        public void OnInventoryEvent(ItemType it, int prev, int next)
        {
            switch (it)
            {
                case ItemType.Paper:
                    map.RevealAll();
                    break;
                case ItemType.Compass:
                    map.PlaceCompass();
                    break;
                default:
                    break;
            }
        }

        // Generates all commands available while the player is moving in a room
        public void MakeCommands()
        {
            ((InventoryState)game.InventoryState).SetHUD(hudLoader, new Vector2(arenaPosition.X, Goober.gameHeight - arenaPosition.Y));

            inputTable = new InputTable();

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

            // Shooting items commands
            //Arrow
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D1), new ShootArrowCommand(player.GetProjectileFactory()));
            //Blue Arrow
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D2), new ShootBlueArrowC(player.GetProjectileFactory()));
            //Bomb
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D3), new ShootBombC(player.GetProjectileFactory()));
            //Boomarang
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D4), new ShootBoomarangC(player.GetProjectileFactory()));
            //FireBall
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D5), new ShootFireBallC(player.GetProjectileFactory()));
            //Blue Boomerang
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D6), new ShootBlueBoomerangC(player.GetProjectileFactory()));

            // Reset command
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.R), new Reset(this));

            // Switching rooms
            inputTable.RegisterMapping(new SingleClickTrigger(SingleClickTrigger.MouseButton.Right), new NextRoomCommand(this));
            inputTable.RegisterMapping(new SingleClickTrigger(SingleClickTrigger.MouseButton.Left), new PrevRoomCommand(this));

            // Switching to new pause state
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Escape), new PauseCommand(game, this));

            // Switching to the inventory state
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.I), new OpenInventoryCommand(this));

            // Middle click through doors
            for (int i = 0; i < doorReference.GetLength(0); i++)
            {
                IDoor[,] slice = new IDoor[doorReference.GetLength(1), doorReference.GetLength(2)];
                for (int r = 0; r < doorReference.GetLength(1); r++)
                {
                    for (int c = 0; c < doorReference.GetLength(2); c++)
                    {
                        slice[r, c] = doorReference[i, r, c];
                    }
                }
                inputTable.RegisterMapping(new ClickInBoundsTrigger(ClickInBoundsTrigger.MouseButton.Middle, doorBounds[i]),
                    new SwitchRoomFromDoorsCommand(slice, this));
            }

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
            hud.EndCycle();


            // Complete additions and deletions
            currRoom.EndCycle();

            // Test for collisions in room
            collisionDetector.Update(gameTime, currRoom.GetMovers(), currRoom.GetStatics());

            // Complete additions and deletions resulting from collisions
            currRoom.EndCycle();
        }

        public void PassToState(IGameState newState)
        {
            game.GameState = newState;
            if (inputTable != null)
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

            // Clear previous data
            currentRoom = new Point(-1, -1);
            ClearRooms(rooms.Length, rooms[0].Length);


            inputTable.ClearDictionary();

            // new player
            player = new Player(spriteLoader, this);

            hiddenRooms = new List<Point>();

            // reload the level
            LevelLoader loader = new LevelLoader(contentManager, this, spriteLoader);
            loader.LoadLevelXML("LevelOne/Level1");

            map = new MapModel(this);

            //reload the hud
            hudLoader = new HUDLoader(contentManager, spriteLoader);
            hudLoader.LoadHUD("HUD/HUDData", loader.GetLevel(), map);
            hud = hudLoader.GetTopDisplay();

            loadDelegates();

            // remake commands
            MakeCommands();

            // enter first room
            SwitchRoom(roomStartPosition, firstRoom, Directions.STILL);
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

            // Terminal position for a fully scrolled arena
            Vector2 max = -dir * (new Vector2(Goober.gameWidth, Goober.gameHeight) - arenaPosition);

            // Set all the start and end positions for the scenes
            Dictionary<SceneObjectManager, Vector4> scrollScenes = new();
            if (currentRoom.X >= 0)
                scrollScenes.Add(rooms[currentRoom.Y][currentRoom.X], new Vector4(arenaPosition.X, arenaPosition.Y, max.X + arenaPosition.X, max.Y + arenaPosition.Y));
            scrollScenes.Add(rooms[idx.Y][idx.X], new Vector4(-max.X + arenaPosition.X, -max.Y + arenaPosition.Y, arenaPosition.X, arenaPosition.Y));
            scrollScenes.Add(hud, Vector4.Zero);

            // Only scroll if direction isn't still
            // This is so the .75 seconds aren't spent pausing
            if (dir != Directions.STILL)
            {
                // Create new GameState to scroll and then set back to this state
                TransitionState scroll = new TransitionState(game, scrollScenes, 0.75f, this);

                PassToState(scroll);
            }

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
            SwitchRoom(roomStartPosition, target, Directions.STILL);
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
            SwitchRoom(roomStartPosition, target, Directions.STILL);
        }

        public void OpenInventory()
        {
            // Set all the start and end positions for the scenes
            Dictionary<SceneObjectManager, Vector4> scrollScenes = new()
            {
                { rooms[currentRoom.Y][currentRoom.X], new Vector4(arenaPosition.X, arenaPosition.Y, arenaPosition.X, Goober.gameHeight) },
                { ((InventoryState)game.InventoryState).GetScene(), new Vector4(-arenaPosition.X, -Goober.gameHeight + arenaPosition.Y, -arenaPosition.X, 0) },
                { hud, new Vector4(0, 0, 0, Goober.gameHeight - arenaPosition.Y) }
            };

            // Create new GameState to scroll and then set back to this state
            TransitionState scroll = new TransitionState(game, scrollScenes, 0.75f, game.InventoryState);

            PassToState(scroll);
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

        public void SetDoors(IDoor[,,] doors, Rectangle[] bounds)
        {
            doorReference = doors;
            doorBounds = bounds;
        }
        public IDoor[,,] GetDoors()
        {
            return doorReference;
        }

        public MapModel GetMap()
        {
            return map;
        }

        public void SetCompassPointer(Point room)
        {
            compassPointer = room;
        }

        public void SetStart(Vector2 posInRoom, Point firstRoom)
        {
            this.firstRoom = firstRoom;
            this.roomStartPosition = posInRoom;
        }

        public Point GetCompassPointer()
        {
            return compassPointer;
        }

        public List<SceneObjectManager> AllObjectManagers()
        {
            List<SceneObjectManager> list = new()
            {
                rooms[currentRoom.Y][currentRoom.X],
                hud
            };
            return list;
        }
    }
}
