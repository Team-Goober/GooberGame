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
using System.Collections.Generic;
using System.Diagnostics;
using Sprint.Functions.Music;
using Sprint.Music.Sfx;
using Sprint.Door;

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

        private Room[][] rooms; // Object managers for each room. Accessed by index
        private Point currentRoom; // Index of currently updated room
        private Vector2 roomStartPosition; // Location in room to begin at when not going through door
        private Point firstRoom; // Room to start the level in

        private Player player; // Player game object to be moved as rooms switch

        private IDoor[,,] doorReference;
        private Rectangle[] doorBounds;
        private MapModel map; // Tracks revealing of rooms for UI
        private Point compassPointer; // Room indices for triforce location
        private HUDLoader hudLoader;

        private bool sleeping; // True when state isnt being updated

        public DungeonState(Goober game, SpriteLoader spriteLoader, ContentManager contentManager)
        {
            this.game = game;
            this.contentManager = contentManager;
            this.spriteLoader = spriteLoader;


            collisionDetector = new CollisionDetector();

            player = new Player(spriteLoader, this);

            currentRoom = new(-1, -1);

            // Load all rooms in the level from XML file
            LevelLoader loader = new LevelLoader(contentManager, this, spriteLoader);
            loader.LoadLevelXML("LevelOne/Level1");

            map = new MapModel(this);

            //Load the hud
            hudLoader = new HUDLoader(contentManager, spriteLoader);
            hudLoader.LoadHUD("HUD/HUDData", loader.GetLevel(), map);

            sleeping = false;

            // enter first room
            SwitchRoom(roomStartPosition, firstRoom, Directions.STILL);

        }

        private void loadDelegates ()
        {
            Inventory inventory = player.GetInventory();
            inventory.InventoryEvent += hudLoader.OnInventoryEvent;
            inventory.InventoryEvent += this.OnInventoryEvent;
            inventory.SelectorChooseEvent += hudLoader.OnSelectorChooseEvent;
            ((InventoryState)game.GetInventoryState()).SelectorMoveEvent += hudLoader.OnSelectorMoveEvent;
        }

        private void unloadDelegates()
        {
            Inventory inventory = player.GetInventory();
            inventory.InventoryEvent -= hudLoader.OnInventoryEvent;
            inventory.InventoryEvent -= this.OnInventoryEvent;
            inventory.SelectorChooseEvent -= hudLoader.OnSelectorChooseEvent;
            ((InventoryState)game.GetInventoryState()).SelectorMoveEvent -= hudLoader.OnSelectorMoveEvent;
        }

        public void OnInventoryEvent(ItemType it, int prev, int next, List<ItemType> ownedUpgrades)
        {
            switch (it)
            {
                case ItemType.Map:
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
            // TODO: These are here because they refer to InventoryState, which may not exist yet. Should be moved
            ((InventoryState)game.GetInventoryState()).SetHUD(hudLoader, new Vector2(arenaPosition.X, Goober.gameHeight - arenaPosition.Y));
            ((InventoryState)game.GetInventoryState()).AttachPlayer(player);
            loadDelegates();

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
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.X), new Cast(player));

            // Using item slot B
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.X), new UseBWeaponCommand(player.GetProjectileFactory(), player.GetInventory()));

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

            // SFX and Song controls
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.L), new MusicUp());
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.J), new MusicDown());
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.K), new MusicMuteToggle());

            }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            // Translate the game arena to the correct location on screen
            Matrix translateMat = Matrix.CreateTranslation(new Vector3(arenaPosition.X, arenaPosition.Y, 0));
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: translateMat);

            // Draw current room's objects
            SceneObjectManager currRoom = rooms[currentRoom.Y][currentRoom.X].GetScene();
            foreach (IGameObject obj in currRoom.GetObjects())
                obj.Draw(spriteBatch, gameTime);


            spriteBatch.End();
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
   
            // Draw HUD
            foreach (IGameObject obj in hudLoader.GetTopDisplay().GetObjects())
                obj.Draw(spriteBatch, gameTime);

            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            // Wake up if sleeping
            if (sleeping)
            {
                sleeping = false;
                // Resume sounds in current room
                if (currentRoom.X >= 0)
                    SfxFactory.GetInstance().ResumeLoopsForObjects(rooms[currentRoom.Y][currentRoom.X].GetScene().GetObjects());
            }


            // Resets the game when requested
            if (resetGame)
            {
                ResetGame();
                resetGame = false;
                // End cycle to complete additions and deletions
                rooms[currentRoom.Y][currentRoom.X].GetScene().EndCycle();
            }

            SceneObjectManager currRoom = rooms[currentRoom.Y][currentRoom.X].GetScene();

            // Detect inputs and execute commands
            inputTable.Update(gameTime);

            // Update room objects
            foreach (IGameObject obj in currRoom.GetObjects())
                obj.Update(gameTime);

            // Update HUD
            foreach (IGameObject obj in hudLoader.GetTopDisplay().GetObjects())
                obj.Update(gameTime);
            hudLoader.GetTopDisplay().EndCycle();


            // Complete additions and deletions
            currRoom.EndCycle();

            // Test for collisions in room
            collisionDetector.Update(gameTime, currRoom.GetMovers(), currRoom.GetStatics());

            // Complete additions and deletions resulting from collisions
            currRoom.EndCycle();

            CheckPuzzle(); // TESTING
        }

        public void PassToState(IGameState newState)
        {

            // Pause sounds in current room
            if (currentRoom.X >= 0)
                SfxFactory.GetInstance().PauseLoopsForObjects(rooms[currentRoom.Y][currentRoom.X].GetScene().GetObjects());

            game.GameState = newState;

            // Sleep updates that need it
            if (inputTable != null)
                inputTable.Sleep();
            sleeping = true;
        }

        //checks if the user requested a reset for game
        public void ResetReq()
        {
            resetGame = true;
        }

        //clears input dictionary and object manager
        public void ResetGame()
        {
            // Stop sounds
            SfxFactory.GetInstance().EndAllLoops();

            // remove events
            unloadDelegates();

            // delete all game objects
            hudLoader.GetTopDisplay().ClearObjects();

            hudLoader.GetTopDisplay().EndCycle();

            // Clear previous data
            currentRoom = new Point(-1, -1);
            ClearRooms(rooms.Length, rooms[0].Length);


            inputTable.ClearDictionary();

            // new player
            player = new Player(spriteLoader, this);

            // reload the level
            LevelLoader loader = new LevelLoader(contentManager, this, spriteLoader);
            loader.LoadLevelXML("LevelOne/Level1");

            map = new MapModel(this);

            //reload the hud
            hudLoader = new HUDLoader(contentManager, spriteLoader);
            hudLoader.LoadHUD("HUD/HUDData", loader.GetLevel(), map);

            ((InventoryState)game.GetInventoryState()).Reset();

            // remake commands and delegates
            MakeCommands();

            sleeping = false;

            // enter first room
            SwitchRoom(roomStartPosition, firstRoom, Directions.STILL);
        }

        public void AddRoom(Point loc, Room room, bool hidden = false)
        {
            rooms[loc.Y][loc.X] = room;
        }

        // Switches current room to a different one by index
        public void SwitchRoom(Vector2 spawn, Point idx, Vector2 dir)
        {


            // Terminal position for a fully scrolled arena
            Vector2 max = -dir * (new Vector2(Goober.gameWidth, Goober.gameHeight) - arenaPosition);

            // Set all the start and end positions for the scenes
            Dictionary<SceneObjectManager, Vector4> scrollScenes = new();
            if (currentRoom.X >= 0)
                scrollScenes.Add(rooms[currentRoom.Y][currentRoom.X].GetScene(), new Vector4(arenaPosition.X, arenaPosition.Y, max.X + arenaPosition.X, max.Y + arenaPosition.Y));
            scrollScenes.Add(rooms[idx.Y][idx.X].GetScene(), new Vector4(-max.X + arenaPosition.X, -max.Y + arenaPosition.Y, arenaPosition.X, arenaPosition.Y));
            scrollScenes.Add(hudLoader.GetTopDisplay(), Vector4.Zero);

            // Only scroll if direction isn't still
            // This is so the .75 seconds aren't spent pausing
            if (dir != Directions.STILL)
            {
                // Create new GameState to scroll and then set back to this state
                TransitionState scroll = new TransitionState(game, scrollScenes, 0.75f, this);

                PassToState(scroll);
            }
            else if(currentRoom.X >= 0)
            {
                // Pause sounds in current room
                // If direction isn't still, sounds will pause during the PassToState method
                SfxFactory.GetInstance().PauseLoopsForObjects(rooms[currentRoom.Y][currentRoom.X].GetScene().GetObjects());
                sleeping = true;
            }

            // Clean up previous room changes
            rooms[idx.Y][idx.X].GetScene().EndCycle();
            // Move player to new room
            player.SetRoom(rooms[idx.Y][idx.X]);
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
            InventoryState inventory = (InventoryState)game.GetInventoryState();
            // Set all the start and end positions for the scenes
            Dictionary<SceneObjectManager, Vector4> scrollScenes = new()
            {
                { rooms[currentRoom.Y][currentRoom.X].GetScene(), new Vector4(arenaPosition.X, arenaPosition.Y, arenaPosition.X, Goober.gameHeight) },
                { inventory.GetScene(), new Vector4(-arenaPosition.X, -Goober.gameHeight + arenaPosition.Y, -arenaPosition.X, 0) },
                { hudLoader.GetTopDisplay(), new Vector4(0, 0, 0, Goober.gameHeight - arenaPosition.Y) }
            };

            // Create new GameState to scroll and then set back to this state
            TransitionState scroll = new TransitionState(game, scrollScenes, 0.75f, inventory);

            PassToState(scroll);
        }

        public Point RoomIndex()
        {
            return currentRoom;
        }

        public Room GetRoomAt(Point p)
        {
            return rooms[p.Y][p.X];
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
            rooms = new Room[rows][];
            for (int i = 0; i < rows; i++)
            {
                rooms[i] = new Room[cols];
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

        private bool solved1 = false;
        private bool solved2 = false;

        public void CheckPuzzle()
        {
            //Puzzle are in index: 0, 3, 4 
             if (currentRoom == new Point(2, 4) && !solved2)
            {
                Room room = GetRoomAt(new Point(2, 4));
                SceneObjectManager scene = room.GetScene();
                List<IDoor> doors = room.GetDoors();
                foreach (IGameObject g in scene.GetObjects())
                {
                    if (g is MoveWallTile)
                    {
                        (Vector2, Vector2) pos = ((MoveWallTile)g).GetPosition();
                        solved1 = pos.Item1 != pos.Item2;
                    }
                }
            } else if (currentRoom == new Point(5, 3) && !solved2)
            {
                Room room = GetRoomAt(new Point(5, 3));
                List<Character> npc = room.GetNpcs();
                List<IDoor> doors = room.GetDoors();

                if(npc.Count == 0)
                {
                    solved2 = true;
                }
                
            }

            if(solved1)
            {
                Room room = GetRoomAt(new Point(2, 4));
                List<IDoor> doors = room.GetDoors();
                foreach (IDoor g in doors)
                {
                    if(g is PuzzleDoor)
                    {
                        g.SetOpen(true);
                    }
                }
            }

            if(solved2)
            {
                Room room = GetRoomAt(new Point(5, 3));
                List<IDoor> doors = room.GetDoors();
                foreach (IDoor g in doors)
                {
                    if (g is PuzzleDoor)
                    {
                        g.SetOpen(true);
                    }
                }
            }
        }

    }
}
