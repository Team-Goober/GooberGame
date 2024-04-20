using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint.Characters;
using Sprint.Collision;
using Sprint.Functions.SecondaryItem;
using Sprint.Functions.RoomTransition;
using Sprint.Functions.Music;
using Sprint.GameStates;
using Sprint.HUD;
using Sprint.Input;
using Sprint.Interfaces;
using Sprint.Levels;
using Sprint.Functions;
using Sprint.Items;
using Sprint.Loader;
using Sprint.Sprite;
using System.Collections.Generic;
using Sprint.Music.Sfx;
using Sprint.Functions.States;

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

        private Rectangle[] doorBounds; // Bounds of the doors in each room for click-through
        private MapModel map; // Tracks revealing of rooms for UI
        private Point compassPointer; // Room indices for triforce location
        private HUDLoader hudLoader;
        private LevelGeneration levelGeneration;

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
            LevelLoader loader = new LevelLoader(contentManager, this, spriteLoader, player);
            loader.LoadLevelXML("LevelOne/Level1");

            map = new MapModel(this);

            //Load the hud
            hudLoader = new HUDLoader(contentManager, spriteLoader);
            hudLoader.LoadHUD("HUD/HUDData", loader.GetLevel(), map);

            sleeping = false;

            // enter first room
            SwitchRoom(roomStartPosition, firstRoom, Directions.STILL);


        }

        // Connect all of the signals
        private void loadDelegates ()
        {
            Inventory inventory = player.GetInventory();
            inventory.ListingUpdateEvent += hudLoader.OnListingUpdateEvent;

            inventory.SelectorChooseEvent += hudLoader.OnSelectorChooseEvent;
            ((InventoryState)game.GetInventoryState()).SelectorMoveEvent += hudLoader.OnSelectorMoveEvent;

            player.OnPlayerHealthChange += hudLoader.UpdateHeartAmount;
            player.OnPlayerMaxHealthChange += hudLoader.UpdateMaxHeartAmount;

        }

        // Disconnect all of the signals
        private void unloadDelegates()
        {
            Inventory inventory = player.GetInventory();
            inventory.ListingUpdateEvent -= hudLoader.OnListingUpdateEvent;

            inventory.SelectorChooseEvent -= hudLoader.OnSelectorChooseEvent;
            ((InventoryState)game.GetInventoryState()).SelectorMoveEvent -= hudLoader.OnSelectorMoveEvent;

            player.OnPlayerHealthChange -= hudLoader.UpdateHeartAmount;
            player.OnPlayerMaxHealthChange -= hudLoader.UpdateMaxHeartAmount;
        }

        // Generates all commands available while the player is moving in a room
        public void MakeCommands()
        {
            // Make the changes that require InventoryState
            loadDelegates();
            hudLoader.SetSlotsArray(player.GetInventory().GetAbilities());
            hudLoader.OnListingUpdateEvent(player.GetInventory().GetListing());
            InventoryState inventoryState = (InventoryState)game.GetInventoryState();
            inventoryState.SetHUD(hudLoader, new Vector2(arenaPosition.X, Goober.gameHeight - arenaPosition.Y));
            inventoryState.AttachPlayer(player);
            inventoryState.MakeCommands();

            inputTable = new InputTable();

            //Uses the ICommand interface (MoveItems.cs) to execute command for the movement of the main character sprite


            // Register single key press triggers for movement
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.W), new Walk(player, Directions.UP));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.A), new Walk(player, Directions.LEFT));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.S), new Walk(player, Directions.DOWN));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.D), new Walk(player, Directions.RIGHT));

            // Register SingleKeyReleaseTrigger for each movement direction
            inputTable.RegisterMapping(new SingleKeyReleaseTrigger(Keys.W), new ReleaseWalk(player, Directions.UP));
            inputTable.RegisterMapping(new SingleKeyReleaseTrigger(Keys.A), new ReleaseWalk(player, Directions.LEFT));
            inputTable.RegisterMapping(new SingleKeyReleaseTrigger(Keys.S), new ReleaseWalk(player, Directions.DOWN));
            inputTable.RegisterMapping(new SingleKeyReleaseTrigger(Keys.D), new ReleaseWalk(player, Directions.RIGHT));

            // Register command to stop movement when multiple movement keys are released
            Keys[] moveKeys = { Keys.A, Keys.D, Keys.W, Keys.S, Keys.Left, Keys.Right, Keys.Up, Keys.Down };
            inputTable.RegisterMapping(new MultipleKeyReleaseTrigger(moveKeys), new StopMoving(player));



            //Player uses a cast move
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Z), new Cast(player));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.X), new Cast(player));

            // Using item slots
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.Z), new UseWeaponCommand(player, 0));
            inputTable.RegisterMapping(new SingleKeyPressTrigger(Keys.X), new UseWeaponCommand(player, 1));
            inputTable.RegisterMapping(new SingleKeyReleaseTrigger(Keys.Z), new ReleaseWeaponCommand(player, 0));
            inputTable.RegisterMapping(new SingleKeyReleaseTrigger(Keys.X), new ReleaseWeaponCommand(player, 1));

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
            for (int d = 0; d < 4; d++)
            {
                inputTable.RegisterMapping(new ClickInBoundsTrigger(ClickInBoundsTrigger.MouseButton.Middle, doorBounds[d]),
                    new SwitchRoomFromDoorsCommand(this, Directions.GetDirectionFromIndex(d)));
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
            spriteBatch.Begin(SpriteSortMode.Immediate, samplerState: SamplerState.PointClamp, transformMatrix: translateMat);

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
            LevelLoader loader = new LevelLoader(contentManager, this, spriteLoader, player);
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
            player.Move(spawn - player.GetPosition());

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

        // Switch to a new win state
        public void WinScreen()
        {
            WinState win = new WinState(game, hudLoader.GetTopDisplay(), rooms[currentRoom.Y][currentRoom.X].GetScene(), player, spriteLoader, arenaPosition);
            PassToState(win);
        }

        // Switch to a new death state
        public void DeathScreen()
        {
            GameOverState death = new GameOverState(game, hudLoader);
            death.GetHUDScene(hudLoader.GetTopDisplay());
            PassToState(death);
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

        // Clear out room list
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

        public void SetDoors(Rectangle[] bounds)
        {
            doorBounds = bounds;
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

    }
}
